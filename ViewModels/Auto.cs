using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class Auto : Switch
	{
		[Browsable(false)]
		public new Components.IAuto Component
		{
			get => base.Component as Components.IAuto;
			protected set => base.Component = value;
		}
		public double Setpoint => Component.Setpoint;
		public double TargetSetpoint { get => Component.Config.Setpoint; set => Component.Setpoint = value; }
		//void TurnOn(double setpoint);
	}
}
