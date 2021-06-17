using HACS.WPF.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace HACS.WPF.Views
{
    public class Heater : View
	{
		[Category("Appearance")]
		public bool Elliptical { get => (bool)GetValue(HacsViewProperties.EllipticalProperty); set => SetValue(HacsViewProperties.EllipticalProperty, value); }

		static Heater()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Heater), new FrameworkPropertyMetadata(typeof(Heater)));
		}

		protected override void OnComponentTypeChanged()
		{
			base.OnComponentTypeChanged();
			AttachBackgroundResource();
		}

		protected void AttachBackgroundResource()
		{
			var backgroundResourceKeyBinding = new Binding(nameof(ViewModels.Heater.IsOn)) { Source = Component };
			backgroundResourceKeyBinding.Converter = OnOffResourceKeyConverter.Heater;
			backgroundResourceKeyBinding.FallbackValue = "NeutralBrush";
			SetBinding(HacsViewProperties.BackgroundResourceKeyProperty, backgroundResourceKeyBinding);
		}
	}
}
