using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
    public class Chamber : HacsComponent
    {
        [Browsable(false)]
        public new Components.IChamber Component
        {
            get => base.Component as Components.IChamber;
            protected set => base.Component = value;
        }
        public double Pressure => Component.Pressure;
        public double Temperature => Component.Temperature;
        public double MilliLiters { get => Component.MilliLiters; set => Component.MilliLiters = value; }

        public ViewModel Manometer => GetFromModel(Component?.Manometer);
        public ViewModel Thermometer => GetFromModel(Component?.Thermometer);
        public ViewModel Heater => GetFromModel(Component?.Heater);
        public ViewModel Coldfinger => GetFromModel(Component?.Coldfinger);


    }
}
