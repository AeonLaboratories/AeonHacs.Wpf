using System.ComponentModel;
using System.Collections.Generic;

namespace HACS.WPF.ViewModels
{
	public class Oven : Thermometer
	{
		[Browsable(false)]
		public new Components.IOven Component
		{
			get => base.Component as Components.IOven;
			protected set => base.Component = value;
		}

		#region Auto
		public double Setpoint => Component.Setpoint;
		public double TargetSetpoint { get => (Component as Components.IAuto).Config.Setpoint; set => Component.Setpoint = value; }

        #region Switch
		public Core.StopAction StopAction { get => Component.StopAction; set => Component.StopAction = value; }

		protected string TurnOnCaption { get; set; } = "Turn on";
		protected string TurnOffCaption { get; set; } = "Turn off";
		public override List<Context> Context()
		{
			var valid = new List<Context>(base.Context());
			valid.Add(new Context(Component.IsOn ? TurnOffCaption : TurnOnCaption));
			return valid;
		}
		public override void Run(string command = "")
		{
			if (command == TurnOnCaption)
				Component?.TurnOn();
			else if (command == TurnOffCaption)
				Component?.TurnOff();
			base.Run(command);
		}

		#region OnOff
		public Core.OnOffState OnOffState => Component.OnOffState;
		public bool IsOn => Component.IsOn;
		public long MillisecondsOn => Component.MillisecondsOn;
		public long MillisecondsOff => Component.MillisecondsOff;
		public long MillisecondsInState => Component.MillisecondsInState;
		#endregion OnOff
		#endregion Switch
		#endregion Auto

	}
}
