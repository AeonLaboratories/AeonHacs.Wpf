using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels;

public class HacsDevice : HacsComponent
{
    [Browsable(false)]
    public new Components.IHacsDevice Component
    {
        get => base.Component as Components.IHacsDevice;
        protected set => base.Component = value;
    }
    public long UpdatesReceived { get => Component.UpdatesReceived; }
}
