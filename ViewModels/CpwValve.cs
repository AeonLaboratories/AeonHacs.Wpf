using AeonHacs.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels;

public class CpwValve : CpwActuator
{
    [Browsable(false)]
    public new Components.ICpwValve Component
    {
        get => base.Component as Components.ICpwValve;
        protected set => base.Component = value;
    }

    public AeonHacs.ValveState ValveState => Component.ValveState;
    public virtual int Position => Component.Position;
    public virtual bool IsOpened => Component.IsOpened;
    public virtual bool IsClosed => Component.IsClosed;
    public int OpenedValue => Component.OpenedValue;
    public int CenterValue => Component.CenterValue;
    public int ClosedValue => Component.ClosedValue;
    public bool OpenIsPositive => Component.OpenIsPositive;
    public virtual List<string> Operations => Component?.Operations;

    protected virtual Dictionary<string, Action> ServiceCommands { get; set; }
    public CpwValve()
    {
        RunHasDefault = true;
        DefineServiceCommands();
    }

    protected virtual void DefineServiceCommands()
    {
        ServiceCommands = new()
        {
            { "Close More (10)", Component.CloseABitMore },
            { "Close More (50)", Component.CloseMore },
            { "Close Less (10)", Component.CloseABitLess },
            { "Close Less (50)", Component.CloseLess },
            { "Open More (10)", Component.OpenABitMore },
            { "Open More (50)", Component.OpenMore },
            { "Open Less (10)", Component.OpenABitLess },
            { "Open Less (50)", Component.OpenLess },
        };
    }

    protected override void StartContext()
    {
        base.StartContext();
        if (CegsPreferences.EnableServiceMode && this is not RS232Valve)
        {
            foreach (var key in ServiceCommands.Keys)
                ContextStart.Add(new Context(key));
        }
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
        else if (ServiceCommands.ContainsKey(command))
            ServiceCommands[command]();
        else
            base.Run(command);
    }
}
