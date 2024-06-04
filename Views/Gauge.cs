using System.Windows;
using System.Windows.Data;

namespace AeonHacs.Wpf.Views
{
	public class Gauge : View
	{
		public static readonly DependencyProperty DefaultContentStringProperty = DependencyProperty.Register(
			nameof(DefaultContentString), typeof(string), typeof(Gauge), new PropertyMetadata("1000"));

        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
        nameof(DisplayMemberPath), typeof(string), typeof(Gauge), new FrameworkPropertyMetadata("", DisplayMemberPathChanged));

        private static void DisplayMemberPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((string)e.NewValue))
                return;

            if (d is FrameworkElement fe)
                fe.SetBinding(ContentProperty, new Binding($"{e.NewValue}"));
        }

        public string DefaultContentString { get => GetValue(DefaultContentStringProperty) as string; set => SetValue(DefaultContentStringProperty, value); }

        public string DisplayMemberPath { get => (string)GetValue(DisplayMemberPathProperty); set => SetValue(DisplayMemberPathProperty, value); }

        static Gauge()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Gauge), new FrameworkPropertyMetadata(typeof(Gauge)));
		}
	}
}
