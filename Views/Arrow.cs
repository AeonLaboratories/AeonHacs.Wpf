using AeonHacs.Wpf.Media;
using System.Windows;
using System.Windows.Controls;

namespace AeonHacs.Wpf.Views
{
    public class Arrow : Control
	{
		#region Direction
		public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
			nameof(Direction), typeof(Direction), typeof(Arrow), new PropertyMetadata(Direction.Down));

		public Direction Direction { get => (Direction)GetValue(DirectionProperty); set => SetValue(DirectionProperty, value); }
		#endregion Direction

		static Arrow()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Arrow), new FrameworkPropertyMetadata(typeof(Arrow)));
		}

	}
}
