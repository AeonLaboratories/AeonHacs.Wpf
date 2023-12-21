using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
	public class Voltmeter : Meter
	{
		[Browsable(false)]
		public new Components.IVoltmeter Component
		{
			get => base.Component as Components.IVoltmeter;
			protected set => base.Component = value;
		}
		public double Voltage => Component.Voltage;
		public double MaximumVoltage { get => Component.MaximumVoltage; set => Component.MaximumVoltage = value; }
		public double MinimumVoltage { get => Component.MinimumVoltage; set => Component.MinimumVoltage = value; }
		public new bool OverRange => Component.OverRange;
		public new bool UnderRange => Component.OverRange;

	}
}
