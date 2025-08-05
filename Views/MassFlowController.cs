using AeonHacs.Wpf.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AeonHacs.Wpf.Views;

[TemplatePart(Name = nameof(PART_canvas))]
public class MassFlowController : View
{
    public static readonly DependencyProperty OrientationProperty =
        LayoutProperties.OrientationProperty.AddOwner(typeof(MassFlowController));

    static MassFlowController()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MassFlowController), new FrameworkPropertyMetadata(typeof(MassFlowController)));
    }

    public RelativeDirection Orientation
    {
        get => (RelativeDirection)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    private Canvas PART_canvas;

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        PART_canvas = GetTemplateChild(nameof(PART_canvas)) as Canvas;
    }

    protected override Size MeasureOverride(Size constraint)
    {
        PART_canvas.LayoutTransform = Orientation switch
        {
            RelativeDirection.Left => new RotateTransform(-90, PART_canvas.Width / 2, PART_canvas.Width / 2),
            RelativeDirection.Right => new RotateTransform(90, PART_canvas.Height / 2, PART_canvas.Height / 2),
            RelativeDirection.Down => new RotateTransform(180, PART_canvas.Width / 2, PART_canvas.Height / 2),
            _ => default,
        };
        PART_canvas.Measure(constraint);

        return base.MeasureOverride(constraint);
    }
}
