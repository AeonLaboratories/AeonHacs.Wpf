using AeonHacs.Wpf.Controls;
using AeonHacs.Wpf.Converters;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Shapes;

public class Needle : Shape
{
    static Needle()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Needle), new FrameworkPropertyMetadata(typeof(Needle)));
    }

    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(Needle));

    public static readonly DependencyProperty SizeProperty = Tubing.TubingSizeProperty.AddOwner(typeof(Needle));

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    [TypeConverter(typeof(TubingSizeConverter))]
    public double Size { get => (double)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }

    protected override Geometry DefiningGeometry => DefineGeometry();

    protected virtual Geometry DefineGeometry()
    {
        var strokeHalf = StrokeThickness / 2;
        var halfSize = (Size + StrokeThickness) / 2;
        var delta = halfSize - 1 - strokeHalf;

        switch (Orientation)
        {
            case RelativeDirection.Right:
                return Geometry.Parse($"M0,{strokeHalf} h2.5 l2,{delta} h3 v2 h-3 l-2,{delta} h-2.5 M7.5,{halfSize} h12.5");
            case RelativeDirection.Down:
                return Geometry.Parse($"M{strokeHalf},0 v2.5 l{delta},2 v3 h2 v-3 l{delta},-2 v-2.5 M{halfSize},7.5 v12.5");
            case RelativeDirection.Up:
                return Geometry.Parse($"M{strokeHalf},20 v-2.5 l{delta},-2 v-3 h2 v3 l{delta},2 v2.5 M{halfSize},12.5 v-12.5");
            case RelativeDirection.Left:
                return Geometry.Parse($"M20,{strokeHalf} h-2.5 l-2,{delta} h-3 v2 h3 l2,{delta} h2.5 M12.5,{halfSize} h-12.5");
            default:
                return Geometry.Empty;
        }
    }
}
