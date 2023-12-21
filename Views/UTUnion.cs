using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Views
{
	public class UTUnion : Control
	{
		#region Orientation
		public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(
			typeof(UTUnion));

		public Orientation Orientation { get => (Orientation)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }
		#endregion Orientation

		static UTUnion()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(UTUnion), new FrameworkPropertyMetadata(typeof(UTUnion)));
		}
	}
}
