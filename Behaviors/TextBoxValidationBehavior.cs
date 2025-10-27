using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AeonHacs.Wpf.Behaviors;

public class TextBoxValidationBehavior : Behavior<TextBox>
{
    protected override void OnAttached()
    {
        if (AssociatedObject != null)
        {
            base.OnAttached();
            Validation.AddErrorHandler(AssociatedObject, HandleValidationError);
            AssociatedObject.PreviewLostKeyboardFocus += AssociatedObject_PreviewLostKeyboardFocus;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.PreviewLostKeyboardFocus -= AssociatedObject_PreviewLostKeyboardFocus;
            Validation.RemoveErrorHandler(AssociatedObject, HandleValidationError);
            base.OnDetaching();
        }
    }

    protected virtual void HandleValidationError(object sender, ValidationErrorEventArgs e)
    {
        if (sender is DependencyObject d)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                ToolTipService.SetToolTip(d, e.Error.ErrorContent);
            else if (Validation.GetErrors(d).Count < 1)
                ToolTipService.SetToolTip(d, null);
        }
    }

    private void AssociatedObject_PreviewLostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
    {
        if (Validation.GetErrors(AssociatedObject).Count > 0)
        {
            // Try to maintain focus.
            e.Handled = true;
            AssociatedObject.Focus();
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(AssociatedObject), AssociatedObject);
            AssociatedObject.SelectAll();
        }
    }
}
