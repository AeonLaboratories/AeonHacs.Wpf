using System.Windows;

namespace AeonHacs.Wpf.Views;

public class VMPressure : View
{
    public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
        nameof(Direction), typeof(FlowDirection), typeof(VMPressure), new PropertyMetadata(FlowDirection.LeftToRight));

    public FlowDirection Direction
    {
        get => (FlowDirection)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    static VMPressure()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(VMPressure), new FrameworkPropertyMetadata(typeof(VMPressure)));
    }
}
