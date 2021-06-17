using System.ComponentModel;

namespace HACS.WPF.ViewModels
{
	public class Thermocouple : Thermometer
	{
		[Browsable(false)]
		public new Components.IThermocouple Component
		{
			get => base.Component as Components.IThermocouple;
			protected set => base.Component = value;
		}
		public Core.ThermocoupleType Type { get => Component.Type; set => Component.Type = value; }
	}
}
