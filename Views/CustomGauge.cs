using AeonHacs.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Views;

public class CustomGauge : Gauge
{
    public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
        nameof(DisplayMemberPath), typeof(string), typeof(CustomGauge), new FrameworkPropertyMetadata("", DisplayMemberPathChanged));

    private static void DisplayMemberPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace((string)e.NewValue))
            return;

        if (d is FrameworkElement fe)
            fe.SetBinding(ContentProperty, new Binding($"Component.{e.NewValue}") { Source = d });
    }

    public string DisplayMemberPath { get => (string)GetValue(DisplayMemberPathProperty); set => SetValue(DisplayMemberPathProperty, value); }

    static CustomGauge()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomGauge), new FrameworkPropertyMetadata(typeof(CustomGauge)));
    }
}
