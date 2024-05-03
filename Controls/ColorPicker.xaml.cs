using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AeonHacs.Wpf.Controls;

/// <summary>
/// Interaction logic for ColorPicker.xaml
/// </summary>
public partial class ColorPicker : UserControl
{
	#region A

	public static readonly DependencyProperty AProperty = DependencyProperty.Register(
		nameof(A), typeof(byte), typeof(ColorPicker), new PropertyMetadata(byte.MaxValue, AChanged));

	private static void AChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is ColorPicker p)
		{
			if (p.UpdateSource == DependencyProperty.UnsetValue)
			{
				p.UpdateSource = AProperty;
				p.Color = Color.FromArgb(p.A, p.R, p.G, p.B);
				p.UpdateSource = DependencyProperty.UnsetValue;
			}
		}
	}

	public byte A { get => (byte)GetValue(AProperty); set => SetValue(AProperty, value); }

	#endregion A

	#region R

	public static readonly DependencyProperty RProperty = DependencyProperty.Register(
		nameof(R), typeof(byte), typeof(ColorPicker), new PropertyMetadata(byte.MaxValue, RChanged));

	private static void RChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is ColorPicker p)
		{
			if (p.UpdateSource == DependencyProperty.UnsetValue)
			{
				p.UpdateSource = RProperty;
				p.Color = Color.FromArgb(p.A, p.R, p.G, p.B);
				//p.ASlider.Background = new LinearGradientBrush(Colors.Transparent, Color.FromArgb(255, p.R, p.G, p.B), 0);
				//p.GSlider.Background = new LinearGradientBrush(Color.FromRgb(p.R, 0, p.B), Color.FromRgb(p.R, 255, p.B), 0);
				//p.BSlider.Background = new LinearGradientBrush(Color.FromRgb(p.R, p.G, 0), Color.FromRgb(p.R, p.G, 255), 0);
				p.UpdateSource = DependencyProperty.UnsetValue;
			}
		}
	}

	public byte R { get => (byte)GetValue(RProperty); set => SetValue(RProperty, value); }

	#endregion R

	#region G

	public static readonly DependencyProperty GProperty = DependencyProperty.Register(
		nameof(G), typeof(byte), typeof(ColorPicker), new PropertyMetadata((byte)0, GChanged));

	private static void GChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is ColorPicker p)
		{
			if (p.UpdateSource == DependencyProperty.UnsetValue)
			{
				p.UpdateSource = GProperty;
				p.Color = Color.FromArgb(p.A, p.R, p.G, p.B);
				//p.ASlider.Background = new LinearGradientBrush(Colors.Transparent, Color.FromArgb(255, p.R, p.G, p.B), 0);
				//p.RSlider.Background = new LinearGradientBrush(Color.FromRgb(0, p.G, p.B), Color.FromRgb(255, p.G, p.B), 0);
				//p.BSlider.Background = new LinearGradientBrush(Color.FromRgb(p.R, p.G, 0), Color.FromRgb(p.R, p.G, 255), 0);
				p.UpdateSource = DependencyProperty.UnsetValue;
			}
		}
	}

	public byte G { get => (byte)GetValue(GProperty); set => SetValue(GProperty, value); }

	#endregion G

	#region B

	public static readonly DependencyProperty BProperty = DependencyProperty.Register(
		nameof(B), typeof(byte), typeof(ColorPicker), new PropertyMetadata((byte)0, BChanged));

	private static void BChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is ColorPicker p)
		{
			if (p.UpdateSource == DependencyProperty.UnsetValue)
			{
				p.UpdateSource = BProperty;
				p.Color = Color.FromArgb(p.A, p.R, p.G, p.B);
				//p.ASlider.Background = new LinearGradientBrush(Colors.Transparent, Color.FromArgb(255, p.R, p.G, p.B), 0);
				//p.RSlider.Background = new LinearGradientBrush(Color.FromRgb(0, p.G, p.B), Color.FromRgb(255, p.G, p.B), 0);
				//p.GSlider.Background = new LinearGradientBrush(Color.FromRgb(p.R, 0, p.B), Color.FromRgb(p.R, 255, p.B), 0);
				p.UpdateSource = DependencyProperty.UnsetValue;
			}
		}
	}

	public byte B { get => (byte)GetValue(BProperty); set => SetValue(BProperty, value); }

	#endregion B

	#region Color

	public class ColorChangedEventArgs : EventArgs
        {
		public Color Color { get; set; }

		public ColorChangedEventArgs() { }

		public ColorChangedEventArgs(Color color) =>
			Color = color;
	}

	public delegate void ColorChangedEvent(object sender, ColorChangedEventArgs e);

	public event ColorChangedEvent OnColorChanged;

	public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
		nameof(Color), typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.Red, ColorChanged));

	private static void ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is ColorPicker p)
		{
			if (p.UpdateSource == DependencyProperty.UnsetValue)
			{
				p.UpdateSource = ColorProperty;
				p.A = p.Color.A;
				p.R = p.Color.R;
				p.G = p.Color.G;
				p.B = p.Color.B;
				p.UpdateSource = DependencyProperty.UnsetValue;
			}
			if (!p.ColorHex.IsFocused)
				p.UpdateHex();
			p.OnColorChanged?.Invoke(p, new ColorChangedEventArgs(p.Color));
		}
	}

	public Color Color { get => (Color)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }

	#endregion Color

	protected object UpdateSource { get; set; } = DependencyProperty.UnsetValue;

	public ColorPicker()
	{
		InitializeComponent();
		UpdateHex();
	}

	public ColorPicker(Color startingColor) : this() =>
		Color = startingColor;

	protected void UpdateHex() =>
		ColorHex.Text = $"#{(A == 255 ? "" : $"{A:X2}")}{R:X2}{G:X2}{B:X2}";

	private void ColorHexChanged(object sender, TextChangedEventArgs e)
	{
		if (UpdateSource == DependencyProperty.UnsetValue)
		{
			try { Color = (Color)ColorConverter.ConvertFromString(ColorHex.Text); }
			catch { }
		}
	}

	private void ColorHex_LostFocus(object sender, RoutedEventArgs e)
	{
		UpdateHex();
	}
}
