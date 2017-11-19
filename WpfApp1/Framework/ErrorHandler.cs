using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public abstract class ErrorHandler
    {
        public abstract void HandleError(NotifiableObject source, PropertyChangedEventArgs args);
    }
}
