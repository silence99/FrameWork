using AopAlliance.Intercept;
using Spring.Aop.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Framework
{
    public class NotifiableObject : INotifyPropertyChangedEx, INotifyPropertyChangingEx
    {
        public virtual bool NotifiableEnable { get; set; }
        public virtual bool ReadOnlyMode { get; set; }

        public List<PropertyChangeError> Errors { get; set; }

        protected virtual object AopWapper { get; set; }

        public event PropertyChangedHandlerEx PropertyChanging;
        public event PropertyChangedHandlerEx PropertyChanged;

        private static int _initialization = 0;
        private static object _syncObj = new object();
        private static Dictionary<NotifiableObject, Dictionary<string, PropertyChangedHandlerEx>> _notifiedChangingRepository = new Dictionary<NotifiableObject, Dictionary<string, PropertyChangedHandlerEx>>();
        private static Dictionary<NotifiableObject, Dictionary<string, PropertyChangedHandlerEx>> _notifiedChangedRepository = new Dictionary<NotifiableObject, Dictionary<string, PropertyChangedHandlerEx>>();
        private List<string> _overriden = new List<string>();


        public NotifiableObject()
        {
            Errors = new List<PropertyChangeError>();
            try
            {
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
                UserInit = _initialization == 0
            };

            RaisePropertyChangingEvent(args);
            object rval = invocation.Proceed();
            RaisePropertyChangedEvent(args);
            return rval;
        }

        private void RaisePropertyChangingEvent(PropertyChangedEventArgs args)
        {
            InvokeEventAndRepositoryRegisterHandlers(args, PropertyChangeEvent.Changing);
        }

        private void RaisePropertyChangedEvent(PropertyChangedEventArgs args)
        {
            InvokeEventAndRepositoryRegisterHandlers(args, PropertyChangeEvent.Changed);
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
            var repository = period == PropertyChangeEvent.Changing ? _notifiedChangingRepository : _notifiedChangedRepository;
            var commonEvent = period == PropertyChangeEvent.Changing ? PropertyChanging : PropertyChanged;
            try
            {
                if (commonEvent != null)
                {
                    commonEvent(this, args);
                }

                if (repository.ContainsKey(this))
                {
                    if (repository[this] != null && repository[this][args.PropertyName] != null)
                    {
                        var handler = repository[this][args.PropertyName];
                        handler(this, args);
                    }
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
        }
    }
}
