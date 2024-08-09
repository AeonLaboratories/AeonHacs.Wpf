using System.Windows;

namespace AeonHacs.Wpf.Views;

public class VTT : Section
{
    static VTT()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(VTT), new FrameworkPropertyMetadata(typeof(VTT)));
    }
}
