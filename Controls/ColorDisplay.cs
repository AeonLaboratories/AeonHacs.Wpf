using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AeonHacs.Wpf.Controls;

public class ColorDisplay : Control
{
    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
        nameof(Color), typeof(Color), typeof(ColorDisplay), new PropertyMetadata((Color)default));

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    static ColorDisplay()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorDisplay), new FrameworkPropertyMetadata(typeof(ColorDisplay)));
    }
}
