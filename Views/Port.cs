using AeonHacs.Components;
using AeonHacs.Wpf.Controls;
using AeonHacs.Wpf.Converters;
using System.ComponentModel;
using System.Windows;

namespace AeonHacs.Wpf.Views;

public abstract class Port : View
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(Port));

    public static readonly DependencyProperty PortTypeProperty = DependencyProperty.RegisterAttached(
        "PortType", typeof(string), typeof(Port));

    public static readonly DependencyProperty ValveProperty = DependencyProperty.Register(
        nameof(Valve), typeof(INotifyPropertyChanged), typeof(Port));

    public static readonly DependencyProperty StateProperty = DependencyProperty.RegisterAttached(
    "State", typeof(LinePort.States), typeof(Port), new FrameworkPropertyMetadata(LinePort.States.Complete,
        FrameworkPropertyMetadataOptions.Inherits));

    static Port()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Port), new FrameworkPropertyMetadata(typeof(Port)));
    }

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    [TypeConverter(typeof(ViewModelConverter))]
    public INotifyPropertyChanged Valve { get => (INotifyPropertyChanged)GetValue(ValveProperty); set => SetValue(ValveProperty, value); }
}
