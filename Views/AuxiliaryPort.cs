using AeonHacs.Components;
using AeonHacs.Wpf.Converters;
using System.ComponentModel;
using System.Windows;

namespace AeonHacs.Wpf.Views;

public class AuxiliaryPort : Port
{
    public static readonly DependencyProperty ColdfingerProperty = DependencyProperty.Register(
               nameof(Coldfinger), typeof(INotifyPropertyChanged), typeof(AuxiliaryPort));

    public string PortType { get => (string)GetValue(PortTypeProperty); set => SetValue(PortTypeProperty, value); }

    [TypeConverter(typeof(ViewModelConverter))]
    public INotifyPropertyChanged Coldfinger { get => (INotifyPropertyChanged)GetValue(ColdfingerProperty); set => SetValue(ColdfingerProperty, value); }

    public LinePort.States State { get => (LinePort.States)GetValue(StateProperty); set => SetValue(StateProperty, value); }

    static AuxiliaryPort()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(AuxiliaryPort), new FrameworkPropertyMetadata(typeof(AuxiliaryPort)));
        PortTypeProperty.OverrideMetadata(typeof(AuxiliaryPort), new FrameworkPropertyMetadata("needle"));
    }
}
