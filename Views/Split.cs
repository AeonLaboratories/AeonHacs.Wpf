using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Views;

public class Split : View
{
    public static readonly DependencyProperty DataProperty = Path.DataProperty.AddOwner(typeof(Split));

    public Geometry Data { get => (Geometry)GetValue(DataProperty); set => SetValue(DataProperty, value); }

    static Split()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Split), new FrameworkPropertyMetadata(typeof(Split)));
    }
}
