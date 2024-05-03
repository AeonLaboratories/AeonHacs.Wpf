using AeonHacs.Wpf.Converters;
using System.ComponentModel;
using System.Windows;

namespace AeonHacs.Wpf.Views;

public class GasSupply : Port
{
    public static readonly DependencyProperty GasNameProperty = DependencyProperty.Register(
        nameof(GasName), typeof(string), typeof(GasSupply));

    public static readonly DependencyProperty FlowValveProperty = DependencyProperty.Register(
        nameof(FlowValve), typeof(INotifyPropertyChanged), typeof(GasSupply));

    public string GasName { get => (string)GetValue(GasNameProperty); set => SetValue(GasNameProperty, value); }

    [TypeConverter(typeof(ViewModelConverter))]
    public INotifyPropertyChanged FlowValve { get => (INotifyPropertyChanged)GetValue(FlowValveProperty); set => SetValue(FlowValveProperty, value); }

    static GasSupply()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(GasSupply), new FrameworkPropertyMetadata(typeof(GasSupply)));
    }
}
