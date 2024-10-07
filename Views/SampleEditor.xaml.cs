using AeonHacs.Components;
using AeonHacs.Wpf.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AeonHacs.Wpf.Views;

// TODO RENAME
public class FormCommands
{
    public static readonly RoutedUICommand Save = new("Save", nameof(Save), typeof(FormCommands));
    //{
    //    InputGestures = { new KeyGesture(Key.S, ModifierKeys.Control) }
    //};

    public static readonly RoutedUICommand Ok = new("Ok", nameof(Ok), typeof(FormCommands));
    //{
    //    InputGestures = { new KeyGesture(Key.Enter, ModifierKeys.Control) }
    //};

    public static readonly RoutedUICommand Close = new("Cancel", nameof(Close), typeof(FormCommands));
    //{
    //    InputGestures = { new KeyGesture(Key.Escape) }
    //};
}

/// <summary>
/// Interaction logic for SampleEditor.xaml
/// </summary>
public partial class SampleEditor : UserControl
{
    public event Action Updated;

    private static readonly DependencyPropertyKey SamplePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(Sample),
        typeof(ISample),
        typeof(SampleEditor),
        new PropertyMetadata()
    );

    private static readonly DependencyPropertyKey SampleDataPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(SampleData),
        typeof(SampleData),
        typeof(SampleEditor),
        new PropertyMetadata()
    );

    public static IEnumerable<Data.MassUnits> MassUnits { get; } = typeof(Data.MassUnits).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
        .Select(f => (Data.MassUnits)f.GetValue(null));

    public static readonly DependencyProperty ProcessTypeProperty = DependencyProperty.Register(
        nameof(ProcessType),
        typeof(string),
        typeof(SampleEditor),
        new PropertyMetadata(ProcessTypeChanged)
    );

    private static void ProcessTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SampleEditor se)
            se.FilterProcesses();
    }

    public static readonly DependencyProperty ProcessesProperty = DependencyProperty.Register(
        nameof(Processes),
        typeof(IEnumerable<string>),
        typeof(SampleEditor)
    );

    public static IEnumerable<string> ProcessTypes { get; } = Enum.GetNames<InletPortType>().Prepend("Any");

    public static Visibility Take13CVisibility { get; } =
        NamedObject.FindAll<Id13CPort>().Count > 0 ? Visibility.Visible : Visibility.Collapsed;

    public static IEnumerable<string> InletPorts { get; } = NamedObject.FindAll<IInletPort>().Select(ip => ip.Name).Prepend("None");

    protected IInletPort StartingIP { get; set; }

    public ISample Sample
    {
        get => (ISample)GetValue(SamplePropertyKey.DependencyProperty);
        protected set => SetValue(SamplePropertyKey, value);
    }

    public SampleData SampleData
    {
        get => (SampleData)GetValue(SampleDataPropertyKey.DependencyProperty);
        protected set => SetValue(SampleDataPropertyKey, value);
    }

    public string ProcessType
    {
        get => (string)GetValue(ProcessTypeProperty);
        set => SetValue(ProcessTypeProperty, value);
    }

    public IEnumerable<string> Processes
    {
        get => (IEnumerable<string>)GetValue(ProcessesProperty);
        set => SetValue(ProcessesProperty, value);
    }

    public SampleEditor(ISample sample = null)
    {
        InitializeComponent();
        InitializeCommands();

        Sample = sample;
        SampleData = new(sample);

        if (Sample?.InletPort.PortType is InletPortType processType)
            ProcessType = processType.ToString();
        else
            ProcessType = "Any";
    }

    public SampleEditor(IInletPort ip) : this(ip?.Sample)
    {
        StartingIP = ip;
        SampleData.InletPort = ip?.Name ?? "None";
        if (ProcessType == "Any")
            ProcessType = ip?.PortType.ToString() ?? "Any";
    }

    protected virtual void InitializeCommands()
    {
        CommandBindings.Add(new(FormCommands.Save, (_, _) => Save(), CanSave));
        CommandBindings.Add(new(FormCommands.Ok, (_, _) => { Save(); FormCommands.Close.Execute(null, this); }));
    }

    private void CanSave(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
        //e.CanExecute =
        //    Sample.LabId != SampleData.LabId ||
        //    Sample.Grams != SampleData.Grams ||
        //    Sample.Process != SampleData.Process ||
        //    Sample.AliquotsCount != SampleData.AliquotsCount ||
        //    //Sample.AliquotIds != SampleData.AliquotIds ||
        //    Sample.Take_d13C != SampleData.Take_d13C ||
        //    Sample.SulfurSuspected != SampleData.SulfurSuspected ||
        //    Sample.InletPort.Name != SampleData.InletPort;
    }

    public virtual void Save()
    {
        Sample ??= new Sample();

        Sample.LabId = SampleData.LabId;
        Sample.Grams = SampleData.Units.ToGrams(SampleData.Mass);
        Sample.Process = SampleData.Process;

        Sample.Parameters.Clear();
        foreach (var parameter in SampleData.Parameters)
            Sample.SetParameter(parameter.Clone());

        // Refresh SampleData in case redundant parameters were removed
        SampleData.Parameters.Clear();
        foreach (var parameter in Sample.Parameters)
            SampleData.Parameters.Add(parameter.Clone());

        Sample.AliquotsCount = SampleData.AliquotIds.Count();
        for (int i = 0; i < Sample.AliquotsCount; i++)
            Sample.Aliquots[i].Name = SampleData.AliquotIds[i].Id;

        Sample.Take_d13C = SampleData.Take_d13C;
        Sample.SulfurSuspected = SampleData.SulfurSuspected;
        if (SampleData.InletPort == "None")
            Sample.InletPort = null;
        else
            Sample.InletPort = NamedObject.Find<IInletPort>(SampleData.InletPort);

        // Should we change StartingIP.Sample?
        if (StartingIP != null && StartingIP.Sample == Sample && Sample.InletPort != StartingIP)
            StartingIP.ClearContents();

        // Should we change Sample.InletPort.Sample?
        if (Sample.InletPort is IInletPort ip)
        {
            var ipIsFree =
                    ip.Sample == null ||
                    ip.State == LinePort.States.Complete ||
                    ip.State == LinePort.States.Empty;

            if (ipIsFree)
            {
                ip.Sample = Sample;
                ip.State = LinePort.States.Loaded;
            }
        }

        // Should we update Cegs's Sample?
        if (NamedObject.FirstOrDefault<Cegs>() is Cegs cegs && !cegs.Busy)
            cegs.Sample = Sample;

        Updated?.Invoke();
    }

    protected virtual void FilterProcesses()
    {
        InletPortType ToIPType(string type) => type switch
        {
            "Combustion" =>  InletPortType.Combustion,
            "Needle" => InletPortType.Needle,
            "Manual" => InletPortType.Manual,
            "GasSupply" => InletPortType.GasSupply,
            "TFCombustion" => InletPortType.TFCombustion,
            "FlowThrough" => InletPortType.FlowThrough,
            _ => InletPortType.Combustion
        };

        IEnumerable<ProcessSequence> processes = NamedObject.FindAll<ProcessSequence>();

        var process = SampleData.Process;
        if (ProcessType != "Any")
            processes = processes.Where(p => p.PortType == ToIPType(ProcessType));
        Processes = processes.Select(p => p.Name);
        if (!Processes.Contains(process))
            SampleData.Process = Processes.FirstOrDefault() ?? "";
    }
}
