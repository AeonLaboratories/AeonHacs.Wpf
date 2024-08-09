using System.Windows;

namespace AeonHacs.Wpf.Views;

public class Valve : View
{
    public static readonly DependencyProperty ValveStateProperty = DependencyProperty.Register(
        nameof(ValveState), typeof(ValveState), typeof(Valve), new PropertyMetadata(ValveState.Unknown));

    static Valve()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Valve), new FrameworkPropertyMetadata(typeof(Valve)));
    }

    public ValveState ValveState { get => (ValveState)GetValue(ValveStateProperty); set => SetValue(ValveStateProperty, value); }
}
