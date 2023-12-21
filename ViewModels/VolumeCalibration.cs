using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
	public class VolumeCalibration : HacsComponent
	{
		[Browsable(false)]
		public new Components.IVolumeCalibration Component
		{
			get => base.Component as Components.IVolumeCalibration;
			protected set => base.Component = value;
		}
		//public GasSupply GasSupply { get => Component.GasSupply; set => Component.GasSupply = value; }

		public double CalibrationPressure { get => Component.CalibrationPressure; set => Component.CalibrationPressure = value; }

		public int CalibrationMinutes { get => Component.CalibrationMinutes; set => Component.CalibrationMinutes = value; }

		//public List<VolumeExpansion> Expansions { get => Component.Expansions; set => Component.Expansions = value; }

		public bool ExpansionVolumeIsKnown { get => Component.ExpansionVolumeIsKnown; set => Component.ExpansionVolumeIsKnown = value; }

		public double OkPressure { get => Component.OkPressure; set => Component.OkPressure = value; }

		//public Meter Measurement { get => Component.Measurement; set => Component.Measurement = value; }

		//public HacsLog Log { get => Component.Log; set => Component.Log = value; }
	}
}
