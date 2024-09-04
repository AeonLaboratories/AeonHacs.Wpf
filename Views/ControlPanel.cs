using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace AeonHacs.Wpf.Views
{
    public abstract class ControlPanel : ContentControl
    {
        public HacsBridge Bridge { get; } = new();

        public ControlPanel() { }

        public ControlPanel(Action closeAction)
        {
            Bridge.CloseUI += closeAction;
            Bridge.Start();
        }

        internal virtual void UILoaded(object sender, RoutedEventArgs e) => Bridge?.UILoaded();
        internal virtual void UIShown(object sender, EventArgs e) => Bridge?.UIShown();
        internal virtual void UIClosing(object sender, CancelEventArgs e) => Bridge?.UIClosing();
    }
}
