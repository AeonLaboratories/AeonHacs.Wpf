using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
    public class Aliquot : ViewModel
    {
        [Browsable(false)]
        public new Components.IAliquot Component
        {
            get => base.Component as Components.IAliquot;
            protected set => base.Component = value;
        }
        public string AliquotName { get => Component.Name; set => Component.Name = value; }
        public Components.ISample Sample { get => Component.Sample; set => Component.Sample = value; }
        public string GraphiteReactor { get => Component.GraphiteReactor; set => Component.GraphiteReactor = value; }
        public double MicrogramsCarbon { get => Component.MicrogramsCarbon; set => Component.MicrogramsCarbon = value; }
        public double MicromolesCarbon => Component.MicromolesCarbon;
        public double TargetInitialH2Pressure { get => Component.InitialGmH2Pressure; set => Component.InitialGmH2Pressure = value; }
        public double TargetFinalH2Pressure { get => Component.FinalGmH2Pressure; set => Component.FinalGmH2Pressure = value; }
        public double H2CO2PressureRatio { get => Component.H2CO2PressureRatio; set => Component.H2CO2PressureRatio = value; }
        public double ExpectedResidualPressure { get => Component.ExpectedResidualPressure; set => Component.ExpectedResidualPressure = value; }
        public double ResidualPressure { get => Component.ResidualPressure; set => Component.ResidualPressure = value; }
        public bool ResidualMeasured { get => Component.ResidualMeasured; set => Component.ResidualMeasured = value; }
        public int Tries { get => Component.Tries; set => Component.Tries = value; }
    }
}
