using System.ComponentModel;

namespace HACS.WPF.ViewModels
{
	public class HacsBase : HacsComponent
	{
		[Browsable(false)]
		public new Core.IHacsBase Component
		{
			get => base.Component as Core.IHacsBase;
			protected set => base.Component = value;
		}		
	}
}
