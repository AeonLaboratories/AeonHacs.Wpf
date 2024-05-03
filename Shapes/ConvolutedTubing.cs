using System.Windows;
using System.Windows.Media;

namespace AeonHacs.Wpf.Shapes;

public class ConvolutedTubing : Tubing
{
    protected override Size MeasureOverride(Size availableSize)
    {
        var bounds = Data.GetRenderBounds(new Pen(Stroke, TubingSize + 3));
        if (double.IsInfinity(bounds.Width) || double.IsInfinity(bounds.Height))
            return base.MeasureOverride(availableSize);

        return new Size(bounds.Width, bounds.Height);
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        // TODO should this be done?
        var bounds = Data.GetRenderBounds(new Pen(Stroke, TubingSize + 3));
        drawingContext.PushTransform(new TranslateTransform(-bounds.Left, -bounds.Top));

        var tube = Data.GetFlattenedPathGeometry(0.01, ToleranceType.Absolute);

        if (tube.Figures.Count == 3)
        {
            var tubeEnds = tube.Clone();
            tubeEnds.Figures.RemoveAt(1);
            tubeEnds.Freeze();

            // tube ends
            drawingContext.DrawGeometry(Fill, null, tubeEnds.GetWidenedPathGeometry(new Pen(Fill, TubingSize), 0.01, ToleranceType.Absolute));
            drawingContext.DrawGeometry(Stroke, null, Geometry.Combine(tubeEnds.GetWidenedPathGeometry(new Pen(Stroke, TubingSize + 1), 0.01, ToleranceType.Absolute), tubeEnds.GetWidenedPathGeometry(new Pen(Fill, TubingSize - 1), 0.01, ToleranceType.Absolute), GeometryCombineMode.Exclude, null));

            tube = tube.Clone();
            tube.Figures.RemoveAt(0);
            tube.Figures.RemoveAt(1);
        }
        tube.Freeze();

        drawingContext.DrawGeometry(null, new Pen(Fill, TubingSize + 1), tube);
        drawingContext.DrawGeometry(Stroke, null, Geometry.Combine(tube.GetWidenedPathGeometry(new Pen(Stroke, TubingSize + 3) { DashStyle = new DashStyle([1 / (TubingSize + 3)], 1), DashCap = PenLineCap.Flat }, 0.01, ToleranceType.Absolute), tube.GetWidenedPathGeometry(new Pen(Fill, TubingSize + 1), 0.01, ToleranceType.Absolute), GeometryCombineMode.Exclude, null));

        drawingContext.DrawGeometry(null, new Pen(new SolidColorBrush(Color.FromArgb(50, 0, 0, 0)), TubingSize - 1) { DashStyle = new DashStyle([1 / (TubingSize - 1)], 2), DashCap = PenLineCap.Flat }, tube);
        drawingContext.DrawGeometry(Stroke, null, Geometry.Combine(tube.GetWidenedPathGeometry(new Pen(Stroke, TubingSize + 1) { DashStyle = new DashStyle([1 / (TubingSize + 1)], 0), DashCap = PenLineCap.Flat }, 0.01, ToleranceType.Absolute), tube.GetWidenedPathGeometry(new Pen(Fill, TubingSize - 1), 0.01, ToleranceType.Absolute), GeometryCombineMode.Exclude, null));
    }
}
