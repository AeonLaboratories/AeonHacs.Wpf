using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class ExtractionLine : ProcessManager
	{
		[Browsable(false)]
		public new Components.IExtractionLine Component
		{
			get => base.Component as Components.IExtractionLine;
			protected set => base.Component = value;
		}
		// TODO implementation deferred
	}
}
