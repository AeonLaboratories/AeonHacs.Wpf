using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class AutoManual : Auto
	{
		[Browsable(false)]
		public new Components.IAutoManual Component
		{
			get => base.Component as Components.IAutoManual;
			protected set => base.Component = value;
		}
		public bool ManualMode => Component.ManualMode;
		public bool TargetManualMode { get => Component.Config.ManualMode; set => Component.ManualMode = value; }
		public double PowerLevel => Component.PowerLevel;
		public double TargetPowerLevel { get => Component.Config.PowerLevel; set => Component.PowerLevel = value; }
		public double MaximumPowerLevel => Component.MaximumPowerLevel;
		public double TargetMaximumPowerLevel { get => Component.Config.MaximumPowerLevel; set => Component.MaximumPowerLevel = value; }

		protected string AutoCaption { get; set; } = "Auto";
		protected string ManualCaption { get; set; } = "Manual";
		protected string HoldCaption { get; set; } = "Caption";
		public override void Run(string command = "")
		{
			if (command == AutoCaption)
				Component?.Auto();
			else if (command == ManualCaption)
				Component?.Manual();
			else if (command == HoldCaption)
				Component?.Hold();
			base.Run(command);
		}
	}
}
