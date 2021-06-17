using System.ComponentModel;
using System.Collections.Generic;
using HACS.Core;

namespace HACS.WPF.ViewModels
{
	public class Switch : OnOff
	{
		[Browsable(false)]
		public new Components.ISwitch Component
		{
			get => base.Component as Components.ISwitch;
			protected set => base.Component = value;
		}

		public Core.SwitchState State { get => Component.State; set => Component.State = value; }
		public Core.StopAction StopAction { get => Component.StopAction; set => Component.StopAction = value; }
		protected string TurnOnCaption { get; set; } = "Turn on";
		protected string TurnOffCaption { get; set; } = "Turn off";
		public override List<Context> Context()
		{
			var valid = new List<Context>(base.Context());
			valid.Add(new Context(Component.Config.State.IsOn() ? TurnOffCaption : TurnOnCaption));
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

	}
}
