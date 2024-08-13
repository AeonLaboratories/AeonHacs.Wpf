using System.Windows;

namespace AeonHacs.Wpf.Views;

public class VMPressure : View
{
    public static readonly DependencyProperty OrderProperty = DependencyProperty.Register(
        nameof(Order), typeof(FlowDirection), typeof(VMPressure), new PropertyMetadata(FlowDirection.LeftToRight));

    public FlowDirection Order
    {
        get => (FlowDirection)GetValue(OrderProperty);
        set => SetValue(OrderProperty, value);
    }

    static VMPressure()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(VMPressure), new FrameworkPropertyMetadata(typeof(VMPressure)));
    }
}
