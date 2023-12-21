using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
	public class FlowManager : HacsComponent
	{
		[Browsable(false)]
		public new Components.IFlowManager Component
		{
			get => base.Component as Components.IFlowManager;
			protected set => base.Component = value;
		}

		public ViewModel FlowValve => GetFromModel(Component?.FlowValve);
		public ViewModel Meter => GetFromModel(Component?.Meter);

		public int MillisecondsTimeout { get => Component.MillisecondsTimeout; set => Component.MillisecondsTimeout = value; }
		public double SecondsCycle { get => Component.SecondsCycle; set => Component.SecondsCycle = value; }
		public int StartingMovement { get => Component.StartingMovement; set => Component.StartingMovement = value; }
		public int MaximumMovement { get => Component.MaximumMovement; set => Component.MaximumMovement = value; }
		public double Lag { get => Component.Lag; set => Component.Lag = value; }
		public double Deadband { get => Component.Deadband; set => Component.Deadband = value; }
		public bool DeadbandIsFractionOfTarget { get => Component.DeadbandIsFractionOfTarget; set => Component.DeadbandIsFractionOfTarget = value; }
		public double Gain { get => Component.Gain; set => Component.Gain = value; }
		public bool DivideGainByDeadband { get => Component.DivideGainByDeadband; set => Component.DivideGainByDeadband = value; }
		public bool StopOnFullyOpened { get => Component.StopOnFullyOpened; set => Component.StopOnFullyOpened = value; }
		public bool StopOnFullyClosed { get => Component.StopOnFullyClosed; set => Component.StopOnFullyClosed = value; }
		public double TargetValue { get => Component.TargetValue; set => Component.TargetValue = value; }
		public bool UseRateOfChange { get => Component.UseRateOfChange; set => Component.UseRateOfChange = value; }
		public double MaximumRate { get => Component.MaximumRate; set => Component.MaximumRate = value; }
		public bool Busy => Component.Busy;
	}
}
