using System.Windows;

namespace AeonHacs.Wpf.Views;

public class Switch : View
{
    public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register(
        nameof(IsOn), typeof(bool), typeof(Switch), new FrameworkPropertyMetadata(false));

    static Switch()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Switch), new FrameworkPropertyMetadata(typeof(Switch)));
    }

    public bool IsOn { get => (bool)GetValue(IsOnProperty); set => SetValue(IsOnProperty, value); }
}
