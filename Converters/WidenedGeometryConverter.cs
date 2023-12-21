using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace AeonHacs.Wpf.Converters
{
	[ValueConversion(typeof(Geometry), typeof(Geometry))]
	public class WidenedGeometryConverter : IMultiValueConverter
	{
		public static WidenedGeometryConverter Instance = new WidenedGeometryConverter();

		protected WidenedGeometryConverter() { }

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var geometry = values.ElementAtOrDefault(0) as Geometry;
			var thickness = (double?)values.ElementAtOrDefault(1) ?? 1.0;
			var widened = geometry?.GetWidenedPathGeometry(new Pen(Brushes.Black, thickness));
			return new CombinedGeometry(GeometryCombineMode.Union, widened, null);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new InvalidOperationException();
		}
	}
}
