using AeonHacs.Wpf.Controls;
using System.Windows;
using System.Windows.Media;

namespace AeonHacs.Wpf.Shapes;

public class Stub : FittingBase
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(Stub));

    public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(
           nameof(Length), typeof(double), typeof(Stub), new FrameworkPropertyMetadata(4.0,
               FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure |
               FrameworkPropertyMetadataOptions.AffectsRender));

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    public double Length { get => (double)GetValue(LengthProperty); set => SetValue(LengthProperty, value); }

    protected override Geometry DefineGeometry()
    {
        switch (Orientation)
        {
            case RelativeDirection.Right:
                return Geometry.Parse($"M0,0.5 h{Length - 0.5} v{FittingSize} H0");
            case RelativeDirection.Down:
                return Geometry.Parse($"M0.5,0 v{Length - 0.5} h{FittingSize} V0");
            case RelativeDirection.Up:
                return Geometry.Parse($"M0.5,{Length} V0.5 h{FittingSize} V{Length}");
            case RelativeDirection.Left:
                return Geometry.Parse($"M{Length},0.5 H0.5 v{FittingSize} H{Length}");
            default:
                return Geometry.Empty;
        }
    }
}
