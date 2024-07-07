using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
    public class PidControl : HacsComponent
    {
        [Browsable(false)]
        public new Components.IPidControl Component
        {
            get => base.Component as Components.IPidControl;
            protected set => base.Component = value;
        }
        public ViewModel PidSetup => GetFromModel(Component?.PidSetup);

        public int MillisecondsUpdate { get => Component.MillisecondsUpdate; set => Component.MillisecondsUpdate = value; }
        public double ControlOutputLimit { get => Component.ControlOutputLimit; set => Component.ControlOutputLimit = value; }
        public double ReferencePoint { get => Component.ReferencePoint; set => Component.ReferencePoint = value; }
        public double MinimumControlledProcessVariable { get => Component.MinimumControlledProcessVariable; set => Component.MinimumControlledProcessVariable = value; }
        public double BlindControlOutput { get => Component.BlindControlOutput; set => Component.BlindControlOutput = value; }
        public bool Busy => Component.Busy;
    }
}
