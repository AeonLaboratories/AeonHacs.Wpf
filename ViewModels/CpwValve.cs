using AeonHacs.Components;
using System.Collections.Generic;
using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels;

public class CpwValve : CpwActuator
{
    public CpwValve() { RunHasDefault = true; }

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

    #region Integration

    static string CloseMore10Operation = "Close More (10)";
    static string CloseMore50Operation = "Close More (50)";
    static string CloseLess10Operation = "Close Less (10)";
    static string CloseLess50Operation = "Close Less (50)";

    static string OpenMore10Operation = "Open More (10)";
    static string OpenMore50Operation = "Open More (50)";
    static string OpenLess10Operation = "Open Less (10)";
    static string OpenLess50Operation = "Open Less (50)";

    protected override void StartContext()
    {
        base.StartContext();
        //ContextStart.Add(new Context(CloseMore10Operation));
        //ContextStart.Add(new Context(CloseMore50Operation));
        //ContextStart.Add(new Context(CloseLess10Operation));
        //ContextStart.Add(new Context(CloseLess50Operation));
        //ContextStart.Add(new Context(OpenMore10Operation));
        //ContextStart.Add(new Context(OpenMore50Operation));
        //ContextStart.Add(new Context(OpenLess10Operation));
        //ContextStart.Add(new Context(OpenLess50Operation));
    }

    protected virtual bool Calibrate(string command)
    {
        if (command == CloseMore10Operation)
            Component.CloseABitMore();
        else if (command == CloseMore50Operation)
            Component.CloseMore();
        else if (command == CloseLess10Operation)
            Component.CloseABitLess();
        else if (command == CloseLess50Operation)
            Component.CloseLess();
        else if (command == OpenMore10Operation)
            Component.OpenABitMore();
        else if (command == OpenMore50Operation)
            Component.OpenMore();
        else if (command == OpenLess10Operation)
            Component.OpenABitLess();
        else if (command == OpenLess50Operation)
            Component.OpenLess();
        else
            return false;
        return true;
    }

    #endregion Integration

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
        else if (!Calibrate(command))
            base.Run(command);
    }
}
