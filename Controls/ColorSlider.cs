using System.Windows;

namespace AeonHacs.Wpf.Controls;

public class ColorSlider : ValueSlider
{
    static ColorSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorSlider), new FrameworkPropertyMetadata(typeof(ColorSlider)));
    }
}
