using System;
using System.ComponentModel;

namespace HACS.WPF.ViewModels
{
	public class CEGS : ProcessManager
	{
		[Browsable(false)]
		public new Components.ICegs Component
		{
			get => base.Component as Components.ICegs;
			protected set => base.Component = value;
		}

		public TimeSpan Uptime => Component.Uptime;
		public ViewModel Preferences => GetFromModel(Component?.Preferences);
		public ViewModel Sample => GetFromModel(Component?.Sample);

		// TODO remaining implementation deferred
	}
}
