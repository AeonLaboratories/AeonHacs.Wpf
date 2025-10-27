using System.ComponentModel;
using System.Collections.Generic;
using AeonHacs;
using System;

namespace AeonHacs.Wpf.ViewModels;

public class Switch : OnOff
{
    public Switch() { RunHasDefault = true; }

    [Browsable(false)]
    public new Components.ISwitch Component
    {
        get => base.Component as Components.ISwitch;
        protected set => base.Component = value;
    }

    public AeonHacs.SwitchState State { get => Component.State; set => Component.State = value; }
    public AeonHacs.StopAction StopAction { get => Component.StopAction; set => Component.StopAction = value; }
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
        if (command.IsBlank())
            Component?.TurnOnOff(Component.Config.State.IsOff());
        else if (command == TurnOnCaption)
            Component?.TurnOn();
        else if (command == TurnOffCaption)
            Component?.TurnOff();
    }
}
