using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters
{
    /// <summary>
    /// If the given value matches the specified parameter, return the first of two output options;
    /// otherwise, return the second.
    /// </summary>
    public class MatchConverter : IValueConverter
    {
        public static MatchConverter Default = new MatchConverter()
        {
            MatchValue = true,
            MismatchValue = false
        };
        public static MatchConverter VisibleCollapsed = new MatchConverter()
        {
            MatchValue = Visibility.Visible,
            MismatchValue = Visibility.Collapsed
        };
        public static MatchConverter VisibleHidden = new MatchConverter()
        {
            MatchValue = Visibility.Visible,
            MismatchValue = Visibility.Hidden
        };

        public object MatchValue { get; set; }

        public object MismatchValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var match = value?.Equals(parameter) ?? parameter == null;
            return match ? MatchValue : MismatchValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
