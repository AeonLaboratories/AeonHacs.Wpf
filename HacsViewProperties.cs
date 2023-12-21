using AeonHacs.Wpf.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AeonHacs.Wpf
{
    public static class HacsViewProperties
	{
		#region Elliptical

		public static readonly DependencyProperty EllipticalProperty = DependencyProperty.RegisterAttached(
			"Elliptical", typeof(bool), typeof(HacsViewProperties), new FrameworkPropertyMetadata(false));

		public static void SetElliptical(View view, bool elliptical) =>
			view.SetValue(EllipticalProperty, elliptical);

		public static bool GetElliptical(View view) =>
			view.GetValue(EllipticalProperty) is bool value ? value : false;

		#endregion Elliptical

		#region BackgroundResourceKey

		public static readonly DependencyProperty BackgroundResourceKeyProperty = DependencyProperty.RegisterAttached(
			"BackgroundResourceKey", typeof(string), typeof(HacsViewProperties), new FrameworkPropertyMetadata(null, BackgroundResourceKeyChanged));

		public static void SetBackgroundResourceKey(FrameworkElement control, string key) =>
			control.SetValue(BackgroundResourceKeyProperty, key);

		public static string GetBackgroundResourceKey(FrameworkElement control) =>
			(string)control.GetValue(BackgroundResourceKeyProperty);

		static void BackgroundResourceKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is FrameworkElement c)
			{
				if (e.NewValue is string key && !key.IsBlank())
					c.SetResourceReference(Panel.BackgroundProperty, key);
				else
					c.SetResourceReference(Panel.BackgroundProperty, null);
			}
		}

		#endregion BackgroundResourceKey
	}
}
