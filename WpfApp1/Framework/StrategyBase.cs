using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class StrategyBase : IStrategy
    {
        private ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private StrategyBase _parent = null;
        private StrategyRegistry _registry = null;
        private Dictionary<string, PropertyChangedHandlerEx> _actions = null;

        public StrategyBase() :
            this(null)
        {
        }

        public StrategyBase(StrategyBase parent)
        {
            if (parent == null)
            {
                _actions = new Dictionary<string, PropertyChangedHandlerEx>();
            }
            else
            {
                _parent = parent;
            }
        }

        public PropertyChangedHandlerEx GetAction(string actionName)
        {
            if (_parent == null && _actions == null)
            {
                _logger.ErrorFormat("{0} not init correctly, both parent and _actions properties are null");
                throw new Exception("Strategy not init correctly");
            }
            else
            {
                if (_parent != null)
                {
                    return _parent.GetAction(actionName);
                }
                else
                {
                    if (_actions.ContainsKey(actionName))
                    {
                        return _actions[actionName];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public void RegisterAction(string actionName, PropertyChangedHandlerEx handler)
        {
            _logger.DebugFormat("Strategy {0} register an action: {1}", this.GetType().FullName, actionName);
            if (_parent == null)
            {
                _actions[actionName] = handler;
            }
            else
            {
                _parent.RegisterAction(actionName, handler);
            }
        }

        public void RegisterListener(NotifiableObject notifier, string propertyName, PropertyChangedHandlerEx handler)
        {
            if(notifier != null)
            {

            }
        }
    }
}
