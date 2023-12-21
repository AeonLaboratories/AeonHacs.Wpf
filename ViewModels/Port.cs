using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
	public class Port : Chamber
	{
		[Browsable(false)]
		public new Components.IPort Component
		{
			get => base.Component as Components.IPort;
			protected set => base.Component = value;
		}

		public ViewModel Valve => GetFromModel(Component?.Valve);

		public bool IsOpened => Component.IsOpened;
		public bool IsClosed => Component.IsClosed;
	}
}
