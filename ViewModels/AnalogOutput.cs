using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
    public class AnalogOutput : ManagedDevice
    {
        [Browsable(false)]
        public new Components.IAnalogOutput Component
        {
            get => base.Component as Components.IAnalogOutput;
            protected set => base.Component = value;
        }
        public double Voltage => Component.Voltage;
        public double TargetVoltage { get => Component.Config.Voltage; set => Component.Voltage = value; }
        public long MillisecondsInState => Component.MillisecondsInState;
    }
}
