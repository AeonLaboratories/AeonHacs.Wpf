using AeonHacs.Wpf.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace AeonHacs.Wpf.Views
{
    public class Heater : View
    {
        public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register(
            nameof(IsOn), typeof(bool), typeof(Heater), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty EllipticalProperty = DependencyProperty.Register(
            nameof(Elliptical), typeof(bool), typeof(Heater), new FrameworkPropertyMetadata(false));

        static Heater()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Heater), new FrameworkPropertyMetadata(typeof(Heater)));
        }

        [Category("Appearance")]
        public bool IsOn { get => (bool)GetValue(IsOnProperty); set => SetValue(IsOnProperty, value); }

        [Category("Appearance")]
        public bool Elliptical { get => (bool)GetValue(EllipticalProperty); set => SetValue(EllipticalProperty, value); }
    }
}
