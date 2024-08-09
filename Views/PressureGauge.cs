using AeonHacs.Wpf.Converters;
using System.Windows;

namespace AeonHacs.Wpf.Views;

public class PressureGauge : Gauge
{
    public static readonly DependencyProperty PressureBinProperty = DependencyProperty.Register(
        nameof(PressureBin), typeof(PressureBin), typeof(PressureGauge), new FrameworkPropertyMetadata(PressureBin.Gauge));

    static PressureGauge()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PressureGauge), new FrameworkPropertyMetadata(typeof(PressureGauge)));
    }

    public PressureBin PressureBin { get => (PressureBin)GetValue(PressureBinProperty); set => SetValue(PressureBinProperty, value); }
}
