using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
	public class TubeFurnace : Oven
	{
		[Browsable(false)]
		public new Components.ITubeFurnace Component
		{
			get => base.Component as Components.ITubeFurnace;
			protected set => base.Component = value;
		}
		public ViewModel SerialController => GetFromModel(Component.SerialController);

		public double TimeLimit { get => Component.TimeLimit; set => Component.TimeLimit = value; }
		public bool UseTimeLimit { get => Component.UseTimeLimit; set => Component.UseTimeLimit = value; }
		public double RampingSetpoint => Component.RampingSetpoint;
		public double MinutesInState => Component.MinutesInState;
		public double MinutesOn => Component.MinutesOn;
		public double MinutesOff => Component.MinutesOff;
		public bool Ready => Component.Ready;


		// TODO context menu option?
		//void TurnOn(double setpoint, double minutes);


	}
}
