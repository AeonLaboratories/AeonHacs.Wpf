using System.Collections.Generic;
using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels;

public class Valve : HacsDevice
{
    public Valve() { RunHasDefault = true; }

    [Browsable(false)]
    public new Components.IValve Component
    {
        get => base.Component as Components.IValve;
        protected set => base.Component = value;
    }
    public AeonHacs.ValveState ValveState => Component.ValveState;
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
                case AeonHacs.ValveState.Closed:
                    Component?.OpenWait();
                    break;
                case AeonHacs.ValveState.Opening:
                case AeonHacs.ValveState.Closing:
                    Component?.Stop();
                    break;
                default:
                case AeonHacs.ValveState.Opened:
                case AeonHacs.ValveState.Unknown:
                    Component?.CloseWait();
                    break;
            }
        }
        else
            Component?.DoOperation(command);
    }
}
