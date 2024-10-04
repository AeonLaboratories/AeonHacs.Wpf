using AeonHacs.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AeonHacs.Wpf.Data;

public class SampleData : BindableObject
{
    string labId;
    public string LabId
    {
        get => labId;
        set => Ensure(ref labId, value);
    }

    MassWithUnits mwu = new();

    public double Mass
    {
        get => mwu.Mass;
        set => mwu.Mass = value;
    }

    public MassUnits Units
    {
        get => mwu.Units;
        set => mwu.Units = value;
    }

    string process;
    public string Process
    {
        get => process;
        set => Ensure(ref process, value);
    }

    public ObservableCollection<Parameter> Parameters { get; } = [];

    public ObservableCollection<AliquotId> AliquotIds { get; } = [];

    bool take_d13C;
    public bool Take_d13C
    {
        get => take_d13C;
        set => Ensure(ref take_d13C, value);
    }

    bool sulfurSuspected;
    public bool SulfurSuspected
    {
        get => sulfurSuspected;
        set => Ensure(ref sulfurSuspected, value);
    }

    string inletPort;
    public string InletPort
    {
        get => inletPort;
        set => Ensure(ref inletPort, value);
    }

    public SampleData(ISample sample)
    {
        mwu.PropertyChanged += NotifyPropertyChanged;
        if (sample != null)
        {
            LabId = sample.LabId;
            Mass = sample.Grams;
            Process = sample.Process;
            sample.Parameters.ForEach(p => Parameters.Add(p.Clone()));
            AliquotIds = new(sample.AliquotIds.Select(id => new AliquotId() { Id = id }));
            Take_d13C = sample.Take_d13C;
            SulfurSuspected = sample.SulfurSuspected;
            InletPort = sample.InletPort?.Name ?? "None";
        }
        else
        {
            Take_d13C = CegsPreferences.Take13CDefault;
            AliquotIds.Add(new());
            InletPort = "None";
        }
        Units = CegsPreferences.DefaultMassUnits switch
        {
            AeonHacs.MassUnits.μmol => MassUnits.MicromolesCarbon,
            AeonHacs.MassUnits.μg => MassUnits.Micrograms,
            AeonHacs.MassUnits.mg => MassUnits.Milligrams,
            AeonHacs.MassUnits.g => MassUnits.Grams,
            _ => MassUnits.Grams,
        };
    }

    protected override void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender == mwu)
            base.NotifyPropertyChanged(this, e);
        else
            base.NotifyPropertyChanged(sender, e);
    }
}
