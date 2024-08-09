using System.Windows;

namespace AeonHacs.Wpf.Shapes;

public class Union : Adapter
{
    static Union()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Union), new FrameworkPropertyMetadata(typeof(Union)));
    }
}