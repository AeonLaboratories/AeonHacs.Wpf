using System.Windows;

namespace AeonHacs.Wpf.Views;

public class DualCT : Section
{
    static DualCT()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DualCT), new FrameworkPropertyMetadata(typeof(DualCT)));
    }
}
