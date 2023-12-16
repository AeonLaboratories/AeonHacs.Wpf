using System.ComponentModel;

namespace HACS.WPF.ViewModels
{
	public class HC6Heater : ManagedHeater
	{
		[Browsable(false)]
		public new Components.IHC6Heater Component
		{
			get => base.Component as Components.IHC6Heater;
			protected set => base.Component = value;
		}
	}
}
