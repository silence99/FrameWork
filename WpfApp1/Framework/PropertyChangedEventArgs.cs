using System;
using System.ComponentModel;

namespace Framework
{
    public class PropertyChangedEventArgsEx : PropertyChangedEventArgs
    {
        public PropertyChangedEventArgsEx(string propertyName) : base(propertyName)
        {
        }

        public object Source { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public bool UserInited { get; set; }
        public bool UIChanged { get; set; }
    }
}
