using AeonHacs.Wpf.Converters;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Shapes;

public abstract class FittingBase : Shape
{
    #region FittingSize
    public static readonly DependencyProperty FittingSizeProperty = DependencyProperty.RegisterAttached(
            nameof(FittingSize), typeof(double), typeof(FittingBase), new FrameworkPropertyMetadata(6.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

    [TypeConverter(typeof(FittingSizeConverter))]
    public static double GetFittingSize(FrameworkElement d) =>
        (double)d.GetValue(FittingSizeProperty);

    public static void SetFittingSize(FrameworkElement d, double size) =>
        d.SetValue(FittingSizeProperty, size);

    [TypeConverter(typeof(FittingSizeConverter))]
    public double FittingSize { get => (double)GetValue(FittingSizeProperty); set => SetValue(FittingSizeProperty, value); }
    #endregion FittingSize

    static FittingBase()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FittingBase), new FrameworkPropertyMetadata(typeof(FittingBase)));
    }

    protected override Geometry DefiningGeometry => DefineGeometry();

    protected abstract Geometry DefineGeometry();
}
