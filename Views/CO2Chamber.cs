using System.Windows;

namespace AeonHacs.Wpf.Views;
public class CO2Chamber : Port
{
    static CO2Chamber()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CO2Chamber), new FrameworkPropertyMetadata(typeof(CO2Chamber)));
    }
}
