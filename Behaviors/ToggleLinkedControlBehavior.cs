using AeonHacs.Wpf.Views;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;

namespace AeonHacs.Wpf.Behaviors
{
    public class ToggleLinkedControlBehavior : Behavior<View>
	{
		[ConstructorArgument("linkedControls")]
		protected List<UIElement> LinkedControls { get; }

		public ToggleLinkedControlBehavior(params UIElement[] linkedControls) =>
			LinkedControls = linkedControls.ToList();

		protected override void OnAttached()
		{
			if (AssociatedObject != null)
			{
				base.OnAttached();
				AssociatedObject.Click += AssociatedObject_Click; ;
			}
		}

		private void AssociatedObject_Click(object sender, RoutedEventArgs e)
		{
			if (LinkedControls != null)
			{
				LinkedControls.ForEach(control =>
				{
					if (control.Visibility == Visibility.Visible)
						control.Visibility = Visibility.Collapsed;
					else
						control.Visibility = Visibility.Visible;
				});
			}
		}
	}
}
