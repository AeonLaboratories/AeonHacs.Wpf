using System.Windows;

namespace AeonHacs.Wpf.Data;
public class BooleanCard : PropertyCard
{
    static BooleanCard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(BooleanCard), new FrameworkPropertyMetadata(typeof(BooleanCard)));
    }
}
