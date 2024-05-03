using AeonHacs.Wpf.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Shapes;

public class CO2Cylinder : Shape
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(CO2Cylinder));

    static CO2Cylinder()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CO2Cylinder), new FrameworkPropertyMetadata(typeof(CO2Cylinder)));
    }

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    protected override Geometry DefiningGeometry => DefineGeometry();

    protected virtual Geometry DefineGeometry()
    {
        switch (Orientation)
        {
            case RelativeDirection.Left:
                return Geometry.Parse($"M0,4.5 h2.5 c2,0 6,-4 10,-4 h14 a6,6 0 1 1 0,12 h-14 c-4,0 -8,-4 -10,-4 h-2.5");
            case RelativeDirection.Up:
                return Geometry.Parse($"M4.5,0 v2.5 c0,2 -4,6 -4,10 v14 a6,6 0 1 0 12,0 v-14 c0,-4 -4,-8 -4,-10 v-2.5");
            case RelativeDirection.Right:
                return Geometry.Parse($"M33,4.5 h-2.5 c-2,0 -6,-4 -10,-4 h-14 a6,6 0 1 0 0,12 h14 c4,0 8,-4 10,-4 h2.5");
            case RelativeDirection.Down:
                return Geometry.Parse($"M4.5,33 v-2.5 c0,-2 -4,-6 -4,-10 v-14 a6,6 0 1 1 12,0 v14 c0,4 -4,8 -4,10 v2.5");
            default:
                return Geometry.Empty;
        }
    }
}
