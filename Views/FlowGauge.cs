using System.Windows;

namespace AeonHacs.Wpf.Views;

public class FlowGauge : Gauge
{
    static FlowGauge()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FlowGauge), new FrameworkPropertyMetadata(typeof(FlowGauge)));
    }
}
