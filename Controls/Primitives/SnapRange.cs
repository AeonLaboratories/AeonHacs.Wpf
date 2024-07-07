using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace AeonHacs.Wpf.Controls.Primitives;

//TODO I think this functionality might be present in a built in control.
public abstract class SnapRange : RangeBase
{
    #region IsSnapToSmallChangeEnabled

    public static readonly DependencyProperty IsSnapToSmallChangeEnabledProperty = DependencyProperty.Register(
        nameof(IsSnapToSmallChangeEnabled), typeof(bool), typeof(SnapRange), new PropertyMetadata(true, IsSnapToSmallChangeEnabledChanged));

    private static void IsSnapToSmallChangeEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SnapRange n)
        {
            double snapped = Math.Round(n.Value / n.SmallChange) * n.SmallChange;
            if (n.Value != snapped)
                n.Value = snapped;
        }
    }

    public bool IsSnapToSmallChangeEnabled
    {
        get => (bool)GetValue(IsSnapToSmallChangeEnabledProperty);
        set => SetValue(IsSnapToSmallChangeEnabledProperty, value);
    }

    #endregion IsSnapToSmallChangeEnabled

    #region Value

    private static object CoerceValue(DependencyObject d, object baseValue)
    {
        if (d is SnapRange s && s.TemplateApplied)
        {
            double value = Math.Clamp((double)baseValue, s.Minimum, s.Maximum);
            if (value != (double)baseValue)
                s.Value = value;
            return s.Snap(value);
        }
        return baseValue;
    }

    #endregion Value

    protected bool TemplateApplied { get; set; }

    static SnapRange()
    {
        ValueProperty.OverrideMetadata(typeof(SnapRange), new FrameworkPropertyMetadata(null, CoerceValue));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        TemplateApplied = true;
        CoerceValue(ValueProperty);
    }

    protected virtual double Snap(double value) =>
        IsSnapToSmallChangeEnabled ? Math.Round(value / SmallChange) * SmallChange : value;

    protected void IncreaseValueBy(double amount) =>
        Value = Snap(Math.Min(Value + amount, Maximum));

    protected virtual void IncreaseValue(bool largeChange) =>
        IncreaseValueBy(largeChange ? LargeChange : SmallChange);

    protected void DecreaseValueBy(double amount) =>
        Value = Snap(Math.Max(Value - amount, Minimum));

    protected virtual void DecreaseValue(bool largeChange) =>
        DecreaseValueBy(largeChange ? LargeChange : SmallChange);
}
