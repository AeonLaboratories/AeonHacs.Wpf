using System.ComponentModel;
using System.Collections.Generic;

namespace AeonHacs.Wpf.ViewModels
{
    public class DeviceManager : ManagedDevice
    {
        [Browsable(false)]
        public new Components.IDeviceManager Component
        {
            get => base.Component as Components.IDeviceManager;
            protected set => base.Component = value;
        }

        // TODO ViewModel for Dictionary?
        public Dictionary<string, Components.IManagedDevice> Devices => Component.Devices;

        public bool LogEverything { get => Component.LogEverything; set => Component.LogEverything = value; }

        // is there any reason to provide this?
        //Dictionary<IManagedDevice, string> Keys { get; }
    }
}
