using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AeonHacs.Wpf.Data;

public class PchipInterpolatorCard : Control
{
    public static readonly RoutedUICommand AddDataPoint = new("Add", nameof(AddDataPoint), typeof(PchipInterpolatorCard));
    public static readonly RoutedUICommand RemoveDataPoint = new("Remove", nameof(RemoveDataPoint), typeof(PchipInterpolatorCard));
    public static readonly RoutedUICommand UpdateDataPoint = new("Update", nameof(UpdateDataPoint), typeof(PchipInterpolatorCard));

    public static readonly DependencyProperty InterpolatorProperty =
        DependencyProperty.Register(
            nameof(Interpolator),
            typeof(PchipInterpolator),
            typeof(PchipInterpolatorCard),
            new PropertyMetadata(InterpolatorChanged));

    private static void InterpolatorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is PchipInterpolator oldInterpolator)
            BindingOperations.DisableCollectionSynchronization(oldInterpolator.CalibrationData);
        if (e.NewValue is PchipInterpolator newInterpolator)
            BindingOperations.EnableCollectionSynchronization(newInterpolator.CalibrationData, new());
    }

    public PchipInterpolator Interpolator
    {
        get => (PchipInterpolator)GetValue(InterpolatorProperty);
        set => SetValue(InterpolatorProperty, value);
    }

    static PchipInterpolatorCard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PchipInterpolatorCard), new FrameworkPropertyMetadata(typeof(PchipInterpolatorCard)));
    }

    public PchipInterpolatorCard()
    {
        InitializeCommands();
    }

    protected virtual void InitializeCommands()
    {
        CommandBindings.Add(new(AddDataPoint, Add));
        CommandBindings.Add(new(RemoveDataPoint, Remove, CanRemove));
        CommandBindings.Add(new(UpdateDataPoint, Update));
    }

    private void Add(object sender, RoutedEventArgs e)
    {
        if (Interpolator is not { CalibrationData: var data })
            return;

        if (data.Count == 0)
        {
            data.Add(new() { Input = 0, Output = 0 });
            return;
        }

        var last = data[^1];
        if (data.Count > 1)
        {
            var prev = data[^2];
            var x = last.Input + (last.Input - prev.Input);
            data.Add(new() { Input = x, Output = Interpolator.Interpolate(x) });
        }
        else
        {
            var x = last.Input + 1;
            data.Add(new() { Input = x, Output = x });
        }
    }

    private void Remove(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is DataPoint d)
            Interpolator?.CalibrationData.Remove(d);
    }

    private void CanRemove(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = Interpolator is { } interpolator && interpolator.CalibrationData.Count > 2;
    }

    private void Update(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is ValueTuple<DataPoint, DataPoint> p && Interpolator is { CalibrationData: var data })
        {
            var (oldPoint, newPoint) = p;

            var focusedTextBox = Keyboard.FocusedElement as TextBox;
            bool isInput = focusedTextBox?.GetBindingExpression(TextBox.TextProperty)?.ParentBinding.Path.Path == "Input";
            int selectionStart = focusedTextBox?.SelectionStart ?? 0;
            int selectionLength = focusedTextBox?.SelectionLength ?? 0;

            data.Remove(oldPoint);
            data.Add(newPoint);

            if (focusedTextBox != null)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    var cards = FindVisualChildren<DataPointCard>(this);
                    if (cards.FirstOrDefault(c => c.DataPoint.Equals(newPoint)) is { } newCard)
                    {
                        var textBoxes = FindVisualChildren<TextBox>(newCard).ToList();
                        var targetTextBox = isInput ? textBoxes.FirstOrDefault() : textBoxes.ElementAtOrDefault(1);
                        if (targetTextBox != null)
                        {
                            targetTextBox.Focus();
                            targetTextBox.SelectionStart = selectionStart;
                            targetTextBox.SelectionLength = selectionLength;
                        }
                    }
                }, System.Windows.Threading.DispatcherPriority.Loaded);
            }
        }
    }

    private static System.Collections.Generic.IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        if (depObj == null) yield break;
        for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++)
        {
            DependencyObject child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);
            if (child is T t)
                yield return t;

            foreach (T childOfChild in FindVisualChildren<T>(child))
                yield return childOfChild;
        }
    }
}
