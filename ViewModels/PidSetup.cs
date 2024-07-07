using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
    public class PidSetup : HacsComponent
    {
        [Browsable(false)]
        public new Components.IPidSetup Component
        {
            get => base.Component as Components.IPidSetup;
            protected set => base.Component = value;
        }
        public double Gain { get => Component.Gain; set => Component.Gain = value; }
        public double Integral { get => Component.Integral; set => Component.Integral = value; }
        public double Derivative { get => Component.Derivative; set => Component.Derivative = value; }
        public double Preset { get => Component.Preset; set => Component.Preset = value; }
        public int GainPrecision { get => Component.GainPrecision; set => Component.GainPrecision = value; }
        public int IntegralPrecision { get => Component.IntegralPrecision; set => Component.IntegralPrecision = value; }
        public int DerivativePrecision { get => Component.DerivativePrecision; set => Component.DerivativePrecision = value; }
        public int PresetPrecision { get => Component.PresetPrecision; set => Component.PresetPrecision = value; }
        // These are just "mirrors" for the doubles above; they are not need in Device settings
        //public int EncodedGain { get => Component.EncodedGain; set => Component.EncodedGain = value; }
        //public int EncodedIntegral { get => Component.EncodedIntegral; set => Component.EncodedIntegral = value; }
        //public int EncodedDerivative { get => Component.EncodedDerivative; set => Component.EncodedDerivative = value; }
        //public int EncodedPreset { get => Component.EncodedPreset; set => Component.EncodedPreset = value; }
    }
}
