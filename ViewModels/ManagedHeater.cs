using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
    public class ManagedHeater : Heater
    {
        [Browsable(false)]
        public new Components.IManagedHeater Component
        {
            get => base.Component as Components.IManagedHeater;
            protected set => base.Component = value;
        }
        public ViewModel Manager => GetFromModel(Component?.Manager);
    }
}
