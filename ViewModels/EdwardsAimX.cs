using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class EdwardsAimX : SwitchedManometer
	{
		[Browsable(false)]
		public new Components.IEdwardsAimX Component
		{
			get => base.Component as Components.IEdwardsAimX;
			protected set => base.Component = value;
		}
		public Core.AnalogInputMode AnalogInputMode { get => Component.AnalogInputMode; set => Component.AnalogInputMode = value; }
		public double MaximumVoltage { get => Component.MaximumVoltage; set => Component.MaximumVoltage = value; }
		public double MinimumVoltage { get => Component.MinimumVoltage; set => Component.MinimumVoltage = value; }

		public ViewModel Manager => GetFromModel(Component.Manager);
	}
}
