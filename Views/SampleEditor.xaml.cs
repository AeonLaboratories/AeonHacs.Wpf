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
        typeof(Sample),
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

    public static readonly DependencyProperty ProtocolTypeProperty = DependencyProperty.Register(
        nameof(ProtocolType),
        typeof(string),
        typeof(SampleEditor),
        new PropertyMetadata(ProtocolTypeChanged)
    );

    private static void ProtocolTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SampleEditor se)
            se.FilterProtocols();
    }

    public static readonly DependencyProperty ProtocolProperty = DependencyProperty.Register(
        nameof(Protocols),
        typeof(IEnumerable<string>),
        typeof(SampleEditor)
    );

    public static IEnumerable<string> ProtocolTypes { get; } = Enum.GetNames<InletPortType>().Prepend("Any");

    public static Visibility Take13CVisibility { get; } =
        NamedObject.FindAll<Id13CPort>().Count > 0 ? Visibility.Visible : Visibility.Collapsed;

    public static IEnumerable<string> InletPorts { get; } = NamedObject.FindAll<IInletPort>().Select(ip => ip.Name).Prepend("None");

    public Sample Sample
    {
        get => (Sample)GetValue(SamplePropertyKey.DependencyProperty);
        protected set => SetValue(SamplePropertyKey, value);
    }

    public SampleData SampleData
    {
        get => (SampleData)GetValue(SampleDataPropertyKey.DependencyProperty);
        protected set => SetValue(SampleDataPropertyKey, value);
    }

    public string ProtocolType
    {
        get => (string)GetValue(ProtocolTypeProperty);
        set => SetValue(ProtocolTypeProperty, value);
    }

    public IEnumerable<string> Protocols
    {
        get => (IEnumerable<string>)GetValue(ProtocolProperty);
        set => SetValue(ProtocolProperty, value);
    }

    public SampleEditor(Sample sample = null)
    {
        InitializeComponent();
        InitializeCommands();

        Sample = sample;
        SampleData = new(sample);

        if (Sample?.InletPort?.PortType is InletPortType protocolType)
            ProtocolType = protocolType.ToString();
        else
            ProtocolType = "Any";
    }

    public SampleEditor(IInletPort ip) : this(ip?.Sample)
    {
        SampleData.InletPort = ip?.Name ?? "None";
        if (ProtocolType == "Any")
            ProtocolType = ip?.PortType.ToString() ?? "Any";
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
        //    Sample.Protocol != SampleData.Protocol ||
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
        Sample.Protocol = SampleData.Protocol;

        Sample.Parameters.Clear();
        foreach (var parameter in SampleData.Parameters)
        {
            var p = parameter.Clone();
            p.Description = "";         // Clear description to avoid redundancy, save space
            Sample.SetParameter(p);
        }

        // Refresh SampleData in case redundant parameters were removed
        SampleData.Parameters.Clear();
        foreach (var parameter in Sample.Parameters)
            SampleData.Parameters.Add(parameter.Clone());       // description is already empty

        Sample.AliquotsCount = SampleData.AliquotIds.Count();
        for (int i = 0; i < Sample.AliquotsCount; i++)
            Sample.Aliquots[i].Name = SampleData.AliquotIds[i].Id;

        Sample.Take_d13C = SampleData.Take_d13C;
        Sample.SulfurSuspected = SampleData.SulfurSuspected;

        var oldIP = Sample.InletPort;

        if (SampleData.InletPort == "None")
            Sample.InletPort = null;
        else
            Sample.InletPort = NamedObject.Find<IInletPort>(SampleData.InletPort);

        if (Sample.InletPort != oldIP && oldIP?.Sample == Sample)
            oldIP.ClearContents();

        // Should we change Sample.InletPort.Sample?
        if (Sample.InletPort is IInletPort ip)
        {
            var ipIsFree =
                    ip.Sample == null ||
                    ip.Sample.State == Components.Sample.States.Complete ||
                    ip.State == LinePort.States.Complete ||
                    ip.State == LinePort.States.Empty;

            if (ipIsFree)
            {
                ip.Sample = Sample;
                ip.State = LinePort.States.Loaded;
                if (Sample.State == Sample.States.Unknown)
                    Sample.State = Sample.States.Loaded;
            }

            if (ip.Sample == Sample)
            {
                if (NamedObject.Find<Protocol>(Sample.Protocol) is Protocol ps)
                    ip.PortType = ps.PortType;
            }
        }

        // Should we update Cegs's Sample?
        if (NamedObject.FirstOrDefault<Cegs>() is Cegs cegs && !cegs.Busy)
            cegs.Sample = Sample;

        Updated?.Invoke();
    }

    protected virtual void FilterProtocols()
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

        IEnumerable<Protocol> protocols = NamedObject.FindAll<Protocol>();

        var protocol = SampleData.Protocol;
        if (ProtocolType != "Any")
            protocols = protocols.Where(p => p.PortType == ToIPType(ProtocolType));
        Protocols = protocols.Select(p => p.Name);
        if (!Protocols.Contains(protocol))
            SampleData.Protocol = Protocols.FirstOrDefault() ?? "";
    }
}
