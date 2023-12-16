using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HACS.WPF.Converters
{
	[ValueConversion(typeof(bool), typeof(Brush))]
	public class BoolToBrushConverter : IValueConverter
	{
		public static BoolToBrushConverter Default = new BoolToBrushConverter();
		public static BoolToBrushConverter Heater = new BoolToBrushConverter() { Context = ConverterContext.Heater };
		public enum ConverterContext { Default, Heater }
		public static Dictionary<ConverterContext, BoolToResourceKeyConverter> Converter = new Dictionary<ConverterContext, BoolToResourceKeyConverter>()
		{
			{ConverterContext.Default, BoolToResourceKeyConverter.Default},
			{ConverterContext.Heater, BoolToResourceKeyConverter.Heater},
		};

		public ConverterContext Context { get; set; }
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            var key = Converter[Context].ResourceKey(value is bool t && t);
			return (Brush)Application.Current.Resources[key];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}