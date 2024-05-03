using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Shapes;
public class Tubing : TubingBase
{
    #region Data
    public static readonly DependencyProperty DataProperty = Path.DataProperty.AddOwner(typeof(Tubing), new FrameworkPropertyMetadata(Geometry.Empty,
        FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
        FrameworkPropertyMetadataOptions.AffectsRender));

    public Geometry Data { get => (Geometry)GetValue(DataProperty); set => SetValue(DataProperty, value); }
    #endregion Data

    protected override Geometry DefineGeometry()
    {
        return Data;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var bounds = Data.GetRenderBounds(new Pen(Stroke, TubingSize + 1));
        if (double.IsInfinity(bounds.Width) || double.IsInfinity(bounds.Height))
            return base.MeasureOverride(availableSize);

        return new Size(bounds.Width, bounds.Height);
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        var bounds = Data.GetRenderBounds(new Pen(Stroke, TubingSize + 1));
        drawingContext.PushTransform(new TranslateTransform(-bounds.Left, -bounds.Top));

        drawingContext.DrawGeometry(Fill, null, Data.GetWidenedPathGeometry(new Pen(Fill, TubingSize), 0.01, ToleranceType.Absolute));
        drawingContext.DrawGeometry(Stroke, null, Geometry.Combine(Data.GetWidenedPathGeometry(new Pen(Stroke, TubingSize + 1), 0.01, ToleranceType.Absolute), Data.GetWidenedPathGeometry(new Pen(Fill, TubingSize - 1), 0.01, ToleranceType.Absolute), GeometryCombineMode.Exclude, null));
    }
}
