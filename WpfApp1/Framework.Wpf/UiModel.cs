using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Framework.Wpf
{
    public class UiModel : Model
    {
        private Control Container { get; set; }
        public UiModel(Control container)
        {
            Container = container;
            Disabled = new Dictionary<string, bool>();
            Required = new Dictionary<string, bool>();
            Invisiabled = new Dictionary<string, object>();
        }
        public override void Bind()
        {
            if (Container != null)
            {
                ApplyFromUI(Container);
            }
        }

        public override void SetVisiable(string refProperty, object visiable)
        {
            throw new NotImplementedException();
        }

        public override void SetRequired(string refProperty, bool required)
        {
            throw new NotImplementedException();
        }

        public override void SetEnbaled(string refProperty, bool enabled)
        {
            throw new NotImplementedException();
        }

        private void ApplyFromUI(Control container)
        {

        }

        private void ApplyToUI(Control container)
        {

        }

        protected override void PostInitalize()
        {
            base.PostInitalize();
            ApplyToUI(Container);
        }
    }
}
