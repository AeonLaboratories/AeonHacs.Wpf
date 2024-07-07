using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Views;

public class Chamber : View
{
    #region Data
    public static readonly DependencyProperty DataProperty = Path.DataProperty.AddOwner(typeof(Chamber));

    public Geometry Data { get => GetValue(DataProperty) as Geometry; set => SetValue(DataProperty, value); }
    #endregion Data

    #region Fill
    public static readonly DependencyProperty FillProperty = Shape.FillProperty.AddOwner(typeof(Chamber));

    public Brush Fill { get => GetValue(FillProperty) as Brush; set => SetValue(FillProperty, value); }
    #endregion Fill

    #region Stroke
    public static readonly DependencyProperty StrokeProperty = Shape.StrokeProperty.AddOwner(typeof(Chamber));
    public Brush Stroke { get => GetValue(StrokeProperty) as Brush; set => SetValue(StrokeProperty, value); }

    public static readonly DependencyProperty StrokeThicknessProperty = Shape.StrokeThicknessProperty.AddOwner(typeof(Chamber));
    public double StrokeThickness { get => (double)GetValue(StrokeThicknessProperty); set => SetValue(StrokeThicknessProperty, value); }
    #endregion Stroke

    #region Thickness
    public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(
        nameof(Thickness), typeof(double), typeof(Chamber), new PropertyMetadata(8.0));

    public double Thickness { get => (double)GetValue(ThicknessProperty); set => SetValue(ThicknessProperty, value); }
        #endregion Thickness

    #region HitDetectionThickness
    public static readonly DependencyProperty HitDetectionThicknessProperty = DependencyProperty.Register(
        nameof(HitDetectionThickness), typeof(double), typeof(Chamber), new PropertyMetadata(8.0));

    public double HitDetectionThickness { get => (double)GetValue(HitDetectionThicknessProperty); set => SetValue(HitDetectionThicknessProperty, value); }
    #endregion HitDetectionThickness

        static Chamber()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Chamber), new FrameworkPropertyMetadata(typeof(Chamber)));
    }
}
