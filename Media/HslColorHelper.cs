using System;
using System.Linq;
using System.Windows.Media;

namespace AeonHacs.Wpf.Media;

public static class HslColorHelper
{
	public static void ToHsl(this Color color, out int? H, out float S, out float L)
	{
		float r = (float)color.R / byte.MaxValue;
		float g = (float)color.G / byte.MaxValue;
		float b = (float)color.B / byte.MaxValue;

		var rgb = new[] { r, g, b };
		float min = rgb.Min();
		float max = rgb.Max();

		float range = max - min;

		L = (max + min) / 2f;

		if (range == 0)
			S = 0;
		else
			S = range / (1 - Math.Abs(2 * L - 1));

		if (range == 0)
		{
			// Hue is undefined
			H = null;
			return;
		}

		float h = 0;
		if (r == max)
			h = (g - b) / range;
		else if (g == max)
			h = 2 + (b - r) / range;
		else
			h = 4 + (r - g) / range;

		h *= 60;
		if (h < 0)
			h += 360;

		H = (int)Math.Round(h);
	}

	public static Color FromHsl(int H, float S, float L)
	{
		byte r = 0;
		byte g = 0;
		byte b = 0;

		if (S == 0)
		{
			r = g = b = (byte)(L * 255);
		}
		else
		{
			float v1, v2;
			//todo idiot proof this
			float hue = (float)H / 360;

			v2 = (L < 0.5) ? (L * (1 + S)) : ((L + S) - (L * S));
			v1 = 2 * L - v2;

			r = (byte)(255 * HueToRgb(v1, v2, hue + (1.0f / 3)));
			g = (byte)(255 * HueToRgb(v1, v2, hue));
			b = (byte)(255 * HueToRgb(v1, v2, hue - (1.0f / 3)));
		}

		return Color.FromRgb(r, g, b);

		float HueToRgb(float v1, float v2, float vH)
		{
			if (vH < 0)
				vH += 1;

			if (vH > 1)
				vH -= 1;

			if ((6 * vH) < 1)
				return (v1 + (v2 - v1) * 6 * vH);

			if ((2 * vH) < 1)
				return v2;

			if ((3 * vH) < 2)
				return (v1 + (v2 - v1) * ((2.0f / 3) - vH) * 6);

			return v1;
		}
	}
}
