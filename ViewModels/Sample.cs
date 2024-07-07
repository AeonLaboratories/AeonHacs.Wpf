using System.ComponentModel;
using System.Collections.Generic;

namespace AeonHacs.Wpf.ViewModels
{
    public class Sample : HacsComponent
    {
        [Browsable(false)]
        public new Components.ISample Component
        {
            get => base.Component as Components.ISample;
            protected set => base.Component = value;
        }
        public string LabId { get => Component.LabId; set => Component.LabId = value; }
        public ViewModel InletPort => GetFromModel(Component?.InletPort);
        public string Process { get => Component.Process; set => Component.Process = value; }
        public bool SulfurSuspected { get => Component.SulfurSuspected; set => Component.SulfurSuspected = value; }
        public bool Take_d13C { get => Component.Take_d13C; set => Component.Take_d13C = value; }
        public double Grams { get => Component.Grams; set => Component.Grams = value; }
        public double Milligrams { get => Component.Milligrams; set => Component.Milligrams = value; }
        public double Micrograms { get => Component.Micrograms; set => Component.Micrograms = value; }
        public double Micromoles { get => Component.Micromoles; set => Component.Micromoles = value; }
        public double TotalMicrogramsCarbon { get => Component.TotalMicrogramsCarbon; set => Component.TotalMicrogramsCarbon = value; }
        public double TotalMicromolesCarbon { get => Component.TotalMicromolesCarbon; set => Component.TotalMicromolesCarbon = value; }
        public double SelectedMicrogramsCarbon { get => Component.SelectedMicrogramsCarbon; set => Component.SelectedMicrogramsCarbon = value; }
        public double SelectedMicromolesCarbon { get => Component.SelectedMicromolesCarbon; set => Component.SelectedMicromolesCarbon = value; }
        public double MicrogramsDilutionCarbon { get => Component.MicrogramsDilutionCarbon; set => Component.MicrogramsDilutionCarbon = value; }
        public double Micrograms_d13C { get => Component.Micrograms_d13C; set => Component.Micrograms_d13C = value; }
        public double d13CPartsPerMillion { get => Component.d13CPartsPerMillion; set => Component.d13CPartsPerMillion = value; }
        public int AliquotsCount { get => Component.AliquotsCount; set => Component.AliquotsCount = value; }
        public List<string> AliquotIds { get => Component.AliquotIds; set => Component.AliquotIds = value; }
        public List<Components.IAliquot> Aliquots { get => Component.Aliquots; set => Component.Aliquots = value; }

    }
}
