using System.Windows;

namespace AeonHacs.Wpf.Views;

public class CT : Section
{
    static CT()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CT), new FrameworkPropertyMetadata(typeof(CT)));
    }
}
