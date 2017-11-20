using System.Collections.Generic;

namespace Framework
{
    public abstract class Model : NotifiableObject
    {
        protected Dictionary<string, bool> Disabled { get; set; }
        protected Dictionary<string, bool> Required { get; set; }
        protected Dictionary<string, object> Invisiabled { get; set; }
        public abstract void Bind();
        public abstract void SetVisiable(string refProperty, object visiable);
        public abstract void SetRequired(string refProperty, bool required);
        public abstract void SetEnbaled(string refProperty, bool enabled);
    }
}
