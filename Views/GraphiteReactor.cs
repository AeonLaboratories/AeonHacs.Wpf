using AeonHacs.Wpf.Converters;
using System.ComponentModel;
using System.Windows;

namespace AeonHacs.Wpf.Views;

public class GraphiteReactor : Port
{
    public static readonly DependencyProperty ColdfingerProperty = DependencyProperty.Register(
        nameof(Coldfinger), typeof(INotifyPropertyChanged), typeof(GraphiteReactor));

    public static readonly DependencyProperty HeaterProperty = DependencyProperty.Register(
        nameof(Heater), typeof(INotifyPropertyChanged), typeof(GraphiteReactor));

    [TypeConverter(typeof(ViewModelConverter))]
    public INotifyPropertyChanged Coldfinger { get => (INotifyPropertyChanged)GetValue(ColdfingerProperty); set => SetValue(ColdfingerProperty, value); }

    [TypeConverter(typeof(ViewModelConverter))]
    public INotifyPropertyChanged Heater { get => (INotifyPropertyChanged)GetValue(HeaterProperty); set => SetValue(HeaterProperty, value); }

    static GraphiteReactor()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphiteReactor), new FrameworkPropertyMetadata(typeof(GraphiteReactor)));
    }
}
