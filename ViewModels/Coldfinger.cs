using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
    public class Coldfinger : StateManager
    {
        [Browsable(false)]
        public new Components.IColdfinger Component
        {
            get => base.Component as Components.IColdfinger;
            protected set => base.Component = value;
        }

        public ViewModel LNValve => GetFromModel(Component?.LNValve);
        public ViewModel AirValve => GetFromModel(Component?.AirValve);
        public ViewModel LNManifold => GetFromModel(Component?.LNManifold);
        public ViewModel LevelSensor => GetFromModel(Component?.LevelSensor);
        public ViewModel AirThermometer => GetFromModel(Component?.AirThermometer);

        public int FrozenTemperature { get => Component.FrozenTemperature; set => Component.FrozenTemperature = value; }
        public string Trickle { get => Component.Trickle; set => Component.Trickle = value; }
        public int FreezeTrigger { get => Component.FreezeTrigger; set => Component.FreezeTrigger = value; }
        public int RaiseTrigger { get => Component.RaiseTrigger; set => Component.RaiseTrigger = value; }
        public int MaximumSecondsLNFlowing { get => Component.MaximumSecondsLNFlowing; set => Component.MaximumSecondsLNFlowing = value; }
        public int SecondsToWaitAfterRaised { get => Component.SecondsToWaitAfterRaised; set => Component.SecondsToWaitAfterRaised = value; }
        public double NearAirTemperature { get => Component.NearAirTemperature; set => Component.NearAirTemperature = value; }
        public bool Thawing => Component.Thawing;
        public bool Frozen => Component.Frozen;
        public bool Raised => Component.Raised;
        public bool IsNearAirTemperature => Component.IsNearAirTemperature;
        public bool IsActivelyCooling => Component.IsActivelyCooling;
        public bool Thawed => Component.Thawed;
        public double Temperature => Component.Temperature;
        public double AirTemperature => Component.AirTemperature;
        public double Target => Component.Target;

        protected string StandbyCaption { get; set; } = "Standby";
        protected string FreezeCaption { get; set; } = "Freeze";
        protected string RaiseCaption { get; set; } = "Raise";
        protected string ThawCaption { get; set; } = "Thaw";
        protected override void StartContext()
        {
            base.StartContext();
            ContextStart.Add(new Context(StandbyCaption));
            ContextStart.Add(new Context(FreezeCaption));
            ContextStart.Add(new Context(RaiseCaption));
            ContextStart.Add(new Context(ThawCaption));
        }

        public override void Run(string command = "")
        {
            if (command == StandbyCaption)
                Component?.Standby();
            else if (command == FreezeCaption)
                Component?.Freeze();
            else if (command == RaiseCaption)
                Component?.Raise();
            else if (command == ThawCaption)
                Component?.Thaw();
            base.Run(command);
        }
    }
}
