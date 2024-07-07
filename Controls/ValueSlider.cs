using AeonHacs.Wpf.Controls.Primitives;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AeonHacs.Wpf.Controls;

//TODO can I simplify the type of these objects?
[TemplatePart(Name = "PART_Bounds", Type = typeof(Canvas))]
[TemplatePart(Name = "PART_Slider", Type = typeof(Canvas))]
public class ValueSlider : SnapRange
{
    #region Orientation

    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation), typeof(Orientation), typeof(ValueSlider), new PropertyMetadata(Orientation.Horizontal));

    public Orientation Orientation { get => (Orientation)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    #endregion Orientation

    #region SliderPosition

    public static readonly DependencyProperty SliderPositionProperty = DependencyProperty.Register(
        nameof(SliderPosition), typeof(Point), typeof(ColorSlider), new PropertyMetadata(new Point(0, 0)));

    public Point SliderPosition { get => (Point)GetValue(SliderPositionProperty); set => SetValue(SliderPositionProperty, value); }

    #endregion SliderPosition

    //TODO can I remove the need for this?
    protected Canvas Bounds { get; set; }
    protected Canvas Slider { get; set; }

    static ValueSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ValueSlider), new FrameworkPropertyMetadata(typeof(ValueSlider)));
    }

    protected virtual void UpdateSliderPosition()
    {
        if (!IsArrangeValid || Maximum == Minimum || Bounds == null || Slider == null)
            return;

        double pos = (Value - Minimum) / Maximum - Minimum;
        pos *= Bounds.ActualWidth;
        Canvas.SetLeft(Slider, pos);
    }

    protected override void OnValueChanged(double oldValue, double newValue)
    {
        UpdateSliderPosition();
        base.OnValueChanged(oldValue, newValue);
    }

    protected virtual void UpdateValueFromMousePosition(MouseEventArgs e)
    {
        double value = Math.Clamp(e.GetPosition(Bounds).X, 0, Bounds.ActualWidth);
        
        value /= Bounds.ActualWidth;
        value *= Maximum - Minimum;
        value += Minimum;
        
        Value = value;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        Bounds = GetTemplateChild("PART_Bounds") as Canvas;
        Slider = GetTemplateChild("PART_Slider") as Canvas;
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        UpdateSliderPosition();
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        Keyboard.Focus(this);
        CaptureMouse();
        UpdateValueFromMousePosition(e);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        ReleaseMouseCapture();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (IsMouseCaptured)
            UpdateValueFromMousePosition(e);
    }

    //TODO cleanup
    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
        {
            e.Handled = true;
            return;
        }
        if (e.Key == Key.Home)
        {
            Value = Minimum;
            e.Handled = true;
            return;
        }
        if (e.Key == Key.End)
        {
            Value = Maximum;
            e.Handled = true;
            return;
        }

        bool shift = Keyboard.Modifiers == ModifierKeys.Shift;

        if (e.Key == Key.Left)
            DecreaseValue(shift);
        else if (e.Key == Key.Up)
        {
            if (Orientation == Orientation.Vertical)
                DecreaseValue(shift);
            else
                IncreaseValue(shift);
        }
        else if (e.Key == Key.Right)
            IncreaseValue(shift);
        else if (e.Key == Key.Down)
        {
            if (Orientation == Orientation.Vertical)
                IncreaseValue(shift);
            else
                DecreaseValue(shift);
        }
        else if (e.Key == Key.PageUp)
        {
            if (Orientation == Orientation.Vertical)
                DecreaseValue(true);
            else
                IncreaseValue(true);
        }
        else if (e.Key == Key.PageDown)
        {
            if (Orientation == Orientation.Vertical)
                IncreaseValue(true);
            else
                DecreaseValue(true);
        }
        else
        {
            base.OnKeyDown(e);
            return;
        }

        e.Handled = true;
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        bool shift = Keyboard.Modifiers == ModifierKeys.Shift;

        if (e.Delta < 0)
            DecreaseValue(shift);
        else if (e.Delta > 0)
            IncreaseValue(shift);

        base.OnMouseWheel(e);
    }
}