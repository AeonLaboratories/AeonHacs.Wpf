using AeonHacs.Wpf.Media;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AeonHacs.Wpf.Controls;

[TemplatePart(Name = "PART_Bounds", Type = typeof(Panel))]
[TemplatePart(Name = "PART_HueGradientStop", Type = typeof(GradientStop))]
public class SLCanvas : Control
{
    #region Hue

    public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
        nameof(Hue), typeof(int), typeof(SLCanvas), new FrameworkPropertyMetadata(0, HueChanged, CoerceHue));

    private static void HueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SLCanvas c)
        {
            if (c.UpdateSource == DependencyProperty.UnsetValue)
            {
                c.UpdateSource = HueProperty;
                c.SelectedColor = c.ColorFromSelectorPosition();
                c.UpdateSource = DependencyProperty.UnsetValue;
            }
            if (c.HueGradientStop != null)
                c.HueGradientStop.Color = c.HueColor;
        }
    }

    private static object CoerceHue(DependencyObject d, object baseValue)
    {
        var value = (int)baseValue;
        if (value == 360)
            value = 0;
        value = Math.Clamp(value, 0, 359);
        if (d is SLCanvas c && value != (int)baseValue)
            c.Hue = value;
        return value;
    }

    public int Hue { get => (int)GetValue(HueProperty); set => SetValue(HueProperty, value); }

    #endregion Hue

    #region SelectedColor

    public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
        nameof(SelectedColor), typeof(Color), typeof(SLCanvas), new PropertyMetadata(Colors.Red, SelectedColorChanged));

    private static void SelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SLCanvas c)
        {
            if (c.UpdateSource == DependencyProperty.UnsetValue)
            {
                c.UpdateSource = SelectedColorProperty;
                c.SelectedColor.ToHsl(out int? H, out float S, out float L);
                if (H != null)
                    c.Hue = (int)H;
                c.SelectorPosition = c.SelectorPositionFromColor();
                c.UpdateSource = DependencyProperty.UnsetValue;
            }
        }
    }

    public Color SelectedColor { get => (Color)GetValue(SelectedColorProperty); set => SetValue(SelectedColorProperty, value); }

    #endregion SelectedColor

    #region SelectorPosition

    public static readonly DependencyProperty SelectorPositionProperty = DependencyProperty.Register(
        nameof(SelectorPosition), typeof(Point), typeof(SLCanvas), new FrameworkPropertyMetadata(SelectorPositionChanged, CoerceSelectorPosition));

    private static object CoerceSelectorPosition(DependencyObject d, object baseValue)
    {
        var pos = (Point)baseValue;
        if (d is SLCanvas c)
        {
            pos.X = Math.Clamp(pos.X, 0, c.Bounds?.RenderSize.Width ?? c.RenderSize.Width);
            pos.Y = Math.Clamp(pos.Y, 0, c.Bounds?.RenderSize.Height ?? c.RenderSize.Height);
            if (pos != (Point)baseValue)
                c.SelectorPosition = pos;
        }
        return pos;
    }

    private static void SelectorPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SLCanvas c)
        {
            if (c.UpdateSource == DependencyProperty.UnsetValue)
            {
                c.UpdateSource = SelectorPositionProperty;
                c.SelectedColor = c.ColorFromSelectorPosition();
                c.UpdateSource = DependencyProperty.UnsetValue;
            }
        }
    }

    public Point SelectorPosition { get => (Point)GetValue(SelectorPositionProperty); set => SetValue(SelectorPositionProperty, value); }

    #endregion SelectorPosition

    protected object UpdateSource { get; set; } = DependencyProperty.UnsetValue;

    //TODO can either of these be gotten rid of with smarter bindings?
    protected Panel Bounds { get; set; }
    protected GradientStop HueGradientStop { get; set; }

    protected Color HueColor => HslColorHelper.FromHsl(Hue, 1f, 0.5f);

    static SLCanvas()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SLCanvas), new FrameworkPropertyMetadata(typeof(SLCanvas)));
    }

    protected virtual Point SelectorPositionFromColor()
    {
        SelectedColor.ToHsl(out int? H, out float S, out float L);

        var rgb = new[] { SelectedColor.R, SelectedColor.G, SelectedColor.B };
        float min = rgb.Min();
        float max = rgb.Max();

        double x, y;
        double width = Bounds?.RenderSize.Width ?? RenderSize.Width;
        double height = Bounds?.RenderSize.Height ?? RenderSize.Height;
        if (max == 0)
            x = width;
        else
            x = width * (1 - (min / max));
        y = height * (1 - (max / 255));

        return new Point(x, y);
    }

    protected virtual Color ColorFromSelectorPosition()
    {
        float max = (float)(1 - (SelectorPosition.Y / Bounds?.RenderSize.Height ?? RenderSize.Height));
        float min = (float)(1 - (SelectorPosition.X / Bounds?.RenderSize.Width ?? RenderSize.Width)) * max;

        float range = max - min;

        float L = (max + min) / 2;

        float S = 0;

        if (range != 0)
            S = range / (1 - Math.Abs(2 * L - 1));

        return HslColorHelper.FromHsl(Hue, S, L);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        Bounds = GetTemplateChild("PART_Bounds") as Panel;
        HueGradientStop = GetTemplateChild("PART_HueGradientStop") as GradientStop;

        if (Bounds != null)
        {
            Bounds.MouseDown += Canvas_MouseDown;
            Bounds.MouseUp += Canvas_MouseUp;
            Bounds.MouseMove += Canvas_MouseMove;
        }

        if (HueGradientStop != null)
            HueGradientStop.Color = HslColorHelper.FromHsl(Hue, 1f, 0.5f);

        SelectorPosition = SelectorPositionFromColor();
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        
        // Just update the selector position. No need to update color.
        UpdateSource = null;
        SelectorPosition = SelectorPositionFromColor();
        UpdateSource = DependencyProperty.UnsetValue;
    }

    private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Bounds.CaptureMouse();
        SelectorPosition = e.GetPosition(Bounds);
    }

    private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
    {
        Bounds.ReleaseMouseCapture();
    }

    private void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (Bounds.IsMouseCaptured)
        {
            SelectorPosition = e.GetPosition(Bounds);
        }
    }
}
