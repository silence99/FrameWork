using AopAlliance.Intercept;
using log4net;
using Spring.Aop.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Linq;

namespace Framework
{
    public class NotifiableObject : INotifyPropertyChangedEx, INotifyPropertyChangingEx
    {
        protected virtual ILog Logger { get; set; }
        protected virtual ErrorHandler ErrorHandler { get; set; }
        public virtual bool NotifiableEnable { get; set; }
        public virtual bool ReadOnlyMode { get; set; }
        protected virtual bool UIChanged { get; set; }

        public List<PropertyChangeError> Errors { get; set; }

        protected virtual object AopWapper { get; set; }

        public event PropertyChangedHandlerEx PropertyChangingEx;
        public event PropertyChangedHandlerEx PropertyChangedEx;

        private static int _initialization = 0;
        private static object _syncObj = new object();
        //private static Dictionary<NotifiableObject, Dictionary<string, PropertyChangedHandlerEx>> _notifiedChangingRepository = new Dictionary<NotifiableObject, Dictionary<string, PropertyChangedHandlerEx>>();
        //private static Dictionary<NotifiableObject, Dictionary<string, PropertyChangedHandlerEx>> _notifiedChangedRepository = new Dictionary<NotifiableObject, Dictionary<string, PropertyChangedHandlerEx>>();
        private List<string> _eventStack = new List<string>();
        private List<string> _overriden = new List<string>();


        public NotifiableObject(ErrorHandler handler = null)
        {
            Errors = new List<PropertyChangeError>();
            try
            {
                ErrorHandler = handler;
                Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                Interlocked.Increment(ref _initialization);
                Initializing();
                PostInitalize();
            }
            finally
            {
                Interlocked.Decrement(ref _initialization);
            }
        }

        public bool IsAopWapper { get { return this is IAdvised && this is IAopProxy; } }

        public object Invoke(IMethodInvocation invocation)
        {
            if (invocation.Arguments.Length != 1)
            {
                throw new Exception("only listen the property changing event of an instance");
            }

            string propertyName = invocation.Method.Name.Substring(3);
            var info = this.GetType().GetProperty(propertyName);
            PropertyChangedEventArgs args = new PropertyChangedEventArgs()
            {
                Source = this,
                NewValue = invocation.Arguments[0],
                OldValue = info.GetValue(this),
                PropertyName = propertyName,
                UserInited = _initialization == 0,
                UIChanged = UIChanged
            };
            object rval;
            lock (_syncObj)
            {
                UIChanged = false;
                RaisePropertyChangingEvent(args);
                rval = invocation.Proceed();
                RaisePropertyChangedEvent(args);
            }

            return rval;
        }

        private void RaisePropertyChangingEvent(PropertyChangedEventArgs args)
        {
            InvokeEventAndRepositoryRegisterHandlers(args, PropertyChangeEvent.Changing);
            HandleErrors();
        }

        private void RaisePropertyChangedEvent(PropertyChangedEventArgs args)
        {
            InvokeEventAndRepositoryRegisterHandlers(args, PropertyChangeEvent.Changed);
            HandleErrors();
        }

        private void HandleErrors()
        {

        }

        protected virtual void Initializing() { }

        protected virtual void PostInitalize() { }

        public bool IsInitializing
        {
            get { return _initialization == 0; }
        }

        public void SetOverriden(string name, bool overriden)
        {
            if (overriden)
            {
                if (!_overriden.Contains(name))
                {
                    _overriden.Add(name);
                }
            }
            else
            {
                if (_overriden.Contains(name))
                {
                    _overriden.Remove(name);
                }
            }
        }

        private void InvokeEventAndRepositoryRegisterHandlers(PropertyChangedEventArgs args, PropertyChangeEvent period)
        {
            //var repository = period == PropertyChangeEvent.Changing ? _notifiedChangingRepository : _notifiedChangedRepository;
            var commonEvent = period == PropertyChangeEvent.Changing ? PropertyChangingEx : PropertyChangedEx;
            var eventName = period == PropertyChangeEvent.Changed ? "Changed" : "Changing";
            try
            {
                if (!_eventStack.Contains(args.PropertyName))
                {
                    if (commonEvent != null)
                    {
                        commonEvent(this, args);
                    }
                    else
                    {
                        Logger.InfoFormat("{0} change property {1} again, {2} event don't be raised.", this.GetType().BaseType.FullName, args.PropertyName, eventName);
                    }
                }

                //if (repository.ContainsKey(this))
                //{
                //    if (repository[this] != null && repository[this][args.PropertyName] != null)
                //    {
                //        var handler = repository[this][args.PropertyName];
                //        handler(this, args);
                //    }
                //}
            }
            catch (PropertyChangeException pce)
            {
                Errors = pce.Errors;
            }
            catch (Exception ex)
            {
                PropertyChangeError error = new PropertyChangeError()
                {
                    Event = period,
                    Exceptions = new List<Exception>() { ex },
                    PropertyName = args.PropertyName
                };
                Errors.Add(error);
            }
            finally
            {
                if (Errors != null && Errors.Count != 0)
                {
                    Logger.ErrorFormat("Error occur when {0} property {1}", eventName, args.PropertyName);
                }
            }
        }

        public void ClearError(string propertyName)
        {
            Errors = Errors.Where(obj => obj.PropertyName != propertyName).ToList();
        }

        public void ChangeFromUI()
        {
            UIChanged = true;
        }
    }
}
