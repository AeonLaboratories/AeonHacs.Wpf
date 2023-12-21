using AeonHacs;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace AeonHacs.Wpf.Views
{
    public partial class Preferences : Window
    {
        public Preferences()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            BrushSetting.ColorPicker?.Close();
            base.OnClosing(e);
        }
    }
}
