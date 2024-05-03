using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AeonHacs.Wpf.Converters
{
	/// <summary>
	/// Coerces a given color into one that contains the specified ARGB values.
	/// </summary>
	[ValueConversion(typeof(Color), typeof(Color))]
	public class CoerceColorConverter : IValueConverter
	{
		public byte? A { get; set; }
		public byte? R { get; set; }
		public byte? G { get; set; }
		public byte? B { get; set; }

		protected byte ASource { get; set; }
		protected byte RSource { get; set; }
		protected byte GSource { get; set; }
		protected byte BSource { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Color color)
			{
				ASource = color.A;
				RSource = color.R;
				GSource = color.G;
				BSource = color.B;
				return Color.FromArgb(A ?? ASource, R ?? RSource, G ?? GSource, B ?? BSource);
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Color color)
			{
				return Color.FromArgb(A == null ? color.A : ASource, R == null ? color.R : RSource, G == null ? color.G : GSource, B == null ? color.B : BSource);
			}
			return value;
		}
	}
}
