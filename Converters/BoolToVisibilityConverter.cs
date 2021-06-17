using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace HACS.WPF.Converters
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public enum FalseVisibilityBehavior { Collapsed, Hidden }

		public static BoolToVisibilityConverter Default = new BoolToVisibilityConverter();
		public static BoolToVisibilityConverter Hidden = new BoolToVisibilityConverter() { FalseVisibility = FalseVisibilityBehavior.Hidden };

		public FalseVisibilityBehavior FalseVisibility { get; set; } = FalseVisibilityBehavior.Collapsed;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool b && b)
				return Visibility.Visible;
			return FalseVisibility == FalseVisibilityBehavior.Hidden ? Visibility.Hidden : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
