using System.ComponentModel;

namespace HACS.WPF.ViewModels
{
	public class DualActionValve : HacsDevice
	{
		[Browsable(false)]
		public new Components.IDualActionValve Component
		{
			get => base.Component as Components.IDualActionValve;
			protected set => base.Component = value;
		}

		public ViewModel OpenValve => GetFromModel(Component?.OpenValve);
		public ViewModel CloseValve => GetFromModel(Component?.CloseValve);
	}
}