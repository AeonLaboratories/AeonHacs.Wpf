using System.ComponentModel;
using System.Windows;

namespace AeonHacs.Wpf
{
	public static class DesignTimeProperties
	{
		private static DependencyObject obj = new DependencyObject();

		public static bool IsDesignTime => DesignerProperties.GetIsInDesignMode(obj);
	}
}
