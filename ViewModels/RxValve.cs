using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels;

public class RxValve : Valve
{
    [Browsable(false)]
    public new Components.IRxValve Component
    {
        get => base.Component as Components.IRxValve;
        protected set => base.Component = value;
    }
    public int Movement => Component.Movement;
    public int CommandedMovement => Component.CommandedMovement;
    public int ConsecutiveMatches { get => Component.ConsecutiveMatches; set => Component.ConsecutiveMatches = value; }
    public bool EnoughMatches => Component.EnoughMatches;
    public int MinimumPosition { get => Component.MinimumPosition; set => Component.MinimumPosition = value; }
    public int MaximumPosition { get => Component.MaximumPosition; set => Component.MaximumPosition = value; }
    public int PositionsPerTurn { get => Component.PositionsPerTurn; set => Component.PositionsPerTurn = value; }
}
