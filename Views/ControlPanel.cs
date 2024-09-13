using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AeonHacs.Wpf.Views
{
    public abstract class ControlPanel : ContentControl
    {
        public HacsBase Hacs { get; }

        public ControlPanel() { }

        public ControlPanel(HacsBase hacs)
        {
            Hacs = hacs;
            AeonHacs.Hacs.Initialize();
            AeonHacs.Hacs.Start();
        }
    }
}
