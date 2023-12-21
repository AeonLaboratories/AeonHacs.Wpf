using AeonHacs.Components;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters
{
	[ValueConversion(typeof(double), typeof(double))]
	public class ScaledValueConverter : IValueConverter
	{
		public static ScaledValueConverter LiquidNitrogen = new ScaledValueConverter(-195.8, 20.0);

		public double MinInput { get; }
		public double MaxInput { get; }

		public ScaledValueConverter(double minInput, double maxInput)
		{
			MinInput = minInput;
			MaxInput = maxInput;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double input)
			{
				var ratio = (input - MinInput) / (MaxInput - MinInput);
				return Math.Clamp(ratio, 0.0, 1.0);
			}
			else
				return 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}