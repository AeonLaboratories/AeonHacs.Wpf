using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class DigitalOutput : ManagedSwitch
	{
		[Browsable(false)]
		public new Components.IDigitalOutput Component
		{
			get => base.Component as Components.IDigitalOutput;
			protected set => base.Component = value;
		}
	}
}
