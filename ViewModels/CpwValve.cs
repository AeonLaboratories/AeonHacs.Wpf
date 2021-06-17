using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace HACS.WPF.ViewModels
{
	public class CpwValve : CpwActuator
	{
		[Browsable(false)]
		public new Components.ICpwValve Component
		{
			get => base.Component as Components.ICpwValve;
			protected set => base.Component = value;
		}

		public Core.ValveState ValveState => Component.ValveState;
		public virtual int Position => Component.Position;
		public virtual bool IsOpened => Component.IsOpened;
		public virtual bool IsClosed => Component.IsClosed;
		public int OpenedValue => Component.OpenedValue;
		public int CenterValue => Component.CenterValue;
		public int ClosedValue => Component.ClosedValue;
		public bool OpenIsPositive => Component.OpenIsPositive;
		public Core.ValveState LastMotion => Component.LastMotion;
		public virtual List<string> Operations => Component?.Operations;


		public override void Run(string command)
		{
			if (command.IsBlank())
			{
				switch (ValveState)
				{
					case Core.ValveState.Closed:
						Component?.DoOperation("Open");
						break;
					case Core.ValveState.Opening:
					case Core.ValveState.Closing:
						Component?.DoOperation("Stop");
						break;
					default:
					case Core.ValveState.Opened:
					case Core.ValveState.Unknown:
						Component?.DoOperation("Close");
						break;
				}
			}
			else
				base.Run(command);
		}
	}
}
