using System;

namespace Framework
{
    public class PropertyChangedEventArgs : EventArgs
    {
        public object Source { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public string PropertyName { get; set; }
        public bool UserInited { get; set; }
        public bool UIChanged { get; set; }
    }
}
