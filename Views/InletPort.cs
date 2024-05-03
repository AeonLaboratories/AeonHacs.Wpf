using AeonHacs.Components;
using AeonHacs.Wpf.Converters;
using System.ComponentModel;
using System.Windows;

namespace AeonHacs.Wpf.Views;
public class InletPort : Port
{
    public static readonly DependencyProperty QuartzFurnaceProperty = CombustionChamber.QuartzFurnaceProperty.AddOwner(typeof(InletPort));

    public static readonly DependencyProperty SampleFurnaceProperty = CombustionChamber.SampleFurnaceProperty.AddOwner(typeof(InletPort));

    public string PortType { get => (string)GetValue(PortTypeProperty); set => SetValue(PortTypeProperty, value); }

    [TypeConverter(typeof(ViewModelConverter))]
    public INotifyPropertyChanged QuartzFurnace { get => (INotifyPropertyChanged)GetValue(QuartzFurnaceProperty); set => SetValue(QuartzFurnaceProperty, value); }

    [TypeConverter(typeof(ViewModelConverter))]
    public INotifyPropertyChanged SampleFurnace { get => (INotifyPropertyChanged)GetValue(SampleFurnaceProperty); set => SetValue(SampleFurnaceProperty, value); }

    public LinePort.States State { get => (LinePort.States)GetValue(StateProperty); set => SetValue(StateProperty, value); }

    static InletPort()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(InletPort), new FrameworkPropertyMetadata(typeof(InletPort)));
        PortTypeProperty.OverrideMetadata(typeof(InletPort), new FrameworkPropertyMetadata("combustion"));
    }
}
