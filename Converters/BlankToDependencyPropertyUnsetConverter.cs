﻿using System;
using System.Windows.Data;
using System.Windows;

namespace AeonHacs.Wpf.Converters
{
    public class BlankToDependencyPropertyUnsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string s)
                return s.IsBlank() ? DependencyProperty.UnsetValue : s;
            return value ?? DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
