using AeonHacs.Wpf.Views;
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AeonHacs.Wpf.Behaviors;


public class ScalableWindowBehavior : Behavior<MainWindow>
{
    public static readonly DependencyProperty ScaleMultiplierProperty = DependencyProperty.RegisterAttached(
        "ScaleMultiplier", typeof(double), typeof(MainWindow), new UIPropertyMetadata(1.0, null, CoerceScaleMultiplier));

    public static double GetScaleMultiplier(Window window) =>
        (double)(window?.GetValue(ScaleMultiplierProperty) ?? 1.0);

    public static void SetScaleMultiplier(Window window, double scaleMultiplier) =>
        window?.SetValue(ScaleMultiplierProperty, scaleMultiplier);

    public static object CoerceScaleMultiplier(DependencyObject d, object baseValue)
    {
        if (baseValue is double value && value > 1.0 && value != double.MaxValue)
            return value;
        return 1.0;
    }

    protected double ScaleMultiplier => (double)AssociatedObject.GetValue(ScaleMultiplierProperty);

    protected ScaleTransform ScaleTransform { get; set; }

    protected UIElement Content => AssociatedObject.MainContent as UIElement;

    protected virtual void CalculateScaleMultiplier()
    {
        var clientSize = Content.RenderSize;
        var desiredSize = new Size(Math.Ceiling(Content.DesiredSize.Width), Math.Ceiling(Content.DesiredSize.Height));
        double xMultiplier = clientSize.Width / desiredSize.Width;
        double yMultiplier = clientSize.Height / desiredSize.Height;
        SetScaleMultiplier(AssociatedObject, Math.Min(xMultiplier, yMultiplier));
    }

    protected override void OnAttached()
    {
        if (AssociatedObject is null)
            return;

        base.OnAttached();
        ScaleTransform = new ScaleTransform(1.0, 1.0, 0, 0);
        Binding scaleBinding = new Binding() { Path = new PropertyPath(ScaleMultiplierProperty), Source = AssociatedObject };
        BindingOperations.SetBinding(ScaleTransform, ScaleTransform.ScaleXProperty, scaleBinding);
        BindingOperations.SetBinding(ScaleTransform, ScaleTransform.ScaleYProperty, scaleBinding);
        AssociatedObject.MainContent.RenderTransform = ScaleTransform;
        AssociatedObject.SizeChanged += AssociatedObject_SizeChanged;
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is null)
            return;

        base.OnDetaching();
        AssociatedObject.SizeChanged -= AssociatedObject_SizeChanged;
    }

    private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        CalculateScaleMultiplier();
    }
}
