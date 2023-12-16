using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HACS.WPF.Behaviors
{
	public class TextBoxExitKeyBehavior : Behavior<TextBox>
	{
		protected string OriginalText { get; set; }

		protected override void OnAttached()
		{
			if (AssociatedObject != null)
			{
				base.OnAttached();
				AssociatedObject.GotFocus += AssociatedObject_GotFocus;
				AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
			}
		}

		protected override void OnDetaching()
		{
			if (AssociatedObject != null)
			{
				AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
				AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
				base.OnDetaching();
			}
		}

		private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
		{
			if (Validation.GetErrors(AssociatedObject).Count < 1)
				OriginalText = (sender as TextBox).Text;
		}

		private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				AssociatedObject.Text = OriginalText;
				var scope = FocusManager.GetFocusScope(AssociatedObject);
				FocusManager.SetFocusedElement(scope, (IInputElement)scope);
				e.Handled = true;
			}
			else if (e.Key == Key.Enter || e.Key == Key.Tab)
			{
				/*
				AssociatedObject.GetBindingExpression(TextBox.TextProperty).ValidateWithoutUpdate();
				if (Interaction.GetBehaviors(AssociatedObject).OfType<TextBoxValidationBehavior>().Count() < 1 && Validation.GetErrors(AssociatedObject).Count > 0)
					AssociatedObject.Text = OriginalText;
				*/

				var scope = FocusManager.GetFocusScope(AssociatedObject);
				FocusManager.SetFocusedElement(scope, (IInputElement)scope);
				e.Handled = true;
			}
		}
	}
}
