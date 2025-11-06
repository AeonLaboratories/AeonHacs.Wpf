using AeonHacs.Components;
using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels;

public class RS232Valve : CpwValve
{
    [Browsable(false)]
    public new Components.IRS232Valve Component
    {
        get => base.Component as Components.IRS232Valve;
        protected set => base.Component = value;
    }
    public bool WaitingForGo => Component.WaitingForGo;
    public bool ControllerStopped => Component.ControllerStopped;
    public bool ActuatorStopped => Component.ActuatorStopped;
    public int Movement => Component.Movement;
    public int CommandedMovement => Component.CommandedMovement;
    public int ConsecutiveMatches => Component.ConsecutiveMatches;
    public int PositionsPerTurn { get => Component.PositionsPerTurn; set => Component.PositionsPerTurn = value; }
    public int MinimumPosition { get => Component.MinimumPosition; set => Component.MinimumPosition = value; }
    public int MaximumPosition { get => Component.MaximumPosition; set => Component.MaximumPosition = value; }
    public int ClosedOffset { get => Component.ClosedOffset; set => Component.ClosedOffset = value; }
    public bool Calibrated { get => Component.Calibrated; }
    public int ControlOutput { get => Component.ControlOutput; }
    public long RS232UpdatesReceived { get => Component.RS232UpdatesReceived; }

    protected override void DefineServiceCommands()
    {
        ServiceCommands = new()
        {
            { "Close 1/16 turn (6)", () => Component.MoveBy(6) },
            { "Close 1/8 turn (12)", () => Component.MoveBy(12) },
            { "Close 1/4 turn (24)", () => Component.MoveBy(24) },
            { "Close 1/2 turn (48)", () => Component.MoveBy(48) },
            { "Open 1/16 turn (-6)", () => Component.MoveBy(-6) },
            { "Open 1/8 turn (-12)", () => Component.MoveBy(-12) },
            { "Open 1/4 turn (-24)", () => Component.MoveBy(-24) },
            { "Open 1/2 turn (-48)", () => Component.MoveBy(-48) },
        };
    }

    protected const string CalibrateCaption = "Calibrate";

    protected override void StartContext()
    {
        base.StartContext();
        if (CegsPreferences.EnableServiceMode)
        {
            foreach (var key in ServiceCommands.Keys)
               ContextStart.Add(new Context(key));
        }

        if (ContextStart.Find(item => item.Label == CalibrateCaption) == null)
            ContextStart.Add(new Context(CalibrateCaption));
    }
}
