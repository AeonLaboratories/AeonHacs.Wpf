using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class CpwActuator : Actuator
	{
		[Browsable(false)]
		public new Components.ICpwActuator Component
		{
			get => base.Component as Components.ICpwActuator;
			protected set => base.Component = value;
		}
		public bool Configured => Component.Configured;
		public bool ControlPulseEnabled => Component.ControlPulseEnabled;
		public bool PositionDetectable => Component.PositionDetectable;
		public bool LimitSwitch0Engaged => Component.LimitSwitch0Engaged;
		public bool LimitSwitch1Engaged => Component.LimitSwitch1Engaged;
		public bool LimitSwitchDetected => Component.LimitSwitchDetected;
		public int Current => Component.Current;
		public bool CurrentLimitDetected => Component.CurrentLimitDetected;
		public int IdleCurrentLimit { get => Component.IdleCurrentLimit; set => Component.IdleCurrentLimit = value; }
		public Components.ActuatorController.ErrorCodes Errors => Component.Errors;
	}
}
