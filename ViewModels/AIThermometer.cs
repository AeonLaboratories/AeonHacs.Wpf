using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
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
