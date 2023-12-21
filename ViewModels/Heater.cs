using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
	public class Heater : Oven
	{
		[Browsable(false)]
		public new Components.IHeater Component
		{
			get => base.Component as Components.IHeater;
			protected set => base.Component = value;
		}
		public double PowerLevel => Component.PowerLevel;
		public double TargetPowerLevel { get => Component.Config.PowerLevel; set => Component.PowerLevel = value; }
		public bool ManualMode => Component.ManualMode;
		public bool TargetManualMode { get => Component.Config.ManualMode; set => Component.ManualMode = value; }
	}
}
