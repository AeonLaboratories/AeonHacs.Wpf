using System.Windows;
using System.Windows.Controls;

namespace AeonHacs.Wpf.Views
{
	/// <summary>
	/// Interaction logic for CC.xaml
	/// </summary>
	public partial class CC : View
	{
		#region Orientation
		public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(
			typeof(CC));

		public Orientation Orientation { get => (Orientation)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }
		#endregion Orientation

		public CC()
		{
			InitializeComponent();
		}
	}
}
