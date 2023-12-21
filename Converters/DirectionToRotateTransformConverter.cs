using AeonHacs.Wpf.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace AeonHacs.Wpf.Converters
{
	[ValueConversion(typeof(Direction), typeof(RotateTransform))]
	public class DirectionToRotateTransformConverter : IValueConverter
	{
		public static DirectionToRotateTransformConverter Instance = new DirectionToRotateTransformConverter();

		protected DirectionToRotateTransformConverter() { }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (Direction)value switch
			{
				Direction.Left => new RotateTransform(90),
				Direction.Up => new RotateTransform(180),
				Direction.Right => new RotateTransform(-90),
				_ => null
			};
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new InvalidOperationException();
		}
	}
}
