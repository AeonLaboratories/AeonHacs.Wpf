using System.ComponentModel;

namespace HACS.WPF.ViewModels
{
	public class HacsComponent : ViewModel
	{
		[Browsable(false)]
		public new Core.IHacsComponent Component
		{
			get => base.Component as Core.IHacsComponent;
			protected set => base.Component = value;
		}
		public bool Connected => Component.Connected;
		public bool Initialized => Component.Initialized;
		public bool Started => Component.Started;
		public bool Stopped => Component.Stopped;
	}

}
