using AeonHacs.Wpf.Converters;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Shapes;
public abstract class TubingBase : Shape
{
    #region TubingSize
    public static readonly DependencyProperty TubingSizeProperty = DependencyProperty.RegisterAttached(
    "TubingSize", typeof(double), typeof(TubingBase), new FrameworkPropertyMetadata(4.0,
        FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
        FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

    [TypeConverter(typeof(TubingSizeConverter))]
    public static double GetTubingSize(FrameworkElement d) =>
        (double)d.GetValue(TubingSizeProperty);

    public static void SetTubingSize(FrameworkElement d, double size) =>
        d.SetValue(TubingSizeProperty, size);

    [TypeConverter(typeof(TubingSizeConverter))]
    public double TubingSize { get => (double)GetValue(TubingSizeProperty); set => SetValue(TubingSizeProperty, value); }
    #endregion TubingSize

    static TubingBase()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TubingBase), new FrameworkPropertyMetadata(typeof(TubingBase)));
    }

    protected override Geometry DefiningGeometry => DefineGeometry();

    protected abstract Geometry DefineGeometry();
}
