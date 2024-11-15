using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
    public class VTColdfinger : HacsComponent
    {
        [Browsable(false)]
        public new Components.IVTColdfinger Component
        {
            get => base.Component as Components.IVTColdfinger;
            protected set => base.Component = value;
        }
        public double Temperature => Component.Temperature;

        public bool IsOn => Component.IsOn;

        public AeonHacs.OnOffState OnOffState => Component.OnOffState;

        public AeonHacs.StopAction StopAction { get => Component.StopAction; set => Component.StopAction = value; }

        public ViewModel Heater => GetFromModel(Component.Heater);

        public ViewModel Coldfinger => GetFromModel(Component.Coldfinger);

        protected string StandbyCaption { get; set; } = "Standby";
        protected string ThawCaption { get; set; } = "Thaw";
        protected string FreezeCaption { get; set; } = "Freeze";
        protected string RegulateCaption { get; set; } = "Regulate";
        protected override void StartContext()
        {
            ContextStart.Clear();
            ContextStart.Add(new Context(StandbyCaption));
            ContextStart.Add(new Context(ThawCaption));
            ContextStart.Add(new Context(FreezeCaption));
            ContextStart.Add(new Context(RegulateCaption));
        }

        public override void Run(string command = "")
        {
            if (command == StandbyCaption)
                Component?.Standby();
            else if (command == ThawCaption)
                Component?.Thaw();
            else if (command == FreezeCaption)
                Component?.Freeze();
            else if (command == RegulateCaption)
                Component?.Regulate();
        }

    }
}
