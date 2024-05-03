using AeonHacs.Components;
using AeonHacs.Wpf.Controls;
using System.Windows;
using System.Windows.Controls;

namespace AeonHacs.Wpf.Views;

public class Breakseal : Control
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(Breakseal));

    public static readonly DependencyProperty StateProperty = Port.StateProperty.AddOwner(typeof(Breakseal));

    static Breakseal()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Breakseal), new FrameworkPropertyMetadata(typeof(Breakseal)));
    }

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    public LinePort.States State { get => (LinePort.States)GetValue(StateProperty); set => SetValue(StateProperty, value); }
}
