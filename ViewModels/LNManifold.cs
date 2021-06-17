using System.ComponentModel;
using System.Collections.Generic;

namespace HACS.WPF.ViewModels
{
	public class LNManifold : StateManager
	{
		[Browsable(false)]
		public new Components.ILNManifold Component
		{
			get => base.Component as Components.ILNManifold;
			protected set => base.Component = value;
		}

		public ViewModel LNSupplyValve => GetFromModel(Component?.LNSupplyValve);
		public ViewModel Liters => GetFromModel(Component?.Liters);
		public ViewModel LevelSensor => GetFromModel(Component?.LevelSensor);
		public ViewModel OverflowSensor => GetFromModel(Component?.OverflowSensor);

		public int OverflowTrigger { get => Component.OverflowTrigger; set => Component.OverflowTrigger = value; }
		public int MinimumLiters { get => Component.MinimumLiters; set => Component.MinimumLiters = value; }
		public int TargetTemperature { get => Component.TargetTemperature; set => Component.TargetTemperature = value; }
		public int FillTrigger { get => Component.FillTrigger; set => Component.FillTrigger = value; }
		public int SecondsSlowToFill { get => Component.SecondsSlowToFill; set => Component.SecondsSlowToFill = value; }
		public int ColdTemperature { get => Component.ColdTemperature; set => Component.ColdTemperature = value; }
		public bool IsCold => Component.IsCold;
		public bool StayingActive => Component.StayingActive;
		public bool OverflowIsDetected => Component.OverflowIsDetected;
		public int SecondsFilling => Component.SecondsFilling;
		public bool IsSlowToFill => Component.IsSlowToFill;
		public bool SupplyEmpty => Component.SupplyEmpty;

		protected string MonitorCaption { get; set; } = "Monitor";
		protected string StayActiveCaption { get; set; } = "Stay Active";
		protected string ForceFillCaption { get; set; } = "Force fill";
		public override List<Context> Context()
		{
			var valid = new List<Context>(base.Context());
			valid.Add(new Context(Component.StayingActive ? MonitorCaption : StayActiveCaption));
			valid.Add(new Context(ForceFillCaption));
			return valid;
		}
		public override void Run(string command = "")
		{
			if (command == MonitorCaption)
				Component?.Monitor();
			else if (command == StayActiveCaption)
				Component?.StayActive();
			else if (command == ForceFillCaption)
				Component?.ForceFill();
			base.Run(command);
		}

	}
}
