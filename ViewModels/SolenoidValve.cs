using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace AeonHacs.Wpf.ViewModels
{
    public class SolenoidValve : ManagedSwitch
    {
        public SolenoidValve() { RunHasDefault = true; }

        [Browsable(false)]
        public new Components.ISolenoidValve Component
        {
            get => base.Component as Components.ISolenoidValve;
            protected set => base.Component = value;
        }
        public AeonHacs.ValveState PoweredState { get => Component.PoweredState; set => Component.PoweredState = value; }
        public int MillisecondsToChangeState { get => Component.MillisecondsToChangeState; set => Component.MillisecondsToChangeState = value; }
        public bool InMotion => Component.InMotion;

        #region Valve

        public AeonHacs.ValveState ValveState => Component.ValveState;
        public virtual List<string> Operations => Component?.Operations;
        public virtual bool Ready => Component.Ready;
        public virtual bool Idle => Component.Idle;
        public virtual bool IsOpened => Component.IsOpened;
        public virtual bool IsClosed => Component.IsClosed;

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
                    case AeonHacs.ValveState.Closed:
                        Component?.DoOperation("Open");
                        break;
                    case AeonHacs.ValveState.Opening:
                    case AeonHacs.ValveState.Closing:
                        Component?.DoOperation("Stop");
                        break;
                    default:
                    case AeonHacs.ValveState.Opened:
                    case AeonHacs.ValveState.Unknown:
                        Component?.DoOperation("Close");
                        break;
                }
            }
            else if (Component.Operations.Contains(command))
                Component?.DoOperation(command);
            else
                base.Run(command);
        }

        #endregion Valve
    }
}
