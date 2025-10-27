using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels;

public class Manometer : Meter
{
    [Browsable(false)]
    public new Components.IManometer Component
    {
        get => base.Component as Components.IManometer;
        protected set => base.Component = value;
    }
    public double Pressure => Component.Pressure;
}
