using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels;

public class SerialDeviceManager : DeviceManager
{
    [Browsable(false)]
    public new Components.ISerialDeviceManager Component
    {
        get => base.Component as Components.ISerialDeviceManager;
        protected set => base.Component = value;
    }

    public ViewModel SerialController => GetFromModel(Component.SerialController);

}
