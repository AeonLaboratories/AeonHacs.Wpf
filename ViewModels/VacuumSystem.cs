using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels;

public class VacuumSystem : HacsComponent
{
    public VacuumSystem() { RunHasDefault = true; }

    [Browsable(false)]
    public new Components.IVacuumSystem Component
    {
        get => base.Component as Components.IVacuumSystem;
        protected set => base.Component = value;
    }


    public ViewModel Manometer => GetFromModel(Component?.Manometer);
    public ViewModel ForelineManometer => GetFromModel(Component?.ForelineManometer);
    public ViewModel HighVacuumValve => GetFromModel(Component?.HighVacuumValve);
    public ViewModel LowVacuumValve => GetFromModel(Component?.LowVacuumValve);
    public ViewModel BackingValve => GetFromModel(Component?.BackingValve);
    public ViewModel RoughingValve => GetFromModel(Component?.RoughingValve);

    // TODO this should be a protected override, no?
    //public new bool Stopped => Component.Stopped;
    public double Pressure => Component.Pressure;
    public bool AutoManometer { get => Component.AutoManometer; set => Component.AutoManometer = value; }

    public double TargetPressure { get => Component.TargetPressure; set => Component.TargetPressure = value; }
    public double BaselinePressure { get => Component.BaselinePressure; set => Component.BaselinePressure = value; }
    public double GoodBackingPressure { get => Component.GoodBackingPressure; set => Component.GoodBackingPressure = value; }
    public double HighVacuumPreferredPressure { get => Component.HighVacuumPreferredPressure; set => Component.HighVacuumPreferredPressure = value; }
    public double HighVacuumRequiredPressure { get => Component.HighVacuumRequiredPressure; set => Component.HighVacuumRequiredPressure = value; }
    public double LowVacuumRequiredPressure { get => Component.LowVacuumRequiredPressure; set => Component.LowVacuumRequiredPressure = value; }

    public Components.VacuumSystem.StateCode State => Component.State;
    public long MillisecondsInState => Component.MillisecondsInState;
    public TimeSpan TimeAtBaseline => Component.TimeAtBaseline;

    protected string IsolateCaption { get; set; } = "Isolate";
    protected string EvacuateCaption { get; set; } = "Evacuate";
    protected string IsolateManifoldCaption { get; set; } = "Isolate Vacuum Manifold";
    protected string OpenLineCaption { get; set; } = "Open Line";
    protected string RoughCaption { get; set; } = "Rough";
    protected string StandbyCaption { get; set; } = "Standby";

    protected override void StartContext()
    {
        ContextStart.Clear();
        ContextStart.Add(new Context(IsolateCaption));
        ContextStart.Add(new Context(EvacuateCaption));
        ContextStart.Add(new Context(IsolateManifoldCaption));
        ContextStart.Add(new Context(OpenLineCaption));
        ContextStart.Add(new Context(RoughCaption));
        ContextStart.Add(new Context(StandbyCaption));
    }
    protected string AutoManometerCaption { get; set; } = "Auto Manometer";
    protected string ManualManometerCaption { get; set; } = "Manual Manometer";

    public override List<Context> Context()
    {
        var valid = new List<Context>(ContextStart);
        valid.Add(new Context(Component.AutoManometer ? ManualManometerCaption : AutoManometerCaption));
        return valid;
    }

    public override void Run(string command = "")
    {
        if (command == IsolateManifoldCaption)
            Component?.IsolateManifold();
        else if (command == StandbyCaption)
            Component?.Standby();
        else if (command == IsolateCaption)
            Component?.Isolate();
        else if (command == EvacuateCaption)
            Component?.Evacuate();
        else if (command == RoughCaption)
            Component?.Rough();
        else if (command == OpenLineCaption)
            Component?.OpenLine();
        else if (command == AutoManometerCaption && Component != null)
            Component.AutoManometer = true;
        else if (command == ManualManometerCaption && Component != null)
            Component.AutoManometer = false;
        else if (command.IsBlank())
        {
            if (State == Components.VacuumSystem.StateCode.Isolated)
                Component.Evacuate();
            else
                Component.Isolate();
        }
        else
            base.Run(command);
    }
}
