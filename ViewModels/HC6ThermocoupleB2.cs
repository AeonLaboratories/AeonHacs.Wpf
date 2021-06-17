using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class HC6ThermocoupleB2 : ManagedThermocouple
	{
		[Browsable(false)]
		public new Components.IHC6ThermocoupleB2 Component
		{
			get => base.Component as Components.IHC6ThermocoupleB2;
			protected set => base.Component = value;
		}
		public Components.HC6ControllerB2.ErrorCodes Errors => Component.Errors;
	}
}
