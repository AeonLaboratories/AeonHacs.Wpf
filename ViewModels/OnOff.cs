using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
    public class OnOff : HacsDevice
    {
        [Browsable(false)]
        public new Components.IOnOff Component
        {
            get => base.Component as Components.IOnOff;
            protected set => base.Component = value;
        }
        public AeonHacs.OnOffState OnOffState => Component.OnOffState;
        public bool IsOn => Component.IsOn;
        public bool IsOff => Component.IsOff;
        public long MillisecondsOn => Component.MillisecondsOn;
        public long MillisecondsOff => Component.MillisecondsOff;
        public long MillisecondsInState => Component.MillisecondsInState;
    }
}
