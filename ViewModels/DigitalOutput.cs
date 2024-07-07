using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
    public class DigitalOutput : ManagedSwitch
    {
        [Browsable(false)]
        public new Components.IDigitalOutput Component
        {
            get => base.Component as Components.IDigitalOutput;
            protected set => base.Component = value;
        }
    }
}
