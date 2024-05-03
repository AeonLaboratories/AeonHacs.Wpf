using AeonHacs.Wpf.Controls;
using System.Windows;
using System.Windows.Controls;

namespace AeonHacs.Wpf;
public static class LayoutProperties
{
    #region Orientation
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.RegisterAttached(
        nameof(Orientation), typeof(RelativeDirection), typeof(LayoutProperties), new FrameworkPropertyMetadata(RelativeDirection.Up,
        FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
        FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

    public static RelativeDirection GetOrientation(FrameworkElement s) =>
        (RelativeDirection)s.GetValue(OrientationProperty);

    public static void SetOrientation(FrameworkElement s, RelativeDirection orientation) =>
        s.SetValue(OrientationProperty, orientation);
    #endregion Orientation
}