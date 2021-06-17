using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class MtiFurnace : TubeFurnace
	{
		[Browsable(false)]
		public new Components.IMtiFurnace Component
		{
			get => base.Component as Components.IMtiFurnace;
			protected set => base.Component = value;
		}
		public byte InstrumentId => Component.InstrumentId;
	}
}
