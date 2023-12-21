using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
	public class NamedObject : ViewModel
	{
		[Browsable(false)]
		public new AeonHacs.INamedObject Component
		{
			get => base.Component as AeonHacs.INamedObject;
			protected set => base.Component = value;
		}
    }
}
