using System.Windows;
using System.Windows.Controls;

namespace AeonHacs.Wpf.Views;

public class Restriction : Control
{
    #region Orientation
    public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(
        typeof(Restriction));

    public Orientation Orientation { get => (Orientation)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }
    #endregion Orientation

    static Restriction()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Restriction), new FrameworkPropertyMetadata(typeof(Restriction)));
    }
}
