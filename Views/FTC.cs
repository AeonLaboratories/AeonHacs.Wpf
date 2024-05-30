using AeonHacs.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Views
{
    /// <summary>
    /// Interaction logic for FTC.xaml
    /// </summary>
    public class FTC : View
    {
        public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(FTC));

        public static readonly DependencyProperty FillLevelProperty = DependencyProperty.Register(
            nameof(FillLevel), typeof(double), typeof(FTC), new FrameworkPropertyMetadata(0.0));

        static FTC()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FTC), new FrameworkPropertyMetadata(typeof(FTC)));
        }

        public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

        public double FillLevel { get => (double)GetValue(FillLevelProperty); set => SetValue(FillLevelProperty, value); }
    }
}
