using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class HC6ControllerB2 : SerialDeviceManager
	{
		[Browsable(false)]
		public new Components.IHC6ControllerB2 Component
		{
			get => base.Component as Components.IHC6ControllerB2;
			protected set => base.Component = value;
		}
		public bool InterferenceSuppressionEnabled { get => Component.InterferenceSuppressionEnabled; set => Component.InterferenceSuppressionEnabled = value; }
		public string Model => Component.Model;
		public string Firmware => Component.Firmware;
		public int SerialNumber => Component.SerialNumber;
		public int SelectedHeater => Component.SelectedHeater;
		public int SelectedThermocouple => Component.SelectedThermocouple;
		public int AdcCount => Component.AdcCount;
		public double ColdJunction0Temperature => Component.ColdJunction0Temperature;
		public double ColdJunction1Temperature => Component.ColdJunction1Temperature;
		public double ReadingRate => Component.ReadingRate;
		public Components.HC6ControllerB2.ErrorCodes Errors => Component.Errors;
	}
}
