using AeonHacs.Wpf.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Shapes;

public class Vial : Shape
{
    static Vial()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Vial), new FrameworkPropertyMetadata(typeof(Vial)));
    }

    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(Vial));

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    protected override Geometry DefiningGeometry => DefineGeometry();

    protected virtual Geometry DefineGeometry()
    {
        var strokeHalf = StrokeThickness / 2;

        switch (Orientation)
        {
            case RelativeDirection.Left:
                return Geometry.Parse($"M{strokeHalf},{strokeHalf} v8 h34 v-8 z M6.5,{strokeHalf} v8");
            case RelativeDirection.Up:
                return Geometry.Parse($"M{strokeHalf},{strokeHalf} h8 v34 h-8 z M{strokeHalf},6.5 h8");
            case RelativeDirection.Down:
                return Geometry.Parse($"M{strokeHalf},{strokeHalf} h8 v34 h-8 z M{strokeHalf},28.5 h8");
            case RelativeDirection.Right:
                return Geometry.Parse($"M{strokeHalf},{strokeHalf} v8 h34 v-8 z M28.5,{strokeHalf} v8");
            default:
                break;
        }

        return Geometry.Empty;
    }
}
