using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var mydata = btnTest.GetBindingExpression(ContentProperty).DataItem as MyData;
            var before = DateTime.Now;
            //mydata.Name = "KKKKKK";
            mydata.NoUse = "hhh";
            var span = DateTime.Now - before;
            mydata.Span = span.Milliseconds;
        }
    }

    public class MyData : INotifyPropertyChanged
    {
        private string _name;
        private double _span;
        private string _noUse;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public double Span
        {
            get
            {
                return _span;
            }
            set
            {
                _span = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Span"));
            }
        }
        public string NoUse
        {
            get
            {
                return _noUse;
            }
            set
            {
                _noUse = value;
                PropertyChanged(this, new PropertyChangedEventArgs("NoUse"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
