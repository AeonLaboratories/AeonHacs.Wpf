using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
    public class AIVoltmeter : Voltmeter
    {
        [Browsable(false)]
        public new Components.IAIVoltmeter Component
        {
            get => base.Component as Components.IAIVoltmeter;
            protected set => base.Component = value;
        }
        AeonHacs.AnalogInputMode AnalogInputMode { get => Component.AnalogInputMode; set => Component.AnalogInputMode = value; }
        public new double MaximumVoltage { get => Component.MaximumVoltage; set => Component.MaximumVoltage = value; }
        public new double MinimumVoltage { get => Component.MinimumVoltage; set => Component.MinimumVoltage = value; }
        public new bool OverRange => Component.OverRange;
        public new bool UnderRange => Component.OverRange;
    }
}
