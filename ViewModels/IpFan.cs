using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AeonHacs.Wpf.ViewModels
{
    public class IpFan : HacsComponent
    {
        [Browsable(false)]
        public new Components.IpFan Component
        {
            get => base.Component as Components.IpFan;
            protected set => base.Component = value;
        }

        public ViewModel Fan => GetFromModel(Component?.Fan);
        public List<ViewModel> InletPorts
        {
            get => Component?.InletPorts?.ConvertAll(x => GetFromModel(x));
            set { if (Component != null) Component.InletPorts = value?.ConvertAll(x => x.Component as Components.IInletPort); }
        }

        protected string MonitorCaption { get; set; } = "Monitor";
        protected string StayOnCaption { get; set; } = "Stay On";
        protected string StayOffCaption { get; set; } = "Stay Off";
        protected string StandbyCaption { get; set; } = "Standby";

        protected override void StartContext()
        {
            ContextStart.Clear();
            ContextStart.Add(new Context(MonitorCaption));
            ContextStart.Add(new Context(StayOnCaption));
            ContextStart.Add(new Context(StayOffCaption));
            ContextStart.Add(new Context(StandbyCaption));
        }

        public override void Run(string command = "")
        {
            if (command == MonitorCaption)
                Component?.Monitor();
            else if (command == StayOnCaption)
                Component.StayOn();
            else if (command == StayOffCaption)
                Component.StayOff();
            else if (command == StandbyCaption)
                Component?.Standby();
            base.Run(command);
        }
    }
}
