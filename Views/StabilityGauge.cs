using System.Windows;

namespace AeonHacs.Wpf.Views;

public class StabilityGauge : Gauge
{
    static StabilityGauge()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(StabilityGauge), new FrameworkPropertyMetadata(typeof(StabilityGauge)));
    }
}
