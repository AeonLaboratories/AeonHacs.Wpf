namespace HACS.WPF.ViewModels
{
	public class Img100 : SwitchedManometer
	{
		public new Components.IImg100 Component
		{
			get => base.Component as Components.IImg100;
			protected set => base.Component = value;
		}
		// TODO
		//Xgs600 Controller { get; set; }
		//string UserLabel { get; set; }
	}
}
