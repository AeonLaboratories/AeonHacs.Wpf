using AeonHacs.Wpf.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AeonHacs.Wpf.Views;

public enum PressureSensorType
{
    Port,
    Edwards_AimX,
    Inficon_CDG020D,
    Inficon_CDG025D,
    MKS_722_HP,
    MKS_722_LP,
    MKS_901P,
    MKS_DMB
}

[TemplatePart(Name = "PART_canvas")]
public class PressureSensor : View
{
    public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
        nameof(Type), typeof(PressureSensorType), typeof(PressureSensor), new PropertyMetadata(PressureSensorType.Port));

    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(PressureSensor));

    static PressureSensor()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PressureSensor), new FrameworkPropertyMetadata(typeof(PressureSensor)));
    }

    public PressureSensorType Type
    {
        get => (PressureSensorType)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public RelativeDirection Orientation
    {
        get => (RelativeDirection)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    private Canvas PART_canvas;

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

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        PART_canvas = GetTemplateChild("PART_canvas") as Canvas;
    }
}
