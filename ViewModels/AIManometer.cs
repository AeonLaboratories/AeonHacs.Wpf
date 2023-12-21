using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
	public class AIManometer : AIVoltmeter
	{
		[Browsable(false)]
		public new Components.IAIManometer Component
		{
			get => base.Component as Components.IAIManometer;
			protected set => base.Component = value;
		}
		public double Pressure => Component.Pressure;
		public double MaxPressure { get => Component.MaxPressure; set => Component.MaxPressure = value; }
	}
}
