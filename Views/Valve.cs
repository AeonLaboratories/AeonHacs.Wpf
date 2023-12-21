using AeonHacs.Wpf.Converters;
using System.Windows;
using System.Windows.Data;

namespace AeonHacs.Wpf.Views
{
    public class Valve : View
	{
		static Valve()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Valve), new FrameworkPropertyMetadata(typeof(Valve)));
		}

		protected override void OnComponentTypeChanged()
		{
			base.OnComponentTypeChanged();
			AttachBackgroundResource();
		}

		protected void AttachBackgroundResource()
		{
			var backgroundResourceKeyBinding = new Binding(nameof(ViewModels.Valve.ValveState)) { Source = Component };
			backgroundResourceKeyBinding.Converter = ValveStateResourceKeyConverter.Default;
			backgroundResourceKeyBinding.FallbackValue = "UnknownBrush";
			SetBinding(HacsViewProperties.BackgroundResourceKeyProperty, backgroundResourceKeyBinding);
		}
	}
}
