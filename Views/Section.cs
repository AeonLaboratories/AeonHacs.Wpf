using System.Windows;

namespace AeonHacs.Wpf.Views;

public class Section : View
{
    public static readonly DependencyProperty PressureSensorTypeProperty = DependencyProperty.Register(
        nameof(PressureSensorType), typeof(PressureSensorType), typeof(Section));

    static Section()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Section), new FrameworkPropertyMetadata(typeof(Section)));
    }

    public PressureSensorType PressureSensorType { get => (PressureSensorType)GetValue(PressureSensorTypeProperty); set => SetValue(PressureSensorTypeProperty, value); }
}
