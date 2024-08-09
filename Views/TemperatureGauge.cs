using AeonHacs.Wpf.Converters;
using System.Windows;

namespace AeonHacs.Wpf.Views;

public class TemperatureGauge : Gauge
{
    public static readonly DependencyProperty TemperatureBinProperty = DependencyProperty.Register(
        nameof(TemperatureBin), typeof(TemperatureBin), typeof(TemperatureGauge), new FrameworkPropertyMetadata(TemperatureBin.Neutral));

    static TemperatureGauge()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TemperatureGauge), new FrameworkPropertyMetadata(typeof(TemperatureGauge)));
    }

    public TemperatureBin TemperatureBin { get => (TemperatureBin)GetValue(TemperatureBinProperty); set => SetValue(TemperatureBinProperty, value); }
}
