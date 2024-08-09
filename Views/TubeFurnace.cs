using System.Windows;

namespace AeonHacs.Wpf.Views;

public enum TubeFurnaceType
{
    A,
    B
}

public class TubeFurnace : View
{
    public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
        nameof(Type), typeof(TubeFurnaceType), typeof(TubeFurnace), new PropertyMetadata(TubeFurnaceType.A));

    public TubeFurnaceType Type
    {
        get => (TubeFurnaceType)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    static TubeFurnace()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TubeFurnace), new FrameworkPropertyMetadata(typeof(TubeFurnace)));
    }
}
