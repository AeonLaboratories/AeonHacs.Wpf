using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class ManagedValve : Valve
	{
		[Browsable(false)]
		public new Components.IManagedValve Component
		{
			get => base.Component as Components.IManagedValve;
			protected set => base.Component = value;
		}
		public ViewModel Manager => GetFromModel(Component?.Manager);
	}
}
