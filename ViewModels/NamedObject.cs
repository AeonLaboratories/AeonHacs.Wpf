using System.ComponentModel;

namespace HACS.WPF.ViewModels
{
	public class NamedObject : ViewModel
	{
		[Browsable(false)]
		public new Core.INamedObject Component
		{
			get => base.Component as Core.INamedObject;
			protected set => base.Component = value;
		}
    }
}
