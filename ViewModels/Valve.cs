using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace HACS.WPF.ViewModels
{
	public class Valve : HacsDevice
	{
		public Valve() { RunHasDefault = true; }

		[Browsable(false)]
		public new Components.IValve Component
		{
			get => base.Component as Components.IValve;
			protected set => base.Component = value;
		}
		public Core.ValveState ValveState => Component.ValveState;
		public virtual int Position => Component.Position;
		public virtual bool IsOpened => Component.IsOpened;
		public virtual bool IsClosed => Component.IsClosed;
		public virtual bool Idle => Component.Idle;
		public virtual bool Ready => Component.Ready;
		public virtual List<string> Operations => Component?.Operations;

		protected override void StartContext()
		{
			ContextStart.Clear();
			Component.Operations.ForEach(operation =>
			{
				ContextStart.Add(new Context(operation));
			});
		}

		public override void Run(string command)
		{
			if (command.IsBlank())
			{
				switch (ValveState)
				{
					case Core.ValveState.Closed:
						Component?.OpenWait();
						break;
					case Core.ValveState.Opening:
					case Core.ValveState.Closing:
						Component?.Stop();
						break;
					default:
					case Core.ValveState.Opened:
					case Core.ValveState.Unknown:
						Component?.CloseWait();
						break;
				}
			}
			else
				Component?.DoOperation(command);
		}
	}
}
