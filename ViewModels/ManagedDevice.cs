using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels;

public class ManagedDevice : HacsDevice
{
    [Browsable(false)]
    public new Components.IManagedDevice Component
    {
        get => base.Component as Components.IManagedDevice;
        protected set => base.Component = value;
    }
    public ViewModel Manager => GetFromModel(Component?.Manager);
}
