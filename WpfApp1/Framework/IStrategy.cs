using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public interface IStrategy
    {
        void RegisterListener(NotifiableObject notifier, string propertyName, PropertyChangedHandlerEx handler);
        void RegisterAction(string actionName, PropertyChangedHandlerEx handler);
        PropertyChangedHandlerEx GetAction(string actionName);
    }
}
