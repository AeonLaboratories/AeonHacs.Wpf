using AeonHacs.Wpf.Converters;
using AeonHacs.Wpf.Views;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AeonHacs.Wpf.Shapes;

public class Chamber : Fitting
{
    public static readonly DependencyProperty BackgroundProperty = Control.BackgroundProperty.AddOwner(typeof(Chamber),
        new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty BoundsProperty = DependencyProperty.Register(
        nameof(Bounds), typeof(Geometry), typeof(Chamber), new FrameworkPropertyMetadata(Geometry.Empty, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    public static readonly DependencyProperty ComponentProperty = View.ComponentProperty.AddOwner(typeof(Chamber));

    static Chamber()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Chamber), new FrameworkPropertyMetadata(typeof(Chamber)));
    }

    public Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    Geometry bounds;
    public Geometry Bounds
    {
        get => (ReadLocalValue(BoundsProperty) != DependencyProperty.UnsetValue) ? (Geometry)GetValue(BoundsProperty) : bounds;
        set => SetValue(BoundsProperty, value);
    }

    [TypeConverter(typeof(ViewModelConverter))]
    public INotifyPropertyChanged Component
    {
        get => (INotifyPropertyChanged)GetValue(ComponentProperty);
        set => SetValue(ComponentProperty, value);
    }

    protected override Geometry DefineGeometry()
    {
        bounds = Data.GetWidenedPathGeometry(new Pen(Brushes.Black, 25));

        return base.DefineGeometry();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        var bounds = DefiningGeometry.GetRenderBounds(new Pen(Stroke, StrokeThickness));
        drawingContext.PushTransform(new TranslateTransform(-bounds.Left, -bounds.Top));
        drawingContext.DrawGeometry(Background, null, Bounds);
        drawingContext.Pop();

        base.OnRender(drawingContext);
    }
}
