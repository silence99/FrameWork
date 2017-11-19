using System.Collections.Generic;

namespace Framework
{
    public abstract class UiModel : NotifiableObject
    {
        protected virtual Dictionary<string, MetaData> MetaData { get; set; }
        public abstract void Bind();
        public void SetVisiable(string refProperty, bool visiable)
        {
            SetMetaData(MetaDataType.Visiable, refProperty, visiable);
        }
        public void SetRequired(string refProperty, bool required)
        {
            SetMetaData(MetaDataType.Required, refProperty, required);
        }
        public void SetEnbaled(string refProperty, bool enabled)
        {
            SetMetaData(MetaDataType.Enable, refProperty, enabled);
        }

        private void SetMetaData(MetaDataType typ, string refProperty, bool val)
        {
            if (!val)
            {
                MetaData data = null;
                if (!MetaData.ContainsKey(refProperty))
                {
                    data = new MetaData();
                    MetaData[refProperty] = data;
                }
                else
                {
                    data = MetaData[refProperty];
                }

                switch (typ)
                {
                    case MetaDataType.Enable:
                        data.Enabled = val;
                        break;
                    case MetaDataType.Required:
                        data.Required = val;
                        break;
                    case MetaDataType.Visiable:
                        data.Visible = val;
                        break;
                    default:
                        MetaData.Remove(refProperty);
                        break;
                }
            }
            else
            {
                if (MetaData.ContainsKey(refProperty))
                {
                    if (MetaData[refProperty] == null || MetaData[refProperty].IsClean)
                    {
                        MetaData.Remove(refProperty);
                    }
                }
            }
        }

        private enum MetaDataType
        {
            Visiable,
            Required,
            Enable
        }
    }
}
