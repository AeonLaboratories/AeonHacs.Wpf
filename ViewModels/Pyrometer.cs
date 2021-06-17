using System.Collections.Generic;
using System.ComponentModel;

namespace HACS.WPF.ViewModels
{
	public class Pyrometer : Thermometer
	{
		[Browsable(false)]
		public new Components.IPyrometer Component
		{
			get => base.Component as Components.IPyrometer;
			protected set => base.Component = value;
		}
		public byte Address  { get => Component.Address; set => Component.Address = value; }
		public int LaserCooldownSeconds  { get => Component.LaserCooldownSeconds; set => Component.LaserCooldownSeconds = value; }
		public int LaserOnMaxSeconds  { get => Component.LaserOnMaxSeconds; set => Component.LaserOnMaxSeconds = value; }
		public double Emissivity  { get => Component.Emissivity; set => Component.Emissivity = value; }
		public double RatioCorrection  { get => Component.RatioCorrection; set => Component.RatioCorrection = value; }
		public double Transmission  { get => Component.Transmission; set => Component.Transmission = value; }
		public Components.Pyrometer.TimeCode ResponseTime { get => Component.ResponseTime; set => Component.ResponseTime = value; }
		public double MillimetersMeasuringDistance  { get => Component.MillimetersMeasuringDistance; set => Component.MillimetersMeasuringDistance = value; }
		public double TemperatureRangeMinimum => Component.TemperatureRangeMinimum;
		public double TemperatureRangeMaximum => Component.TemperatureRangeMaximum;
		public double MillimetersFocalLength => Component.MillimetersFocalLength;
		public double MillimetersFieldDiameterMinimum  { get => Component.MillimetersFieldDiameterMinimum; set => Component.MillimetersFieldDiameterMinimum = value; }
		public double MillimetersAperture  { get => Component.MillimetersAperture; set => Component.MillimetersAperture = value; }
		public int StatusByte => Component.StatusByte;
		public double InternalTemperature => Component.InternalTemperature;
		public double MeasuringFieldDiameter => Component.MeasuringFieldDiameter;

		public Core.OnOffState OnOffState => Component.OnOffState;
		public bool IsOn => Component.IsOn;
		public bool IsOff => Component.IsOff;
		public long MillisecondsOn => Component.MillisecondsOn;
		public long MillisecondsOff => Component.MillisecondsOff;
		public long MillisecondsInState => Component.MillisecondsInState;

		public Core.StopAction StopAction { get => Component.StopAction; set => Component.StopAction = value; }

		public ViewModel SerialController => GetFromModel(Component.SerialController);

		protected string TurnOnCaption  { get; set; } = "Turn laser on";
		protected string TurnOffCaption { get; set; } = "Turn laser off";
		public override List<Context> Context()
		{
			var valid = new List<Context>(base.Context());
			valid.Add(new Context(Component.IsOn ? TurnOffCaption : TurnOnCaption));
			return valid;
		}
		public override void Run(string command = "")
		{
			if (command == TurnOnCaption)
				Component?.TurnOn();
			else if (command == TurnOffCaption)
				Component?.TurnOff();
			base.Run(command);
		}
	}
}
