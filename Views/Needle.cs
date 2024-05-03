using AeonHacs.Components;
using AeonHacs.Wpf.Controls;
using System.Windows;
using System.Windows.Controls;

namespace AeonHacs.Wpf.Views;

public class Needle : Control
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(Needle));

    public static readonly DependencyProperty StateProperty = Port.StateProperty.AddOwner(typeof(Needle));

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    public LinePort.States State { get => (LinePort.States)GetValue(StateProperty); set => SetValue(StateProperty, value); }

    static Needle()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Needle), new FrameworkPropertyMetadata(typeof(Needle)));
    }
}
