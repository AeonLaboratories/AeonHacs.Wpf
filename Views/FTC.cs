using AeonHacs.Wpf.Controls;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AeonHacs.Wpf.Views;

/// <summary>
/// Interaction logic for FTC.xaml
/// </summary>
public class FTC : View
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(FTC));

    public static readonly DependencyProperty FillLevelProperty = DependencyProperty.Register(
        nameof(FillLevel), typeof(double), typeof(FTC), new FrameworkPropertyMetadata(0.0));

    public static readonly DependencyProperty MaxFillLevelProperty = DependencyProperty.Register(
        nameof(MaxFillLevel), typeof(double), typeof(FTC), new FrameworkPropertyMetadata(0.8,  UpdateGradientBounds, CoerceMaxFillLevel));

    private static void UpdateGradientBounds(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FTC ftc)
        {
            switch (ftc.Orientation)
            {
                case RelativeDirection.Up:
                    ftc.FillBrush.StartPoint = new Point(0.5, 0.0);
                    ftc.FillBrush.EndPoint = new Point(0.5, ftc.MaxFillLevel);
                    break;
                case RelativeDirection.Left:
                    ftc.FillBrush.StartPoint = new Point(0.0, 0.5);
                    ftc.FillBrush.EndPoint = new Point(ftc.MaxFillLevel, 0.5);
                    break;
                case RelativeDirection.Right:
                    ftc.FillBrush.StartPoint = new Point(1.0, 0.5);
                    ftc.FillBrush.EndPoint = new Point(1.0 - ftc.MaxFillLevel, 0.5);
                    break;
                default:
                case RelativeDirection.Down:
                    ftc.FillBrush.StartPoint = new Point(0.5, 1.0);
                    ftc.FillBrush.EndPoint = new Point(0.5, 1 - ftc.MaxFillLevel);
                    break;
            }
        }
    }

    private static object CoerceMaxFillLevel(DependencyObject d, object baseValue)
    {
        if (baseValue is double value)
        {
            return Math.Clamp(value, 0.0, 1.0);
        }
        return 0.8;
    }

    public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register(
        nameof(FillColor), typeof(Color), typeof(FTC), new FrameworkPropertyMetadata(Colors.Transparent));

    public static readonly DependencyPropertyKey FillBrushPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(FillBrush), typeof(LinearGradientBrush), typeof(FTC), new FrameworkPropertyMetadata());

    static FTC()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FTC), new FrameworkPropertyMetadata(typeof(FTC)));
        OrientationProperty.OverrideMetadata(typeof(FTC), new FrameworkPropertyMetadata(RelativeDirection.Down, UpdateGradientBounds));
    }

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    public double FillLevel { get => (double)GetValue(FillLevelProperty); set => SetValue(FillLevelProperty, value); }

    public double MaxFillLevel { get => (double)GetValue(MaxFillLevelProperty); set => SetValue(MaxFillLevelProperty, value); }

    public Color FillColor { get => (Color)GetValue(FillColorProperty); set => SetValue(FillColorProperty, value); }

    public LinearGradientBrush FillBrush { get => (LinearGradientBrush)GetValue(FillBrushPropertyKey.DependencyProperty); protected set => SetValue(FillBrushPropertyKey, value); }

    public FTC()
    {
        var gs1 = new GradientStop();
        BindingOperations.SetBinding(gs1, GradientStop.ColorProperty, new Binding(nameof(FillColor)) { Source = this });
        BindingOperations.SetBinding(gs1, GradientStop.OffsetProperty, new Binding(nameof(FillLevel)) { Source = this });

        var gs2 = new GradientStop();
        gs2.Color = Colors.Transparent;
        BindingOperations.SetBinding(gs2, GradientStop.OffsetProperty, new Binding(nameof(FillLevel)) { Source = this });

        FillBrush = new LinearGradientBrush
        {
            StartPoint = new Point(0.5, 1.0),
            EndPoint = new Point(0.5, 1 - MaxFillLevel),
            GradientStops = { gs1, gs2 }
        };
    }
}
