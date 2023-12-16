using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class HC6Thermocouple : ManagedThermocouple
	{
		[Browsable(false)]
		public new Components.IHC6Thermocouple Component
		{
			get => base.Component as Components.IHC6Thermocouple;
			protected set => base.Component = value;
		}
		public Components.HC6ErrorCodes Errors => Component.Errors;
	}
}
