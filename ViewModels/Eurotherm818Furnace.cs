using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class Eurotherm818Furnace : TubeFurnace
	{
		[Browsable(false)]
		public new Components.IEurotherm818Furnace Component
		{
			get => base.Component as Components.IEurotherm818Furnace;
			protected set => base.Component = value;
		}
		public string InstrumentId { get => Component.InstrumentId; set => Component.InstrumentId = value; }
		public int OutputPowerLimit { get => Component.OutputPowerLimit; set => Component.OutputPowerLimit = value; }
		public int Error => Component.Error;
		public int WorkingSetpoint => Component.WorkingSetpoint;
		public int OutputPower => Component.OutputPower;

	}
}
