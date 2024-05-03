using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AeonHacs.Wpf.Controls;

/// <summary>
/// Defines a content decorator that scales and stretches a single child without distortion to fill the available space.
/// </summary>
public class Viewport : Decorator
{
    #region StretchDirection
    public static readonly DependencyProperty StretchDirectionProperty = Viewbox.StretchDirectionProperty.AddOwner(
        typeof(Viewport), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

    public StretchDirection StretchDirection { get => (StretchDirection)GetValue(StretchDirectionProperty); set => SetValue(StretchDirectionProperty, value); }
    #endregion StretchDirection

    protected override Size MeasureOverride(Size availableSize)
    {
        var infinite = new Size(double.PositiveInfinity, double.PositiveInfinity);

        if (availableSize == infinite)
            availableSize = new Size(ActualWidth, ActualHeight);

        if (Child == null)
            return availableSize;

        Child.Measure(infinite);

        var width = availableSize.Width < Child.DesiredSize.Width ? availableSize.Width : Child.DesiredSize.Width;
        var height = availableSize.Height < Child.DesiredSize.Height ? availableSize.Height : Child.DesiredSize.Height;
        return new Size(width, height);
    }

    // TODO can this be optimized?
    protected override Size ArrangeOverride(Size finalSize)
    {
        if (Child == null)
            return finalSize;

        var scale = Math.Min(finalSize.Height / Child.DesiredSize.Height, finalSize.Width / Child.DesiredSize.Width);
        if ((StretchDirection == StretchDirection.UpOnly && scale < 1) || (StretchDirection == StretchDirection.DownOnly && scale > 1))
            scale = 1;
        if (scale == 0)
            return finalSize;

        double left = 0;
        double top = 0;
        double width = finalSize.Width / scale;
        double height = finalSize.Height / scale;

        if (Child is FrameworkElement fe)
        {
            if (!double.IsNaN(fe.Width))
            {
                left = Math.Max(0, (finalSize.Width - Child.DesiredSize.Width * scale) / 2);
                width = Child.DesiredSize.Width;
            }
            if (!double.IsNaN(fe.Height))
            {
                top = Math.Max(0, (finalSize.Height - Child.DesiredSize.Height * scale) / 2);
                height = Child.DesiredSize.Height;
            }
        }

        Child.RenderTransform = new ScaleTransform(scale, scale);
        Child.Arrange(new Rect(left, top, width, height));
        return finalSize;
    }
}