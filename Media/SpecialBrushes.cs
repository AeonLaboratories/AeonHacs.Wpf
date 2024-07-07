using System.Windows;
using System.Windows.Media;

namespace AeonHacs.Wpf.Media;

public static class SpecialBrushes
{
    public static Brush TransparentPattern { get; } = createTransparentPatternBrush();
    private static Brush createTransparentPatternBrush()
    {
        var rg1 = new RectangleGeometry(new Rect(0, 0, 8, 8));

        var rg2 = new RectangleGeometry(new Rect(0, 4, 4, 4));
        var rg3 = new RectangleGeometry(new Rect(4, 0, 4, 4));

        var gg = new GeometryGroup();
        gg.Children.Add(rg2);
        gg.Children.Add(rg3);

        var gd1 = new GeometryDrawing(Brushes.LightGray, null, rg1);
        var gd2 = new GeometryDrawing(Brushes.WhiteSmoke, null, gg);

        var dg = new DrawingGroup();
        dg.Children.Add(gd1);
        dg.Children.Add(gd2);

        return new DrawingBrush(dg) { TileMode = TileMode.Tile, Stretch = Stretch.None, Viewport = new Rect(0, 0, 8, 8), ViewportUnits = BrushMappingMode.Absolute };
    }

    public static Brush HueGradient { get; } = createHueGradientBrush();
    private static Brush createHueGradientBrush()
    {
        var r = new GradientStop(Color.FromRgb(255, 0, 0), 0);
        var y = new GradientStop(Color.FromRgb(255, 255, 0), 0.166);
        var g = new GradientStop(Color.FromRgb(0, 255, 0), 0.333);
        var c = new GradientStop(Color.FromRgb(0, 255, 255), 0.5);
        var b = new GradientStop(Color.FromRgb(0, 0, 255), 0.666);
        var m = new GradientStop(Color.FromRgb(255, 0, 255), 0.833);
        var r2 = new GradientStop(Color.FromRgb(255, 0, 0), 1);

        var gsc = new GradientStopCollection
        {
            r,
            y,
            g,
            c,
            b,
            m,
            r2
        };

        return new LinearGradientBrush(gsc, 0);
    }
}
