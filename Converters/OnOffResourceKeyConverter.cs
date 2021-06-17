using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace HACS.WPF.Converters
{
	[ValueConversion(typeof(bool), typeof(string))]
	public class OnOffResourceKeyConverter : IValueConverter
	{
		public static OnOffResourceKeyConverter Default = new OnOffResourceKeyConverter();
		public static OnOffResourceKeyConverter Heater = new OnOffResourceKeyConverter() { Context = ConverterContext.Heater };
		//TODO implement
		//public static OnOffResourceKeyConverter Foreground = new OnOffResourceKeyConverter() { Context = ConverterContext.Foreground };

		public enum ConverterContext { Default, /*Foreground,*/ Heater }

		public ConverterContext Context { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool on))
				on = false;

			if (Context == ConverterContext.Heater)
				return on ? "HotBrush" : "NeutralBrush";

			return on ? "OnBrush" : "OffBrush";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
