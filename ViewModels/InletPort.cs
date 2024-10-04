using AeonHacs.Components;
using System.ComponentModel;
using System.Windows;

namespace AeonHacs.Wpf.ViewModels
{
    public class InletPort : LinePort
    {
        public InletPort() { RunHasDefault = true; }

        [Browsable(false)]
        public new Components.IInletPort Component
        {
            get => base.Component as Components.IInletPort;
            protected set => base.Component = value;
        }

        public AeonHacs.InletPortType PortType { get => Component.PortType; set => Component.PortType = value; }

        public ViewModel QuartzFurnace
        {
            get => GetFromModel(Component?.QuartzFurnace);
            set { }
        }
        public ViewModel SampleFurnace
        {
            get
            {
                if (Component?.SampleFurnace is ITubeFurnace)
                    return null;    // omit CC furnace if it's depicted elsewhere
                return GetFromModel(Component?.SampleFurnace);
            }
        }

        public bool NotifySampleFurnaceNeeded { get => Component.NotifySampleFurnaceNeeded; set => Component.NotifySampleFurnaceNeeded = value; }
        public int WarmTemperature { get => Component.WarmTemperature; set => Component.WarmTemperature = value; }

        // TODO Decide context menu for InletPorts

        //void TurnOffFurnaces();

        protected string SampleCaption { get; set; } = "Edit Sample";
        protected override void StartContext()
        {
            ContextStart.Add(new Wpf.Context(SampleCaption, dispatch:false));
            base.StartContext();
        }

        public override void Run(string command = "")
        {
            if (command == SampleCaption || command.IsBlank())
                EditSample();
            else
                base.Run(command);
        }

        void EditSample()
        {
            Application.Current.Dispatcher.Invoke(() => HacsCommands.EditSample.Execute(Component, null));
        }
    }
}
