using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
	public class Xgs600 : HacsComponent
	{
		[Browsable(false)]
		public new Components.IXgs600 Component
		{
			get => base.Component as Components.IXgs600;
			protected set => base.Component = value;
		}

		// TODO 
		//public List<Meter> Gauges
		//{
		//	get
		//	{
		//		Component.Gauges.ToList<Meter>(x => );
		//	}
		//	set
		//	{ 
		//		gauges = value;
		//		var list = new List<Components.IMeter>();
		//		Component.Gauges = list;

		//	}
		//}
		//List<Meter> gauges;

		public Components.Xgs600.PressureUnits Units => Component.Units;
		public Components.Xgs600.PressureUnits TargetUnits { get => Component.TargetUnits; set => Component.TargetUnits = value; }
	}
}
