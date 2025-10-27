using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels;

public class GraphiteReactor : Port
{
    [Browsable(false)]
    public new Components.IGraphiteReactor Component
    {
        get => base.Component as Components.IGraphiteReactor;
        protected set => base.Component = value;
    }

    public Components.GraphiteReactor.States State { get => Component.State; set => Component.State = value; }

    public Components.GraphiteReactor.Sizes Size { get => Component.Size; set => Component.Size = value; }

    public ViewModel Sample => GetFromModel(Component?.Sample);
    public ViewModel Aliquot => GetFromModel(Component?.Aliquot);

    public int GraphitizingTemperature { get => Component.GraphitizingTemperature; set => Component.GraphitizingTemperature = value; }
    public Utilities.Stopwatch StateStopwatch => Component.StateStopwatch;
    public Utilities.Stopwatch ProgressStopwatch => Component.ProgressStopwatch;
    public double PriorPressure { get => Component.PriorPressure; set => Component.PriorPressure = value; }
    public int PressurePeak { get => Component.PressurePeak; set => Component.PressurePeak = value; }
    public string Contents => Component.Contents;
    public bool Busy => Component.Busy;
    public bool Ready => Component.Prepared;
    public double HeaterTemperature => Component.HeaterTemperature;
    public int SampleTemperatureOffset { get => Component.SampleTemperatureOffset; set => Component.SampleTemperatureOffset = value; }
    public double SampleTemperature => Component.SampleTemperature;
    public double ColdfingerTemperature => Component.ColdfingerTemperature;
    public bool FurnaceUnresponsive => Component.FurnaceUnresponsive;
    public bool ReactionNotStarting => Component.ReactionNotStarting;
    public bool ReactionNotFinishing => Component.ReactionNotFinishing;

    // TODO Graphite reactor context menu -- allow to force state?

    protected string StartCaption { get; set; } = "Start";
    protected string StopCaption { get; set; } = "Stop";
    protected string ServiceCompleteCaption { get; set; } = "ServiceComplete";
    protected string PreparationCompleteCaption { get; set; } = "PreparationComplete";
    protected string EmptyCaption { get; set; } = "Empty";

    protected override void StartContext()
    {
        ContextStart.Clear();
        ContextStart.Add(new Context(StartCaption));
        ContextStart.Add(new Context(StopCaption));
        ContextStart.Add(new Context(ServiceCompleteCaption));
        ContextStart.Add(new Context(PreparationCompleteCaption));
        ContextStart.Add(new Context(EmptyCaption));
    }

    public override void Run(string command = "")
    {
        if (command == StartCaption)
            Component?.Start();
        else if (command == StopCaption)
            Component?.Stop();
        else if (command == ServiceCompleteCaption)
            Component?.ServiceComplete();
        else if (command == PreparationCompleteCaption)
            Component?.PreparationComplete();
        else if (command == EmptyCaption)
            Component.Aliquot = null;
        else
            base.Run(command);
    }
}
