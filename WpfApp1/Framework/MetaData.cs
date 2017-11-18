using System.Collections.Generic;

namespace Framework
{
    public class MetaData
    {
        public MetaData()
        {
            Visible = true;
            Required = true;
            Enabled = true;
        }
        public bool Visible { get; set; }
        public bool Required { get; set; }
        public bool Enabled { get; set; }
        /// <summary>
        /// for dropdown list
        /// </summary>
        public List<object> DropDownItems { get; set; }

        public bool IsClean
        {
            get
            {
                return Visible && Required && Enabled;
            }
        }
    }
}
