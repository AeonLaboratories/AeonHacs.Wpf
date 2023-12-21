using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
	public class HacsBase : HacsComponent
	{
		[Browsable(false)]
		public new AeonHacs.IHacsBase Component
		{
			get => base.Component as AeonHacs.IHacsBase;
			protected set => base.Component = value;
		}		
	}
}
