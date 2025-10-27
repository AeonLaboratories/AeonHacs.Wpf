using System.Windows;

namespace AeonHacs.Wpf.Views;

public class Ambient : View
{
    static Ambient()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Ambient), new FrameworkPropertyMetadata(typeof(Ambient)));
    }
}
