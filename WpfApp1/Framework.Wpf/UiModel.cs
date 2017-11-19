using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using System.Windows.Controls;

namespace Framework.Wpf
{
    public class UiModel : Framework.UiModel
    {
        private Control Container { get; set; }
        public UiModel(Control container)
        {
            Container = container;
        }
        public override void Bind()
        {
            if (Container != null)
            {
            }
        }
    }
}
