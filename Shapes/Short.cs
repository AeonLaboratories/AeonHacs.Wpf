using AeonHacs.Wpf.Controls;
using System.Windows;
using System.Windows.Media;

namespace AeonHacs.Wpf.Shapes;
public class Short : TubingBase
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(Short));

    public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(
               nameof(Length), typeof(double), typeof(Short), new FrameworkPropertyMetadata(2.0,
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
                return new PathGeometry(
                    [
                    new PathFigure(
                        new Point(0, 0.5),
                        [
                            new LineSegment(new Point(Length, 0.5), true),
                            new LineSegment(new Point(Length, 0.5 + TubingSize), false),
                            new LineSegment(new Point(0, 0.5 + TubingSize), true)
                        ],
                        false)
                    ]);
            case RelativeDirection.Up:
            case RelativeDirection.Down:
                return new PathGeometry(
                    [
                    new PathFigure(
                        new Point(0.5, 0),
                        [
                            new LineSegment(new Point(0.5, Length), true),
                            new LineSegment(new Point(0.5 + TubingSize, Length), false),
                            new LineSegment(new Point(0.5 + TubingSize, 0), true)
                        ],
                        false)
                    ]);
            default:
                return Geometry.Empty;
        }
    }
}
