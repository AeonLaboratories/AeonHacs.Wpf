using AeonHacs.Wpf.Controls;
using System.Windows;
using System.Windows.Media;

namespace AeonHacs.Wpf.Shapes;

public class Break : FittingBase
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(Break));

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    protected override Geometry DefineGeometry()
    {
        var fittingSize = FittingSize;

        switch (Orientation)
        {
            default:
                return Geometry.Empty;
            case RelativeDirection.Right:
                return Geometry.Parse($"M0,1.5 h{fittingSize} l{-fittingSize / 8},{fittingSize / 3} l{fittingSize / 4},{fittingSize / 3} l{-fittingSize / 8},{fittingSize / 3} h-{fittingSize} M{fittingSize},{1.5} l0.375,-1 M{fittingSize},{fittingSize + 1.5} l-0.375,1");
            case RelativeDirection.Down:
                return Geometry.Parse($"M1.5,0 v{fittingSize} l{fittingSize / 3},{fittingSize / 8} l{fittingSize / 3},{-fittingSize / 4} l{fittingSize / 3},{fittingSize / 8} v-{fittingSize} M1.5,{fittingSize} l-1,-0.375 M{fittingSize + 1.5},{fittingSize} l1,0.375");
            case RelativeDirection.Left:
                return Geometry.Parse($"M{fittingSize + 3},1.5 h-{fittingSize} l{-fittingSize / 8},{fittingSize / 3} l{fittingSize / 4},{fittingSize / 3} l{-fittingSize / 8},{fittingSize / 3} h{fittingSize} M3,1.5 l0.375,-1 M3,{fittingSize + 1.5} l-0.375,1");
            case RelativeDirection.Up:
                return Geometry.Parse($"M1.5,{fittingSize + 3} v-{fittingSize} l{fittingSize / 3},{fittingSize / 8} l{fittingSize / 3},{-fittingSize / 4} l{fittingSize / 3},{fittingSize / 8} v{fittingSize} M1.5,3 l-1,-0.375 M{fittingSize + 1.5},3 l1,0.375");
        }
    }

    protected override Size MeasureOverride(Size constraint)
    {
        return new Size(FittingSize + 3, FittingSize + 3);
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        drawingContext.DrawGeometry(Fill, new Pen(Stroke, StrokeThickness), RenderedGeometry);
    }
}
