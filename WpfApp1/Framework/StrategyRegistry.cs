using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework
{
    internal class StrategyRegistry : Dictionary<NotifiableObject, SortedSet<Tuple<string, PropertyChangedHandlerEx, PropertyChangedHandlerEx>>>
    {
        private ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private object _syncObj = new object();
        public void RegisterListener(NotifiableObject notifier, string propertyName, PropertyChangedHandlerEx handler)
        {
            Register(notifier, propertyName, null, handler);
        }

        public void UnregisterListener(NotifiableObject notifier, string propertyName, PropertyChangedHandlerEx handler)
        {
            Unregister(notifier, propertyName, null, handler);
        }

        public void RegisterHandler(NotifiableObject notifier, string propertyName, PropertyChangedHandlerEx changingHandler, PropertyChangedHandlerEx changedHandler)
        {
            Register(notifier, propertyName, changingHandler, changedHandler);
        }

        public void UnregisterHandler(NotifiableObject notifier, string propertyName, PropertyChangedHandlerEx changingHandler, PropertyChangedHandlerEx changedHandler)
        {
            Unregister(notifier, propertyName, changingHandler, changedHandler);
        }

        public void Invoke(NotifiableObject notifier, PropertyChangedEventArgsEx args, PropertyChangeEvent eventType)
        {
            if (notifier != null)
            {
                if (this.ContainsKey(notifier) && this[notifier] != null)
                {
                    foreach (var handlers in this[notifier].Where(item => item.Item1 == args.PropertyName))
                    {
                        //changing handler[1]
                        if (eventType == PropertyChangeEvent.Changing)
                        {
                            if (handlers.Item2 != null)
                            {
                                handlers.Item2(notifier, args);
                            }
                        }
                        else
                        {
                            if (handlers.Item3 != null)
                            {
                                handlers.Item3(notifier, args);
                            }
                        }
                    }
                }
            }
        }

        private void Register(NotifiableObject notifier, string propertyName, PropertyChangedHandlerEx changingHandler, PropertyChangedHandlerEx changedHandler)
        {
            lock (_syncObj)
            {
                if (notifier != null)
                {
                    var pair = new Tuple<string, PropertyChangedHandlerEx, PropertyChangedHandlerEx>(propertyName, changingHandler, changedHandler);
                    var regItem = ContainsKey(notifier) ? this[notifier] : new SortedSet<Tuple<string, PropertyChangedHandlerEx, PropertyChangedHandlerEx>>();
                    if (!ContainsKey(notifier))
                    {
                        this[notifier] = regItem;
                    }
                    else
                    {
                        var existed = regItem.Where(item => item.Item1 == propertyName && item.Item2 == changingHandler && item.Item3 == changedHandler);
                        if (existed != null && existed.Count() > 0)
                        {
                            Logger.WarnFormat("there is register handler on property {0} change.", propertyName);
                            return;
                        }
                    }

                    regItem.Add(pair);
                }
            }
        }
        private void Unregister(NotifiableObject notifier, string propertyName, PropertyChangedHandlerEx changingHandler, PropertyChangedHandlerEx changedHandler)
        {
            lock (_syncObj)
            {
                if (notifier != null)
                {
                    if (this.ContainsKey(notifier) && this[notifier] != null)
                    {
                        this[notifier].RemoveWhere(item => item.Item1 == propertyName && item.Item2 == changingHandler && item.Item3 == changedHandler);
                    }
                }
            }
        }
    }
}
