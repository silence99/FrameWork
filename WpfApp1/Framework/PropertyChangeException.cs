using System;
using System.Collections.Generic;

namespace Framework
{
    public class PropertyChangeException : Exception
    {
        public List<PropertyChangeError> Errors { get; set; }

        public PropertyChangeException()
        {
            Errors = new List<PropertyChangeError>();
        }
    }
}
