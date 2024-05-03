using System;
using System.Windows;
using System.Windows.Controls;

namespace AeonHacs.Wpf.Controls;

public class StackingPanel : Panel
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(StackingPanel));

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
        nameof(Offset), typeof(double), typeof(StackingPanel), new FrameworkPropertyMetadata(0.0,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

    public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
        nameof(Spacing), typeof(double), typeof(StackingPanel), new FrameworkPropertyMetadata(0.0,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

    public double Offset { get => (double)GetValue(OffsetProperty); set => SetValue(OffsetProperty, value); }

    public double Spacing { get => (double)GetValue(SpacingProperty); set => SetValue(SpacingProperty, value); }

    protected override Size MeasureOverride(Size availableSize)
    {
        Size desiredSize = new Size();
        double maxWidth = 0, maxHeight = 0;

        foreach (UIElement child in Children)
        {
            child.Measure(availableSize);
            desiredSize.Width += child.DesiredSize.Width;
            maxWidth = Math.Max(maxWidth, child.DesiredSize.Width);
            desiredSize.Height += child.DesiredSize.Height;
            maxHeight = Math.Max(maxHeight, child.DesiredSize.Height);
        }

        switch (Orientation)
        {
            case RelativeDirection.Left:
            case RelativeDirection.Right:
                desiredSize.Width += Math.Max(Spacing * (Children.Count - 1), 0) + Offset;
                desiredSize.Height = maxHeight;
                break;
            default:
            case RelativeDirection.Up:
            case RelativeDirection.Down:
                desiredSize.Width = maxWidth;
                desiredSize.Height += Math.Max(Spacing * (Children.Count - 1), 0) + Offset;
                break;
        }

        return desiredSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        switch (Orientation)
        {
            case RelativeDirection.Left:
                Point origin = new Point(Offset, 0);
                foreach (UIElement child in Children)
                {
                    origin.Y = (finalSize.Height - child.DesiredSize.Height) / 2;
                    child.Arrange(new Rect(origin, child.DesiredSize));
                    origin.X += child.DesiredSize.Width + Spacing;
                }
                break;
            default:
            case RelativeDirection.Up:
                origin = new Point(0, Offset);
                foreach (UIElement child in Children)
                {
                    origin.X = (finalSize.Width - child.DesiredSize.Width) / 2;
                    child.Arrange(new Rect(origin, child.DesiredSize));
                    origin.Y += child.DesiredSize.Height + Spacing;
                }
                break;
            case RelativeDirection.Down:
                origin = new Point(0, finalSize.Height - Offset);
                foreach (UIElement child in Children)
                {
                    origin.X = (finalSize.Width - child.DesiredSize.Width) / 2;
                    origin.Y -= child.DesiredSize.Height;
                    child.Arrange(new Rect(origin, child.DesiredSize));
                    origin.Y -= Spacing;
                }
                break;
            case RelativeDirection.Right:
                origin = new Point(finalSize.Width - Offset, 0);
                foreach (UIElement child in Children)
                {
                    origin.X -= child.DesiredSize.Width;
                    origin.Y = (finalSize.Height - child.DesiredSize.Height) / 2;
                    child.Arrange(new Rect(origin, child.DesiredSize));
                    origin.X -= Spacing;
                }
                break;
        }
        return finalSize;
    }
}
