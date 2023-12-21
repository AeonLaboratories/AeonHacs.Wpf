using System;
using System.Globalization;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters
{
	[ValueConversion(typeof(bool), typeof(string))]
	public class BoolToResourceKeyConverter : IValueConverter
	{
		public static BoolToResourceKeyConverter Default = new BoolToResourceKeyConverter()
		{
			TrueValue = "OnBrush",
			FalseValue = "OffBrush"
		};
		public static BoolToResourceKeyConverter Heater = new BoolToResourceKeyConverter()
		{
			TrueValue = "HotBrush",
			FalseValue = "NeutralBrush"
		};
		public static BoolToResourceKeyConverter HighVacuum = new BoolToResourceKeyConverter()
		{
			TrueValue = "HighVacuumBrush",
			FalseValue = "NeutralBrush"
		};

		protected string TrueValue { get; set; }

		protected string FalseValue { get; set; }

		public string ResourceKey(bool t) =>
			t ? TrueValue : FalseValue;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
			ResourceKey(value is bool t && t);

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
