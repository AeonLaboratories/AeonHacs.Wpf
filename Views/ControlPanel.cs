using HACS.Core;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace HACS.WPF.Views
{
	public abstract class ControlPanel : ContentControl
	{
		public HacsBridge Bridge { get; set; }

		internal virtual void UILoaded(object sender, RoutedEventArgs e) => Bridge?.UILoaded();
		internal virtual void UIShown(object sender, EventArgs e) => Bridge?.UIShown();
		internal virtual void UIClosing(object sender, CancelEventArgs e) => Bridge?.UIClosing();
	}

	public abstract class ControlPanel<T> : ControlPanel where T : HacsBase, new()
	{
		public ControlPanel() { }

		public ControlPanel(Action closeAction)
		{
			Bridge = new HacsBridge<T>(!File.Exists("settings.json"));
			Bridge.CloseUI += closeAction;
			Bridge.Start();
		}
	}
}
