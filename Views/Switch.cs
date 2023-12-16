using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows;
using HACS.Components;
using HACS.WPF.Converters;

namespace HACS.WPF.Views
{
    public class Switch : View
	{
		static Switch()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Switch), new FrameworkPropertyMetadata(typeof(Switch)));
		}

		protected override void OnComponentTypeChanged()
		{
			base.OnComponentTypeChanged();
			AttachBackgroundResource();
		}

		protected void AttachBackgroundResource()
		{
			var backgroundResourceKeyBinding = new Binding(nameof(ViewModels.Switch.IsOn)) { Source = Component };
			backgroundResourceKeyBinding.Converter = BoolToResourceKeyConverter.Default;
			backgroundResourceKeyBinding.FallbackValue = "OffBrush";
			SetBinding(HacsViewProperties.BackgroundResourceKeyProperty, backgroundResourceKeyBinding);
		}
	}
}
