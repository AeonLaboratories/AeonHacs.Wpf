using AeonHacs.Wpf.Controls.Primitives;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace AeonHacs.Wpf.Controls;

[TemplatePart(Name = "PART_DecreaseButton", Type = typeof(ButtonBase))]
[TemplatePart(Name = "PART_IncreaseButton", Type = typeof(ButtonBase))]
public class NumericUpDown : SnapRange
{
    static NumericUpDown()
    {
        ValueProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(EnableDisableButtons));
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
    }

    protected static void EnableDisableButtons(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NumericUpDown n)
        {
            if (n.DecreaseButton != null)
                n.DecreaseButton.IsEnabled = n.Value > n.Minimum;
            if (n.IncreaseButton != null)
                n.IncreaseButton.IsEnabled = n.Value < n.Maximum;
        }
    }

    protected ButtonBase DecreaseButton { get; set; }
    protected ButtonBase IncreaseButton { get; set; }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        if (e.Delta < 0)
            DecreaseValue(Keyboard.Modifiers == ModifierKeys.Shift);
        else if (e.Delta > 0)
            IncreaseValue(Keyboard.Modifiers == ModifierKeys.Shift);

        base.OnMouseWheel(e);
    }

    public override void OnApplyTemplate()
    {
        if (DecreaseButton != null)
            DecreaseButton.Click -= Decrease;
        if (IncreaseButton != null)
            IncreaseButton.Click -= Increase;

        base.OnApplyTemplate();

        DecreaseButton = GetTemplateChild("PART_DecreaseButton") as ButtonBase;
        IncreaseButton = GetTemplateChild("PART_IncreaseButton") as ButtonBase;

        if (DecreaseButton != null)
        {
            DecreaseButton.Click += Decrease;
            DecreaseButton.IsEnabled = Value > Minimum;
        }

        if (IncreaseButton != null)
        {
            IncreaseButton.Click += Increase;
            IncreaseButton.IsEnabled = Value < Maximum;
        }

        void Decrease(object sender, RoutedEventArgs e) =>
            DecreaseValue(Keyboard.Modifiers == ModifierKeys.Shift);

        void Increase(object sender, RoutedEventArgs e) =>
            IncreaseValue(Keyboard.Modifiers == ModifierKeys.Shift);
    }
}
