using System.ComponentModel;
namespace HACS.WPF.ViewModels
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


		protected string ResetTrackedFlowCaption { get; set; } = "Reset Tracked Flow";
		protected override void StartContext()
		{
			base.StartContext();
			ContextStart.Add(new Context(ResetTrackedFlowCaption));
		}

		public override void Run(string command = "")
		{
			if (command == ResetTrackedFlowCaption)
				Component?.ResetTrackedFlow();
			base.Run(command);
		}


	}
}
