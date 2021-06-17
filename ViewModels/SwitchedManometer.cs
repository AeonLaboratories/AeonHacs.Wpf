using HACS.Core;
using System.Collections.Generic;
using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class SwitchedManometer : Manometer
	{
		[Browsable(false)]
		public new Components.ISwitchedManometer Component
		{
			get => base.Component as Components.ISwitchedManometer;
			protected set => base.Component = value;
		}
		public int MillisecondsToValid { get => Component.MillisecondsToValid; set => Component.MillisecondsToValid = value; }
		public int MinimumMillisecondsOff { get => Component.MinimumMillisecondsOff; set => Component.MinimumMillisecondsOff = value; }
		public bool Valid => Component.Valid;

		protected string TurnOnCaption { get; set; } = "Turn on";
		protected string TurnOffCaption { get; set; } = "Turn off";
		public override List<Context> Context()
		{
			var valid = new List<Context>();
			valid.Add(new Context(Component.Config.State.IsOn() ? TurnOffCaption : TurnOnCaption));
			valid.AddRange(base.Context());
			return valid;
		}
		public override void Run(string command = "")
		{
			if (command == TurnOnCaption)
				Component?.TurnOn();
			else if (command == TurnOffCaption)
				Component?.TurnOff();
			else
				base.Run(command);
		}
	}
}
