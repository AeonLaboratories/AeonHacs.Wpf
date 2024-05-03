using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Shapes;
public class Fitting : FittingBase
{
    #region Data
    public static readonly DependencyProperty DataProperty = Path.DataProperty.AddOwner(typeof(Fitting), new FrameworkPropertyMetadata(Geometry.Empty,
        FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
        FrameworkPropertyMetadataOptions.AffectsRender));

    public Geometry Data { get => (Geometry)GetValue(DataProperty); set => SetValue(DataProperty, value); }
    #endregion Data

    protected override Geometry DefineGeometry()
    {
        return new CombinedGeometry(GeometryCombineMode.Union, Data.GetWidenedPathGeometry(new Pen(Stroke, FittingSize), 0.01, ToleranceType.Absolute), null);
    }

    protected override Size MeasureOverride(Size constraint)
    {
        var bounds = DefiningGeometry.GetRenderBounds(new Pen(Stroke, StrokeThickness));
        if (double.IsInfinity(bounds.Width) || double.IsInfinity(bounds.Height))
            return base.MeasureOverride(constraint);
        else
            return new Size(bounds.Width, bounds.Height);
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        var bounds = DefiningGeometry.GetRenderBounds(new Pen(Stroke, StrokeThickness));
        drawingContext.PushTransform(new TranslateTransform(-bounds.Left, -bounds.Top));

        base.OnRender(drawingContext);
    }
}