using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AeonHacs.Wpf.Behaviors
{
    public class SwapVisibilityBehavior : Behavior<UIElement>
    {
        protected UIElement ToggleWith { get; set; }

        protected UIElement FocusElement { get; set; }

        public SwapVisibilityBehavior(UIElement toggleWith)
        {
            ToggleWith = toggleWith;
            if (ToggleWith is MenuItem menuItem)
                menuItem.Click += (sender, e) => e.Handled = SwapVisiblity();
            else if (ToggleWith is Control control)
                control.MouseDoubleClick += (sender, e) => e.Handled = SwapVisiblity();
        }

        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                base.OnAttached();
                if (AssociatedObject is MenuItem menuItem && menuItem.Header is TextBox tb)
                    FocusElement = tb;
                else
                    FocusElement = AssociatedObject;
                AssociatedObject.PreviewLostKeyboardFocus += AssociatedObject_PreviewLostKeyboardFocus;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.PreviewLostKeyboardFocus -= AssociatedObject_PreviewLostKeyboardFocus;
                base.OnDetaching();
            }
        }

        protected bool SwapVisiblity()
        {
            if (AssociatedObject != null && ToggleWith != null)
            {
                if (ToggleWith.Visibility == Visibility.Visible)
                {
                    if (FocusManager.GetFocusedElement(FocusManager.GetFocusScope(AssociatedObject)) is DependencyObject element && Validation.GetErrors(element).Count > 0)
                        return false;

                    ToggleWith.Visibility = Visibility.Collapsed;
                    AssociatedObject.Visibility = Visibility.Visible;
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(FocusElement), FocusElement);
                    if (FocusElement is TextBox tb)
                        tb.SelectAll();
                    return true;
                }
                else
                {
                    AssociatedObject.Visibility = Visibility.Collapsed;
                    ToggleWith.Visibility = Visibility.Visible;
                }
            }
            return false;
        }

        private void AssociatedObject_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!e.Handled)
                SwapVisiblity();
        }
    }
}
