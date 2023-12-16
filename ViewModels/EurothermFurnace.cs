using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class EurothermFurnace : TubeFurnace
	{
		[Browsable(false)]
		public new Components.IEurothermFurnace Component
		{
			get => base.Component as Components.IEurothermFurnace;
			protected set => base.Component = value;
		}
		public int WorkingOutput => Component.WorkingOutput;
		public int ControlOutput { get => Component.ControlOutput; set => Component.ControlOutput = value; }
		public int SetpointRateLimit { get => Component.SetpointRateLimit; set => Component.SetpointRateLimit = value; }
		public Components.EurothermFurnace.SetpointRateLimitUnitsCode SetpointRateLimitUnits { get => Component.SetpointRateLimitUnits; set => Component.SetpointRateLimitUnits = value; }
		public int OutputRateLimit { get => Component.OutputRateLimit; set => Component.OutputRateLimit = value; }
		public Components.EurothermFurnace.AutoManualCode OperatingMode => Component.OperatingMode;
		public bool ContactorDisengaged { get => Component.ContactorDisengaged; set => Component.ContactorDisengaged = value; }
	}
}
