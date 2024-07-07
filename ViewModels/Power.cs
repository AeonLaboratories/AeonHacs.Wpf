using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
    public class Power : HacsComponent
    {
        [Browsable(false)]
        public new Components.IPower Component
        {
            get => base.Component as Components.IPower;
            protected set => base.Component = value;
        }

        public ViewModel DC5V => GetFromModel(Component?.DC5V);
        public ViewModel MainsDetect => GetFromModel(Component?.MainsDetect);

        public double MainsDetectMinimumVoltage { get => Component.MainsDetectMinimumVoltage; set => Component.MainsDetectMinimumVoltage = value; }
        bool MainsIsDown => Component.MainsIsDown;
        bool MainsHasFailed => Component.MainsHasFailed;
    }
}
