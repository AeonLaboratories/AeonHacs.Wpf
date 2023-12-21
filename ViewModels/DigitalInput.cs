using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
	public class DigitalInput : OnOff
	{
		[Browsable(false)]
		public new Components.IDigitalInput Component
		{
			get => base.Component as Components.IDigitalInput;
			protected set => base.Component = value;
		}
		public ViewModel Manager => GetFromModel(Component?.Manager);
	}
}
