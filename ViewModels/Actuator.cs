using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels;
public class Actuator : ManagedDevice
{
    [Browsable(false)]
    public new Components.IActuator Component
    {
        get => base.Component as Components.IActuator;
        protected set => base.Component = value;
    }
    public Components.IActuatorOperation Operation => Component.Operation;
    public bool Ready => Component.Ready;
    public int PendingOperations => Component.PendingOperations;
    public bool Idle => Component.Idle;
    public bool Active => Component.Active;
    public bool Linked => Component.Linked;
    public bool MotionInhibited => Component.MotionInhibited;
    public bool InMotion => Component.InMotion;
    public double Elapsed => Component.Elapsed;
    public double TimeLimit { get => Component.TimeLimit; set => Component.TimeLimit = value; }
    public bool TimeLimitDetected => Component.TimeLimitDetected;
    public bool StopRequested => Component.StopRequested;
    new public bool Stopped => Component.Stopped;
    public bool ActionSucceeded => Component.ActionSucceeded;

    public AeonHacs.ObservableItemsCollection<Components.ActuatorOperation> ActuatorOperations
    {
        get => Component.ActuatorOperations;
        set => Component.ActuatorOperations = value;
    }

    protected override void StartContext()
    {
        ContextStart.Clear();
        ContextStart.Add(new Context("Stop"));
        Component.Operations.ForEach(operation =>
        {
            ContextStart.Add(new Context(operation));
        });
    }

    public override void Run(string command)
    {
        Component?.DoOperation(command);
    }
}
