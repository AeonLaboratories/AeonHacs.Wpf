using AeonHacs.Wpf.Controls;
using System.Windows;
using System.Windows.Media;

namespace AeonHacs.Wpf.Shapes;

public class Connection : FittingBase
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(Connection));

    public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(
        nameof(Length), typeof(double), typeof(Connection), new PropertyMetadata(7.0));

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
                            new LineSegment(new Point(Length, 0.5 + FittingSize), false),
                            new LineSegment(new Point(0, 0.5 + FittingSize), true)
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
                            new LineSegment(new Point(0.5 + FittingSize, Length), false),
                            new LineSegment(new Point(0.5 + FittingSize, 0), true)
                        ],
                        false)
                    ]);
            default:
                return Geometry.Empty;
        }
    }
}
