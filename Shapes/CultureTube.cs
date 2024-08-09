using AeonHacs.Wpf.Controls;
using AeonHacs.Wpf.Converters;
using AeonHacs.Wpf.Views;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Shapes;

public class CultureTube : Shape
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(CultureTube));

    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size), typeof(Size), typeof(CultureTube),
        new FrameworkPropertyMetadata(new Size(5, 31), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    [TypeConverter(typeof(CultureTubeSizeConverter))]
    public Size Size { get => (Size)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }

    protected override Geometry DefiningGeometry => DefineGeometry();

    static CultureTube()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CultureTube), new FrameworkPropertyMetadata(typeof(CultureTube)));
        OrientationProperty.OverrideMetadata(typeof(CultureTube), new FrameworkPropertyMetadata(RelativeDirection.Down));
    }

    protected virtual Geometry DefineGeometry()
    {
        PathFigure figure;
        var strokeHalf = StrokeThickness / 2;
        var radius = Size.Width / 2;
        var length = Size.Height;

        switch (Orientation)
        {
            case RelativeDirection.Right:
                figure = new PathFigure()
                {
                    StartPoint = new Point(0, strokeHalf),
                    Segments =
                    {
                        new PolyLineSegment([new Point(length - radius - strokeHalf, strokeHalf)], true),
                        new ArcSegment(new Point(length - radius - strokeHalf, 2 * radius + strokeHalf), new Size(radius, radius), 0, false, SweepDirection.Clockwise, true),
                        new LineSegment(new Point(0, 2 * radius + strokeHalf), true)
                    }
                };
                break;
            default:
            case RelativeDirection.Down:
                figure = new PathFigure()
                {
                    StartPoint = new Point(strokeHalf, 0),
                    Segments =
                    {
                        new LineSegment(new Point(strokeHalf, length - radius - strokeHalf), true),
                        new ArcSegment(new Point(2 * radius + strokeHalf, length - radius - strokeHalf), new Size(radius, radius), 0, false, SweepDirection.Counterclockwise, true),
                        new LineSegment(new Point(2 * radius + strokeHalf, 0), true)
                    }
                };
                break;
            case RelativeDirection.Up:
                figure = new PathFigure()
                {
                    StartPoint = new Point(strokeHalf, length),
                    Segments =
                    {
                        new LineSegment(new Point(strokeHalf, radius + strokeHalf), true),
                        new ArcSegment(new Point(2 * radius + strokeHalf, radius + strokeHalf), new Size(radius, radius), 0, false, SweepDirection.Clockwise, true),
                        new LineSegment(new Point(2 * radius + strokeHalf, length), true)
                    }
                };
                break;
            case RelativeDirection.Left:
                figure = new PathFigure()
                {
                    StartPoint = new Point(length, strokeHalf),
                    Segments =
                    {
                        new LineSegment(new Point(radius + strokeHalf, strokeHalf), true),
                        new ArcSegment(new Point(radius + strokeHalf, 2 * radius + strokeHalf), new Size(radius, radius), 0, false, SweepDirection.Counterclockwise, true),
                        new LineSegment(new Point(length, 2 * radius + strokeHalf), true)
                    }
                };
                break;
        }

        return new PathGeometry() { Figures = { figure } };
    }
}
