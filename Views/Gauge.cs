using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace AeonHacs.Wpf.Views;

public class Gauge : View
{
    public static readonly DependencyProperty DefaultContentStringProperty = DependencyProperty.Register(
        nameof(DefaultContentString), typeof(string), typeof(Gauge), new PropertyMetadata("1000"));

    public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
        nameof(DisplayMemberPath), typeof(string), typeof(Gauge), new FrameworkPropertyMetadata("", DisplayMemberPathChanged));

    private static void DisplayMemberPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace((string)e.NewValue))
        {
            BindingOperations.ClearBinding(d, ContentProperty);
            return;
        }

        BindingOperations.SetBinding(d, ContentProperty, new Binding($"Component.{e.NewValue}") { Source = d });
    }

    public string DefaultContentString { get => GetValue(DefaultContentStringProperty) as string; set => SetValue(DefaultContentStringProperty, value); }

    public string DisplayMemberPath { get => (string)GetValue(DisplayMemberPathProperty); set => SetValue(DisplayMemberPathProperty, value); }

    static Gauge()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Gauge), new FrameworkPropertyMetadata(typeof(Gauge)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        AdornerLayer.GetAdornerLayer(this)?.Add(new SetpointAdorner(this));
    }
}
