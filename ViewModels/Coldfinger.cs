using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels;

public class Coldfinger : HacsComponent
{
    [Browsable(false)]
    public new Components.IColdfinger Component
    {
        get => base.Component as Components.IColdfinger;
        protected set => base.Component = value;
    }

    public double Temperature => Component.Temperature;

    protected string StandbyCaption { get; set; } = "Standby";
    protected string FreezeCaption { get; set; } = "Freeze";
    protected string RaiseCaption { get; set; } = "Raise";
    protected string ThawCaption { get; set; } = "Thaw";
    protected override void StartContext()
    {
        base.StartContext();
        ContextStart.Add(new Context(StandbyCaption));
        ContextStart.Add(new Context(FreezeCaption));
        ContextStart.Add(new Context(RaiseCaption));
        ContextStart.Add(new Context(ThawCaption));
    }

    public override void Run(string command = "")
    {
        if (command == StandbyCaption)
            Component?.Standby();
        else if (command == FreezeCaption)
            Component?.Freeze();
        else if (command == RaiseCaption)
            Component?.Raise();
        else if (command == ThawCaption)
            Component?.Thaw();
        else
            base.Run(command);
    }
}
