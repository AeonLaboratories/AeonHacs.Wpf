using HACS.Core;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HACS.WPF.Views
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
