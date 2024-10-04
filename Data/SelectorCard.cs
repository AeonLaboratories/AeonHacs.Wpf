using System.Collections;
using System.Windows;

namespace AeonHacs.Wpf.Data;

public class SelectorCard : PropertyCard
{
    public static readonly DependencyProperty StandardValuesProperty = DependencyProperty.Register(
        nameof(StandardValues),
        typeof(IEnumerable),
        typeof(SelectorCard)
    );

    static SelectorCard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectorCard), new FrameworkPropertyMetadata(typeof(SelectorCard)));
    }

    public IEnumerable StandardValues
    {
        get => (IEnumerable)GetValue(StandardValuesProperty);
        set => SetValue(StandardValuesProperty, value);
    }
}
