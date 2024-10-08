﻿using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
    public class MassFlowController : AnalogOutput
    {
        [Browsable(false)]
        public new Components.IMassFlowController Component
        {
            get => base.Component as Components.IMassFlowController;
            protected set => base.Component = value;
        }

        // TODO OperationSet ViewModel needed?
        public Utilities.OperationSet OutputConverter
        { get => Component.OutputConverter; set => Component.OutputConverter = value; }

        public double TrackedFlow { get => Component.TrackedFlow; set => Component.TrackedFlow = value; }
        public double MinimumSetpoint { get => Component.MinimumSetpoint; set => Component.MinimumSetpoint = value; }
        public double MaximumSetpoint { get => Component.MaximumSetpoint; set => Component.MaximumSetpoint = value; }
        public double FlowRate => Component.FlowRate;
        public double TargetSetpoint { get => Component.Setpoint; set => Component.Setpoint = value; }


        protected string ResetTrackedFlowCaption { get; set; } = "Reset Tracked Flow";
        protected string ZeroNowCaption { get; set; } = "Zero Now";
        protected override void StartContext()
        {
            base.StartContext();
            ContextStart.Add(new Context(ResetTrackedFlowCaption));
            ContextStart.Add(new Context(ZeroNowCaption));
        }

        public override void Run(string command = "")
        {
            if (command == ResetTrackedFlowCaption)
                Component?.ResetTrackedFlow();
            else if (command == ZeroNowCaption)
                Component?.ZeroNow();
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
