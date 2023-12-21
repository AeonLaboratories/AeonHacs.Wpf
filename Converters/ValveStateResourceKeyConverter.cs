using AeonHacs;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters
{
	[ValueConversion(typeof(ValveState), typeof(string))]
	public class ValveStateResourceKeyConverter : IValueConverter
	{
		public static ValveStateResourceKeyConverter Default = new ValveStateResourceKeyConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is ValveState state)
			{
				switch (state)
				{
					case ValveState.Closed:
						return "ClosedBrush";
					case ValveState.Opened:
						return "OpenedBrush";
					case ValveState.Closing:
						return "ClosingBrush";
					case ValveState.Opening:
						return "OpeningBrush";
					default:
					case ValveState.Unknown:
					case ValveState.Other:
						break;
				}
			}
			return "UnknownBrush";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
