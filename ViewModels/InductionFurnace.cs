using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels;

public class InductionFurnace : TubeFurnace
{
    [Browsable(false)]
    public new Components.IInductionFurnace Component
    {
        get => base.Component as Components.IInductionFurnace;
        protected set => base.Component = value;
    }
    public ViewModel Pyrometer => GetFromModel(Component?.Pyrometer);

    public int PowerLevel { get => Component.PowerLevel; set => Component.PowerLevel = value; }
    public int MinimumControlledTemperature => Component.MinimumControlledTemperature;
    public int PowerLimit => Component.PowerLimit;
    public int Error => Component.Error;
    public int Voltage => Component.Voltage;
    public double Current => Component.Current;
    public int Frequency => Component.Frequency;
    public Components.InductionFurnace.ControlModeCode ControlMode => Component.ControlMode;
    public char DeviceAddress => Component.DeviceAddress;
    public char HostAddress => Component.HostAddress;
    public int Status => Component.Status;
    public string InterfaceBoardRevision => Component.InterfaceBoardRevision;
}
