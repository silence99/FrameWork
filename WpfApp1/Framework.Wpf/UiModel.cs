using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Framework.Wpf
{
    public class UiModel : NotifiableObject
    {
        public override void ApplyToUI(string property)
        {
            TriggerNotifyToUIEvent(new PropertyChangedEventArgs(property));
        }
    }
}
