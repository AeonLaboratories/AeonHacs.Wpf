using AeonHacs.Wpf.Controls;
using System.Windows;
using System.Windows.Media;

namespace AeonHacs.Wpf.Shapes;

public class Coupling : FittingBase
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(Coupling));

    public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(
               nameof(Length), typeof(double), typeof(Coupling), new FrameworkPropertyMetadata(2.0,
                   FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure |
                   FrameworkPropertyMetadataOptions.AffectsRender));

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    public double Length { get => (double)GetValue(LengthProperty); set => SetValue(LengthProperty, value); }

    protected override Geometry DefineGeometry()
    {
        switch (Orientation)
        {
            case RelativeDirection.Left:
            case RelativeDirection.Right:
                return new RectangleGeometry(new Rect(0.5, 0.5, Length - 1, FittingSize));
            case RelativeDirection.Up:
            case RelativeDirection.Down:
                return new RectangleGeometry(new Rect(0.5, 0.5, FittingSize, Length - 1));
            default:
                return Geometry.Empty;
        }
    }
}
