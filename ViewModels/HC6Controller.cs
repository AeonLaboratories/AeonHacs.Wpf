using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels;

public class HC6Controller : SerialDeviceManager
{
    [Browsable(false)]
    public new Components.IHC6Controller Component
    {
        get => base.Component as Components.IHC6Controller;
        protected set => base.Component = value;
    }
    public string Model => Component.Model;
    public string Firmware => Component.Firmware;
    public int SerialNumber => Component.SerialNumber;
    public int SelectedHeater => Component.SelectedHeater;
    public int SelectedThermocouple => Component.SelectedThermocouple;
    public int AdcCount => Component.AdcCount;
    public double ColdJunctionTemperature => Component.ColdJunctionTemperature;
    public double ReadingRate => Component.ReadingRate;
    public Components.HC6ErrorCodes Errors => Component.Errors;
}
