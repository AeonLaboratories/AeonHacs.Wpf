using AeonHacs.Components;
using AeonHacs.Wpf.Controls;
using AeonHacs.Wpf.Converters;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace AeonHacs.Wpf.Views;

public class CombustionChamber : Control
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(CombustionChamber));

    public static readonly DependencyProperty QuartzFurnaceProperty = DependencyProperty.RegisterAttached(
        nameof(QuartzFurnace), typeof(INotifyPropertyChanged), typeof(CombustionChamber), new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty SampleFurnaceProperty = DependencyProperty.RegisterAttached(
        nameof(SampleFurnace), typeof(INotifyPropertyChanged), typeof(CombustionChamber), new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty IsFlowThroughProperty = DependencyProperty.RegisterAttached(
        nameof(IsFlowThrough), typeof(bool), typeof(CombustionChamber), new FrameworkPropertyMetadata(false,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty StateProperty = Port.StateProperty.AddOwner(typeof(CombustionChamber));

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    [TypeConverter(typeof(ViewModelConverter))]
    public INotifyPropertyChanged QuartzFurnace { get => (INotifyPropertyChanged)GetValue(QuartzFurnaceProperty); set => SetValue(QuartzFurnaceProperty, value); }

    [TypeConverter(typeof(ViewModelConverter))]
    public INotifyPropertyChanged SampleFurnace { get => (INotifyPropertyChanged)GetValue(SampleFurnaceProperty); set => SetValue(SampleFurnaceProperty, value); }

    public bool IsFlowThrough { get => (bool)GetValue(IsFlowThroughProperty); set => SetValue(IsFlowThroughProperty, value); }

    public LinePort.States State { get => (LinePort.States)GetValue(StateProperty); set => SetValue(StateProperty, value); }

    static CombustionChamber()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CombustionChamber), new FrameworkPropertyMetadata(typeof(CombustionChamber)));
    }
}
