using System.Windows;

namespace AeonHacs.Wpf.Views
{
	public class CoilTrap : View
	{
		static CoilTrap()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CoilTrap), new FrameworkPropertyMetadata(typeof(CoilTrap)));
		}
	}
}
