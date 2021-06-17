using HACS.WPF.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HACS.WPF
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

		public static void SetBackgroundResourceKey(Control control, string key) =>
			control.SetValue(BackgroundResourceKeyProperty, key);

		public static string GetBackgroundResourceKey(Control control) =>
			(string)control.GetValue(BackgroundResourceKeyProperty);

		static void BackgroundResourceKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is Control c)
			{
				if (e.NewValue is string key && !key.IsBlank())
					c.SetResourceReference(Control.BackgroundProperty, key);
				else
					c.SetResourceReference(Control.BackgroundProperty, null);
			}
		}

		#endregion BackgroundResourceKey

	}
}
