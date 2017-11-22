using AopAlliance.Intercept;
using log4net;
using Spring.Aop.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Framework
{
    public class NotifiableObject : INotifyPropertyChangedEx, INotifyPropertyChangingEx, INotifyPropertyChanged
    {
        protected virtual ILog Logger { get; set; }
        protected virtual ErrorHandler ErrorHandler { get; set; }
        public virtual bool NotifiableEnable { get; set; }
        public virtual bool ReadOnlyMode { get; set; }
        protected virtual bool UIChanged { get; set; }

        public List<PropertyChangeError> Errors { get; set; }

        public virtual object AopWapper { get; set; }

        public event PropertyChangedHandlerEx PropertyChangingEx;
        public event PropertyChangedHandlerEx PropertyChangedEx;
        public event PropertyChangedEventHandler PropertyChanged;

        private static int _initialization = 0;
        private static Stack<KeyValuePair<NotifiableObject, string>> _eventStack = new Stack<KeyValuePair<NotifiableObject, string>>();
        private List<string> _overriden = new List<string>();
        private List<string> _readonly = new List<string>();
        private List<string> _bindingToUI = new List<string>();
        protected Dictionary<string, bool> _disabled = new Dictionary<string, bool>();
        protected Dictionary<string, bool> _required = new Dictionary<string, bool>();
        protected Dictionary<string, object> _invisiabled = new Dictionary<string, object>();
        private static NotifierManager _notifyManager = new NotifierManager();

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

        public virtual void SetVisiable(string refProperty, object visiable) { }
        public virtual void SetRequired(string refProperty, bool required) { }
        public virtual void SetEnbaled(string refProperty, bool enabled)
        {
        }

        public void SetReadOnlyMode(bool readOnly)
        {
            ReadOnlyMode = readOnly;
        }

        public void SetReadOnly(string propertyName, bool readOnly)
        {
            if (readOnly)
            {
                if (!_readonly.Contains(propertyName))
                {
                    _readonly.Add(propertyName);
                }
            }
            else
            {
                if (_readonly.Contains(propertyName))
                {
                    _readonly.Remove(propertyName);
                }
            }
        }

        public bool IsAopWapper { get { return this is IAdvised && this is IAopProxy; } }

        public object Invoke(IMethodInvocation invocation)
        {
            string propertyName = invocation.Method.Name.Substring(3);
            if (ReadOnlyMode)
            {
                Logger.Debug("notifier is readonly");
                return invocation.Arguments[0];
            }

            if (_readonly.Contains(propertyName))
            {
                Logger.DebugFormat("notifier is readonly, property {0} can't change", propertyName);
                return invocation.Arguments[0];
            }

            if (invocation.Arguments.Length != 1)
            {
                Logger.Error("only listen the property changing event of an instance, other method not support.");
                throw new Exception("only listen the property changing event of an instance, other method not support.");
            }

            var info = this.GetType().GetProperty(propertyName);
            var val = invocation.Arguments[0];
            var oldValue = info.GetValue(this);
            if (val == oldValue)
            {
                Logger.InfoFormat("property {0} of type [{1}] not change, new value equle old value.", propertyName, this.GetType().BaseType.FullName);
                return val;
            }

            PropertyChangedEventArgsEx args = new PropertyChangedEventArgsEx(propertyName)
            {
                Source = this,
                NewValue = invocation.Arguments[0],
                OldValue = info.GetValue(this),
                UserInited = _initialization == 0,
                UIChanged = UIChanged
            };
            object rval;

            UIChanged = false;
            RaisePropertyChangingEvent(args);
            rval = invocation.Proceed();
            RaisePropertyChangedEvent(args);

            return rval;
        }

        private void RaisePropertyChangingEvent(PropertyChangedEventArgsEx args)
        {
            InvokeEventAndRepositoryRegisterHandlers(args, PropertyChangeEvent.Changing);
            HandleErrors();
        }

        private void RaisePropertyChangedEvent(PropertyChangedEventArgsEx args)
        {
            InvokeEventAndRepositoryRegisterHandlers(args, PropertyChangeEvent.Changed);
            _notifyManager.RegisterToNotify(this, args.PropertyName);
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

        private void InvokeEventAndRepositoryRegisterHandlers(PropertyChangedEventArgsEx args, PropertyChangeEvent period)
        {
            //var repository = period == PropertyChangeEvent.Changing ? _notifiedChangingRepository : _notifiedChangedRepository;
            var commonEvent = period == PropertyChangeEvent.Changing ? PropertyChangingEx : PropertyChangedEx;
            var eventName = period == PropertyChangeEvent.Changed ? "Changed" : "Changing";
            var stackitem = new KeyValuePair<NotifiableObject, string>(this, args.PropertyName);
            try
            {
                if (!_eventStack.Contains(stackitem))
                {
                    _eventStack.Push(stackitem);
                    if (commonEvent != null)
                    {
                        commonEvent(this, args);
                    }
                    else
                    {
                        Logger.InfoFormat("{0} change property {1} again, {2} event don't be raised.", this.GetType().BaseType.FullName, args.PropertyName, eventName);
                    }
                }
                else
                {
                    return;
                }
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

            _eventStack.Pop();
        }

        public void ClearError(string propertyName)
        {
            Errors = Errors.Where(obj => obj.PropertyName != propertyName).ToList();
        }

        public void ChangeFromUI()
        {
            UIChanged = true;
        }

        public virtual void ApplyToUI(string property)
        {
            throw new NotImplementedException("Apply to UI not implemented");
        }

        protected void TriggerNotifyToUIEvent(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, args);
            }
        }

        protected virtual void ApplyMetaDataToUI()
        {
            throw new NotImplementedException("Apply meta data to UI not implemented");
        }
    }
}
