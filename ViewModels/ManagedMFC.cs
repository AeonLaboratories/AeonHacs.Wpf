using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
    public class ManagedMFC : ManagedDevice
    {
        [Browsable(false)]
        public new Components.ManagedMFC Component
        {
            get => base.Component as Components.ManagedMFC;
            protected set => base.Component = value;
        }

        public string PendingRequest { get => Component.PendingRequest; set => Component.PendingRequest = value; }
        public double TrackedFlow { get => Component.TrackedFlow; set => Component.TrackedFlow = value; }
        public double MinimumSetpoint { get => Component.MinimumSetpoint; set => Component.MinimumSetpoint = value; }
        public double MaximumSetpoint { get => Component.MaximumSetpoint; set => Component.MaximumSetpoint = value; }
        public double FlowRate => Component.FlowRate;
        public double TargetSetpoint { get => Component.Setpoint; set => Component.Setpoint = value; }


        protected string ResetTrackedFlowCaption { get; set; } = "Reset Tracked Flow";
        protected string ZeroNowCaption { get; set; } = "Zero Now";
        protected string CheckProgrammedGasCaption { get; set; } = "Check Programmed Gas";
        protected string CheckMaximumSetpointCaption { get; set; } = "Check Maximum Setpoint";
        protected string CheckSetpointCaption { get; set; } = "Check Setpoint";

        protected override void StartContext()
        {
            base.StartContext();
            ContextStart.Add(new Context(ResetTrackedFlowCaption));
            ContextStart.Add(new Context(ZeroNowCaption));
            ContextStart.Add(new Context(CheckProgrammedGasCaption));
            ContextStart.Add(new Context(CheckMaximumSetpointCaption));
            ContextStart.Add(new Context(CheckSetpointCaption));
        }

        public override void Run(string command = "")
        {
            if (command == ResetTrackedFlowCaption)
                Component?.ResetTrackedFlow();
            else if (command == ZeroNowCaption)
                Component?.ZeroNow();
            else if (command == CheckProgrammedGasCaption)
                Component?.CheckProgrammedGas();
            else if (command == CheckMaximumSetpointCaption)
                Component?.CheckMaximumSetpoint();
            else if (command == CheckSetpointCaption)
                Component?.CheckSetpoint();
            else
                base.Run(command);
        }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Setpoint")
                base.OnPropertyChanged(sender, PropertyChangedEventArgs(nameof(TargetSetpoint)));
            else
                base.OnPropertyChanged(sender, e);
        }
    }
}
