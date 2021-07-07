using System.Windows;

namespace HACS.WPF.Views
{
	public class CoilTrap : View
	{
		static CoilTrap()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CoilTrap), new FrameworkPropertyMetadata(typeof(CoilTrap)));
		}
	}
}
