using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class AIThermometer : AIVoltmeter
	{
		[Browsable(false)]
		public new Components.IAIThermometer Component
		{
			get => base.Component as Components.IAIThermometer;
			protected set => base.Component = value;
		}
		public double Temperature => Component.Temperature;
	}
}
