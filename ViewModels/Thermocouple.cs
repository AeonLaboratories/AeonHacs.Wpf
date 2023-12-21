using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
	public class Thermocouple : Thermometer
	{
		[Browsable(false)]
		public new Components.IThermocouple Component
		{
			get => base.Component as Components.IThermocouple;
			protected set => base.Component = value;
		}
		public AeonHacs.ThermocoupleType Type { get => Component.Type; set => Component.Type = value; }
	}
}
