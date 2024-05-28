using AeonHacs.Components;
using System.Collections.Generic;
using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
    public class Section : Chamber
    {
        [Browsable(false)]
        public new Components.ISection Component
        {
            get => base.Component as Components.ISection;
            protected set => base.Component = value;
        }


        public List<ViewModel> Chambers
        {
            get => Component?.Chambers?.ConvertAll(x => GetFromModel(x));
            set { if (Component != null) Component.Chambers = value?.ConvertAll(x => x.Component as Components.IChamber); }
        }

        public List<ViewModel> Ports
        {
            get => Component?.Ports?.ConvertAll(x => GetFromModel(x));
            set { if (Component != null) Component.Ports = value?.ConvertAll(x => x.Component as Components.IPort); }
        }

        public ViewModel VacuumSystem => GetFromModel(Component?.VacuumSystem);

        public List<ViewModel> Isolation
        {
            get => Component?.Isolation?.ConvertAll(x => GetFromModel(x));
            set { if (Component != null) Component.Isolation = value?.ConvertAll(x => x.Component as Components.IValve); }
        }

        public List<ViewModel> InternalValves
        {
            get => Component?.InternalValves?.ConvertAll(x => GetFromModel(x));
            set { if (Component != null) Component.InternalValves = value?.ConvertAll(x => x.Component as Components.IValve); }
        }

        public List<ViewModel> PathToVacuum
        {
            get => Component?.PathToVacuum?.ConvertAll(x => GetFromModel(x));
            set { if (Component != null) Component.PathToVacuum = value?.ConvertAll(x => x.Component as Components.IValve); }
        }

        public List<ViewModel> PathToVacuumIsolation
        {
            get => Component?.PathToVacuumIsolation?.ConvertAll(x => GetFromModel(x));
            set { if (Component != null) Component.PathToVacuumIsolation = value?.ConvertAll(x => x.Component as Components.IValve); }
        }


        public new double MilliLiters => Component.MilliLiters;
        public ViewModel FlowValve => GetFromModel(Component?.FlowValve);
        public ViewModel FlowManager => GetFromModel(Component?.FlowManager);
        public bool IsOpened => Component.IsOpened;

        protected string IsolateCaption { get; set; } = "Isolate";
        protected string OpenCaption { get; set; } = "Open";
        protected string CloseCaption { get; set; } = "Close";
        protected string IsolateFromVacuumCaption { get; set; } = "Isolate from vacuum";
        protected string JoinToVacuumCaption { get; set; } = "Join to vacuum";
        protected string IsolateAndJoinToVacuumCaption { get; set; } = "Isolate and Join to vacuum";
        protected string OpenAndEvacuateCaption { get; set; } = "Open and Evacuate";
        protected string EvacuateCaption { get; set; } = "Evacuate";
        protected string OpenPortsCaption { get; set; } = "Open ports";
        protected string ClosePortsCaption { get; set; } = "Close ports";
        protected string FreezeCaption { get; set; } = "Freeze";
        protected string EmptyAndFreezeCaption { get; set; } = "Empty and Freeze";
        protected string ThawCaption { get; set; } = "Thaw";


        protected override void StartContext()
        {
            ContextStart.Clear();
            ContextStart.Add(new Context(IsolateCaption));
            ContextStart.Add(new Context(EvacuateCaption));
            if (Component?.InternalValves is List<Components.IValve> iv && iv.Count > 0)
            {
                ContextStart.Add(new Context(OpenAndEvacuateCaption));
                ContextStart.Add(new Context(OpenCaption));
                ContextStart.Add(new Context(CloseCaption));
            }
            if (Component?.Ports is List<Components.IPort> pl && pl.Count > 0)
            {
                ContextStart.Add(new Context(OpenPortsCaption));
                ContextStart.Add(new Context(ClosePortsCaption));
            }
            if (Component?.VTColdfinger != null || Component?.Coldfinger != null)
            {
                ContextStart.Add(new Context(FreezeCaption));
                ContextStart.Add(new Context(EmptyAndFreezeCaption));
                ContextStart.Add(new Context(ThawCaption));
            }
            ContextStart.Add(new Context(IsolateFromVacuumCaption));
            ContextStart.Add(new Context(JoinToVacuumCaption));
        }

        public override void Run(string command = "")
        {
            if (command == IsolateCaption)
                Component?.Isolate();
            else if (command == EvacuateCaption)
                Component?.Evacuate();
            else if (command == OpenAndEvacuateCaption)
                Component?.OpenAndEvacuate();
            else if (command == OpenCaption)
                Component?.Open();
            else if (command == CloseCaption)
                Component?.Close();
            else if (command == OpenPortsCaption)
                Component?.OpenPorts();
            else if (command == ClosePortsCaption)
                Component?.ClosePorts();
            else if (command == FreezeCaption)
                Component?.Freeze();
            else if (command == EmptyAndFreezeCaption)
                Component?.EmptyAndFreeze(AeonHacs.NamedObject.FirstOrDefault<Cegs>()?.GetParameter("CleanPressure") ?? 5e-4);
            else if (command == ThawCaption)
                Component?.Thaw();
            else if (command == IsolateFromVacuumCaption)
                Component?.IsolateFromVacuum();
            else if (command == JoinToVacuumCaption)
                Component?.JoinToVacuum();
            base.Run(command);
        }

    }
}
