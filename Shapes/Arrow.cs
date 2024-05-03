using AeonHacs.Wpf.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Shapes;

public class Arrow : Shape
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(Arrow));

    static Arrow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Arrow), new FrameworkPropertyMetadata(typeof(Arrow)));
    }

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    protected override Geometry DefiningGeometry => DefineGeometry();

    protected virtual Geometry DefineGeometry()
    {
        switch (Orientation)
        {
            case RelativeDirection.Left:
                return Geometry.Parse("M0,3.5 L8.5,7 V6 L3.5,4 H17 V3 H3.5 L8.5,1 V0 Z");
            case RelativeDirection.Up:
                return Geometry.Parse("M3.5,0 L7,8.5 H6 L4,3.5 V17 H3 V3.5 L1,8.5 H0 Z");
            case RelativeDirection.Right:
                return Geometry.Parse("M17,3.5 L8.5,0 V1 L13.5,3 H0 V4 H13.5 L8.5,6 V7 Z");
            case RelativeDirection.Down:
                return Geometry.Parse("M3.5,17 L0,8.5 H1 L3,13.5 V0 H4 V13.5 L6,8.5 H7 Z");
            default:
                return Geometry.Empty;
        }
    }
}
