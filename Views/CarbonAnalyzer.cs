using System.Windows;

namespace AeonHacs.Wpf.Views;

public class CarbonAnalyzer : View
{
    static CarbonAnalyzer()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CarbonAnalyzer), new FrameworkPropertyMetadata(typeof(CarbonAnalyzer)));
    }
}
