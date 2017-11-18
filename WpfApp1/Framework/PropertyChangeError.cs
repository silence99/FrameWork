using System;
using System.Collections.Generic;

namespace Framework
{
    public class PropertyChangeError
    {
        public string PropertyName { get; set; }
        public List<Exception> Exceptions { get; set; }
        public PropertyChangeEvent Event { get; set; }
    }

    public enum PropertyChangeEvent
    {
        Changing,
        Changed
    }
}
