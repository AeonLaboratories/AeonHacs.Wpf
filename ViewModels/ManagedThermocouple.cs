using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
	public class ManagedThermocouple : Thermocouple
	{
		[Browsable(false)]
		public new Components.IManagedThermocouple Component
		{
			get => base.Component as Components.IManagedThermocouple;
			protected set => base.Component = value;
		}
		public ViewModel Manager => GetFromModel(Component?.Manager);
	}
}
