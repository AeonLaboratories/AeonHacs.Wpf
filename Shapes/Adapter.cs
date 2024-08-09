using AeonHacs.Wpf.Controls;
using AeonHacs.Wpf.Converters;
using AeonHacs.Wpf.Views;
using System.ComponentModel;
using System.Windows;

namespace AeonHacs.Wpf.Shapes;

public class Adapter : View
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(Adapter));

    public static readonly DependencyProperty Connection1Property = DependencyProperty.Register(
        nameof(Connection1), typeof(double), typeof(Adapter), new PropertyMetadata(6.0));

    public static readonly DependencyProperty Connection2Property = DependencyProperty.Register(
        nameof(Connection2), typeof(double), typeof(Adapter), new PropertyMetadata(8.0));

    public RelativeDirection Orientation
    {
        get => (RelativeDirection)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    [TypeConverter(typeof(FittingSizeConverter))]
    public double Connection1
    {
        get => (double)GetValue(Connection1Property);
        set => SetValue(Connection1Property, value);
    }

    [TypeConverter(typeof(FittingSizeConverter))]
    public double Connection2
    {
        get => (double)GetValue(Connection2Property);
        set => SetValue(Connection2Property, value);
    }

    static Adapter()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Adapter), new FrameworkPropertyMetadata(typeof(Adapter)));
    }
}
