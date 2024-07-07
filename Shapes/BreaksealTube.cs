using AeonHacs.Wpf.Controls;
using AeonHacs.Wpf.Converters;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Shapes;

public class BreaksealTube : Shape
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(BreaksealTube));

    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
    nameof(Size), typeof(Size), typeof(BreaksealTube),
    new FrameworkPropertyMetadata(new Size(5, 31), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty StickoutProperty = DependencyProperty.Register(
    nameof(Stickout), typeof(double?), typeof(BreaksealTube), new FrameworkPropertyMetadata(null,
        FrameworkPropertyMetadataOptions.AffectsMeasure));

    static BreaksealTube()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(BreaksealTube), new FrameworkPropertyMetadata(typeof(BreaksealTube)));
    }

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    [TypeConverter(typeof(CultureTubeSizeConverter))]
    public Size Size { get => (Size)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }

    public double? Stickout { get => (double?)GetValue(StickoutProperty); set => SetValue(StickoutProperty, value); }

    protected override Geometry DefiningGeometry => DefineGeometry();

    protected virtual Geometry DefineGeometry()
    {
        var radius = Size.Width / 2;

        switch (Orientation)
        {
            case RelativeDirection.Left:
            case RelativeDirection.Right:
                return Geometry.Parse($"M{radius + 0.5},0.5 a{radius},{radius} 0 1 0 0,{Size.Width} h{Size.Height - Size.Width - 1} a{radius},{radius} 0 1 0 0,{-Size.Width} z");
            case RelativeDirection.Up:
            case RelativeDirection.Down:
                return Geometry.Parse($"M0.5,{radius + 0.5} a{radius},{radius} 0 1 1 {Size.Width},0 v{Size.Height - Size.Width - 1} a{radius},{radius} 0 1 1 {-Size.Width},0 z");
            default:
                return Geometry.Empty;
        }
    }

    protected override Size MeasureOverride(Size constraint)
    {
        var defaultSize = base.MeasureOverride(constraint);

        if (Stickout is not double stickout)
        {
            ClearValue(OpacityMaskProperty);
            return defaultSize;
        }

        bool reverse = Orientation == RelativeDirection.Left || Orientation == RelativeDirection.Up;

        switch (Orientation)
        {
            case RelativeDirection.Left:
            case RelativeDirection.Right:
                OpacityMask = new LinearGradientBrush([new GradientStop(Colors.Black, stickout / defaultSize.Width), new GradientStop(Color.FromArgb(55,0,0,0), stickout / defaultSize.Width)], new Point(reverse ? 1 : 0, 0.5), new Point(reverse ? 0 : 1, 0.5));
                return new Size(stickout, defaultSize.Height);
            case RelativeDirection.Up:
            case RelativeDirection.Down:
                OpacityMask = new LinearGradientBrush([new GradientStop(Colors.Black, stickout / defaultSize.Height), new GradientStop(Color.FromArgb(55, 0, 0, 0), stickout / defaultSize.Height)], new Point(0.5, reverse ? 1 : 0), new Point(0.5, reverse ? 0 : 1));
                return new Size(defaultSize.Width, stickout);
            default:
                return defaultSize;
        }
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        if (Stickout is double stickout)
        {
            var bounds = RenderedGeometry.Bounds;

            switch (Orientation)
            {
                case RelativeDirection.Left:
                    drawingContext.PushTransform(new TranslateTransform(stickout - bounds.Width - 1, 0));
                    break;
                case RelativeDirection.Up:
                    drawingContext.PushTransform(new TranslateTransform(0, stickout - bounds.Height - 1));
                    break;
                case RelativeDirection.Down:
                    break;
                case RelativeDirection.Right:
                    break;
                default:
                    break;
            }
        }

        base.OnRender(drawingContext);
    }
}
