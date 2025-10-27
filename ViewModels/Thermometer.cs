using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels;

public class Thermometer : Meter
{
    [Browsable(false)]public new Components.IThermometer Component
    {
        get => base.Component as Components.IThermometer;
        protected set => base.Component = value;
    }
    public double Temperature => Component.Temperature;

    protected override void StartContext()
    {
        base.StartContext();

        // Thermometers don't need to be Zeroed
        var z = ContextStart.Find(x => x.Label == ZeroNowCaption);
        if (z != null) ContextStart.Remove(z);
    }

}
