using System.Windows;
using System.Windows.Controls;

namespace AeonHacs.Wpf.Data;

public class PropertyCard : Control
{
    public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
        nameof(DisplayName),
        typeof(string),
        typeof(PropertyCard)
    );

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(object),
        typeof(PropertyCard)
    );

    static PropertyCard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyCard), new FrameworkPropertyMetadata(typeof(PropertyCard)));
    }

    public string DisplayName
    {
        get => (string)GetValue(DisplayNameProperty);
        set => SetValue(DisplayNameProperty, value);
    }

    public object Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}
