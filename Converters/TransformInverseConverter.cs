using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HACS.WPF.Converters
{
	[ValueConversion(typeof(Transform), typeof(Transform))]
	public class TransformInverseConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Transform transform)
				return transform.Inverse;
			return Transform.Identity;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//TODO should this be the same as above?
			return DependencyProperty.UnsetValue;
		}
	}
}
