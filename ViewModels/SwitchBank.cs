using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
    public class SwitchBank : SerialDeviceManager
    {
        [Browsable(false)]
        public new Components.ISwitchBank Component
        {
            get => base.Component as Components.ISwitchBank;
            protected set => base.Component = value;
        }
        public int Channels { get => Component.Channels; set => Component.Channels = value; }
        public string Model => Component.Model;
        public string Firmware => Component.Firmware;
        public int SelectedSwitch => Component.SelectedSwitch;
        public Components.SwitchBank.ErrorCodes Errors => Component.Errors;

    }
}
