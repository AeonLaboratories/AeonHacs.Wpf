using System.Windows;

namespace AeonHacs.Wpf.Views
{
    public class Gauge : View
    {
        public static readonly DependencyProperty DefaultContentStringProperty = DependencyProperty.Register(
            nameof(DefaultContentString), typeof(string), typeof(Gauge), new PropertyMetadata("1000"));

        public string DefaultContentString { get => GetValue(DefaultContentStringProperty) as string; set => SetValue(DefaultContentStringProperty, value); }

        static Gauge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Gauge), new FrameworkPropertyMetadata(typeof(Gauge)));
        }
    }
}
