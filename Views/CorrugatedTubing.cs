using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HACS.WPF.Views
{
    public class CorrugatedTubing : Control
	{
		#region Data
		public static readonly DependencyProperty DataProperty = Path.DataProperty.AddOwner(
			typeof(CorrugatedTubing));

		public Geometry Data { get => GetValue(DataProperty) as Geometry; set => SetValue(DataProperty, value); }
		#endregion Data

		static CorrugatedTubing()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CorrugatedTubing), new FrameworkPropertyMetadata(typeof(CorrugatedTubing)));
		}
	}
}
