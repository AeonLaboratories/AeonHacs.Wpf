using System.ComponentModel;

namespace HACS.WPF.ViewModels
{
	public class HC6HeaterB2 : ManagedHeater
	{
		[Browsable(false)]
		public new Components.IHC6HeaterB2 Component
		{
			get => base.Component as Components.IHC6HeaterB2;
			protected set => base.Component = value;
		}
	}
}
