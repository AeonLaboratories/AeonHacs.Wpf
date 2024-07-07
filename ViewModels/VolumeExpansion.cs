using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
    public class VolumeExpansion : HacsComponent
    {
        [Browsable(false)]
        public new Components.IVolumeExpansion Component
        {
            get => base.Component as Components.IVolumeExpansion;
            protected set => base.Component = value;
        }

        //public Chamber Chamber { get => Component.Chamber; set => Component.Chamber = value; }

        //TODO: How do I handle lists?
        //public List<Valve> ValveList { get => Component.ValveList; set => Component.ValveList = value; }
    }
}
