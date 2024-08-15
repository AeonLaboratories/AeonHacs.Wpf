using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
    public class DualManometer : SwitchedManometer
    {
        [Browsable(false)]
        public new Components.IDualManometer Component
        {
            get => base.Component as Components.IDualManometer;
            protected set => base.Component = value;
        }

        public ViewModel HighPressureManometer => GetFromModel(Component?.HighPressureManometer);
        public ViewModel LowPressureManometer => GetFromModel(Component?.LowPressureManometer);

        public double MaximumLowPressure { get => Component.MaximumLowPressure; set => Component.MaximumLowPressure = value; }
        public double MinimumHighPressure { get => Component.MinimumHighPressure; set => Component.MinimumHighPressure = value; }
        public double SwitchpointPressure { get => Component.SwitchpointPressure; set => Component.SwitchpointPressure = value; }

        protected override void StartContext() { }
    }
}
