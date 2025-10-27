using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AeonHacs.Wpf.Behaviors;

public class SettingsWindowBehavior : Behavior<Window>
{
    protected override void OnAttached()
    {
        if (AssociatedObject != null)
        {
            base.OnAttached();
            AssociatedObject.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/AeonHacs.Wpf;component/Resources/Settings.ico"));
            AssociatedObject.Deactivated += AssociatedObject_Deactivated;
            //TODO temporary? Find some way to position window relative to element?
            //AssociatedObject.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.Deactivated -= AssociatedObject_Deactivated;
            base.OnDetaching();
        }
    }

    private void AssociatedObject_Deactivated(object sender, EventArgs e)
    {
        FocusManager.SetFocusedElement(AssociatedObject, AssociatedObject);
    }
}
