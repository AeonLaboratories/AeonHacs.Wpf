using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
    public class LinePort : Port
    {
        [Browsable(false)]
        public new Components.ILinePort Component
        {
            get => base.Component as Components.ILinePort;
            protected set => base.Component = value;
        }
        public Components.LinePort.States State { get => Component.State; set => Component.State = value; }
        public ViewModel Sample => GetFromModel(Component?.Sample);
        public ViewModel Aliquot => GetFromModel(Component?.Aliquot);
        public string Contents => Component.Contents;

        // TODO need Context menu for controlling State
        protected string LoadedCaption { get; set; } = "Loaded";
        protected string EmptyCaption { get; set; } = "Empty";
        protected string DisabledCaption { get; set; } = "Disable";

        protected override void StartContext()
        {
            ContextStart.Add(new Context(LoadedCaption));
            ContextStart.Add(new Context(EmptyCaption));
            ContextStart.Add(new Context(DisabledCaption));
            base.StartContext();
        }

        public override void Run(string command = "")
        {
            if (Component == null) return;
            if (command == LoadedCaption)
                Component.State = Components.LinePort.States.Loaded;
            else if (command == DisabledCaption)
                Component.State = Components.LinePort.States.Disabled;
            else if (command == EmptyCaption)
                Component.State = Components.LinePort.States.Empty;
            else
                base.Run(command);
        }

    }
}
