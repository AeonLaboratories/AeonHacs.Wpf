using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
	public class ManagedSwitch : Switch
	{
		[Browsable(false)]
		public new Components.IManagedSwitch Component
		{
			get => base.Component as Components.IManagedSwitch;
			protected set => base.Component = value;
		}
		public ViewModel Manager => GetFromModel(Component?.Manager);
	}
}
