using System.Windows;

namespace AeonHacs.Wpf.Views;

public class VMPressure : View
{
    static VMPressure()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(VMPressure), new FrameworkPropertyMetadata(typeof(VMPressure)));
    }
}
