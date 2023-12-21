using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
	public class AnalogInput : ManagedDevice
	{
		[Browsable(false)]
		public new Components.IAnalogInput Component
		{
			get => base.Component as Components.IAnalogInput;
			protected set => base.Component = value;
		}
		AeonHacs.AnalogInputMode AnalogInputMode { get => Component.AnalogInputMode; set => Component.AnalogInputMode = value; }
		public double MaximumVoltage { get => Component.MaximumVoltage; set => Component.MaximumVoltage = value; }
		public double MinimumVoltage { get => Component.MinimumVoltage; set => Component.MinimumVoltage = value; }
		public bool OverRange => Component.OverRange;
		public bool UnderRange => Component.OverRange;
	}
}
