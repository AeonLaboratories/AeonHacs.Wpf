using System;
using System.Windows.Media;

namespace AeonHacs.Wpf.Media;
public static class ColorExtensions
{
    public static Color Darken(this Color color, float amount)
    {
        amount = Math.Clamp(amount, 0, 1);
        color.ToHsl(out int? h, out float s, out float l);
        l = l - l * amount;
        return HslColorHelper.FromHsl(h ?? 0, s, l);
    }

    public static Color Lighten(this Color color, float amount)
    {
        amount = Math.Clamp(amount, 0, 1);
        color.ToHsl(out int? h, out float s, out float l);
        l = (1 - l) * amount + l;
        return HslColorHelper.FromHsl(h ?? 0, s, l);
    }
}
