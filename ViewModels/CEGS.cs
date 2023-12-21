using System;
using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
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
		public ViewModel CegsPreferences => GetFromModel(Component?.CegsPreferences);
		public ViewModel Sample => GetFromModel(Component?.Sample);
		public bool AutoFeedEnabled => Component?.AutoFeedEnabled ?? false;

        // TODO remaining implementation deferred
    }
}
