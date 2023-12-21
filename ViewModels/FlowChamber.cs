using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
	public class FlowChamber : Chamber
	{
		[Browsable(false)]
		public new Components.IFlowChamber Component
		{
			get => base.Component as Components.IFlowChamber;
			protected set => base.Component = value;
		}

		public ViewModel FlowManager => GetFromModel(Component?.FlowManager);
		public ViewModel FlowValve => GetFromModel(Component?.FlowValve);
	}
}
