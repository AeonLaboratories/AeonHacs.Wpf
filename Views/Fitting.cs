using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HACS.WPF.Views
{
    public class Fitting : Control
	{
		#region Data
		public static readonly DependencyProperty DataProperty = Path.DataProperty.AddOwner(typeof(Fitting));

		public Geometry Data { get => GetValue(DataProperty) as Geometry; set => SetValue(DataProperty, value); }
		#endregion Data

		#region Fill
		public static readonly DependencyProperty FillProperty = Shape.FillProperty.AddOwner(typeof(Fitting));

		public Brush Fill { get => GetValue(FillProperty) as Brush; set => SetValue(FillProperty, value); }
		#endregion Fill

		#region Stroke
		public static readonly DependencyProperty StrokeProperty = Shape.StrokeProperty.AddOwner(typeof(Fitting));
		public Brush Stroke { get => GetValue(StrokeProperty) as Brush; set => SetValue(StrokeProperty, value); }

		public static readonly DependencyProperty StrokeThicknessProperty = Shape.StrokeThicknessProperty.AddOwner(typeof(Fitting));
		public double StrokeThickness { get => (double)GetValue(StrokeThicknessProperty); set => SetValue(StrokeThicknessProperty, value); }
		#endregion Stroke

		#region Thickness
		public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(
			nameof(Thickness), typeof(double), typeof(Fitting), new PropertyMetadata(8.0));

		public double Thickness { get => (double)GetValue(ThicknessProperty); set => SetValue(ThicknessProperty, value); }
		#endregion Thickness

		static Fitting()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Fitting), new FrameworkPropertyMetadata(typeof(Fitting)));
		}
	}
}
