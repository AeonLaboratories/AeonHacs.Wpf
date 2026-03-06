using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AeonHacs.Wpf.Data;

public class DataPointCard : Control
{
    public static readonly DependencyProperty DataPointProperty =
        DependencyProperty.Register(nameof(DataPoint), typeof(DataPoint), typeof(DataPointCard), new PropertyMetadata(default(DataPoint), OnDataPointChanged));

    public DataPoint DataPoint
    {
        get => (DataPoint)GetValue(DataPointProperty);
        set => SetValue(DataPointProperty, value);
    }

    public double Input
    {
        get => DataPoint.Input;
        set => DataPoint = DataPoint with { Input = value };
    }

    public double Output
    {
        get => DataPoint.Output;
        set => DataPoint = DataPoint with { Output = value };
    }

    static DataPointCard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DataPointCard), new FrameworkPropertyMetadata(typeof(DataPointCard)));
    }

    private static void OnDataPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DataPointCard card && e.OldValue is DataPoint oldPoint && e.NewValue is DataPoint newPoint)
        {
            if (oldPoint.Input != newPoint.Input || oldPoint.Output != newPoint.Output)
            {
                if (card.IsKeyboardFocusWithin)
                {
                    if (PchipInterpolatorCard.UpdateDataPoint.CanExecute((oldPoint, newPoint), card))
                        PchipInterpolatorCard.UpdateDataPoint.Execute((oldPoint, newPoint), card);
                }
            }
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.Key == Key.Enter && e.OriginalSource is TextBox textBox)
        {
            var binding = textBox.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();
        }
    }
}
