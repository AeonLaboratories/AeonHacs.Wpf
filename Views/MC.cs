using System.Windows;

namespace AeonHacs.Wpf.Views;

public class MC : Section
{
    static MC()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MC), new FrameworkPropertyMetadata(typeof(MC)));
    }
}
