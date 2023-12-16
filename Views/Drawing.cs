using HACS.WPF.Converters;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;

namespace HACS.WPF.Views
{
    public abstract class Drawing : Control
	{
		public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
			nameof(Angle), typeof(double), typeof(Drawing), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, AngleChanged, CoerceAngle));

		private static void AngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.SetValue(RenderTransformProperty, new RotateTransform((double)e.NewValue));
		}

		private static object CoerceAngle(DependencyObject d, object baseValue)
		{
			if (baseValue is double angle)
				return angle % 360.0;
			return 0.0;
		}

		public double Angle { get => (double)GetValue(AngleProperty); set => SetValue(AngleProperty, value); }



		public static readonly DependencyProperty HelpTextProperty = AutomationProperties.HelpTextProperty.AddOwner(typeof(Drawing));

		public string HelpText { get => (string)GetValue(HelpTextProperty); set => SetValue(HelpTextProperty, value); }


		public static readonly DependencyProperty ComponentProperty = View.ComponentProperty.AddOwner(typeof(Drawing));

		[TypeConverter(typeof(ViewModelConverter))]
		public INotifyPropertyChanged Component { get => (INotifyPropertyChanged)GetValue(ComponentProperty); set => SetValue(ComponentProperty, value); }

		static Drawing()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Drawing), new FrameworkPropertyMetadata(typeof(Drawing)));
		}
	}
}
