using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class Meter : HacsDevice
	{
		[Browsable(false)]
		public new Components.IMeter Component
		{
			get => base.Component as Components.IMeter;
			protected set => base.Component = value;
		}
		public double Value => Component.Value;
		public string UnitSymbol { get => Component.UnitSymbol; set => Component.UnitSymbol = value; }
		public double Sensitivity { get => Component.Sensitivity; set => Component.Sensitivity = value; }
		public double Resolution { get => Component.Resolution; set => Component.Resolution = value; }
		public bool ResolutionIsProportional { get => Component.ResolutionIsProportional; set => Component.ResolutionIsProportional = value; }
		public Core.DigitalFilter Filter { get => Component.Filter; set => Component.Filter = value; }
		public Utilities.OperationSet Conversion { get => Component.Conversion; set => Component.Conversion = value; }
		public Core.RateOfChange RateOfChange { get => Component.RateOfChange; set => Component.RateOfChange = value; }
		public double Stable { get => Component.Stable; set => Component.Stable = value; }
		public bool IsStable => Component.IsStable;
		public double Falling { get => Component.Falling; set => Component.Falling = value; }
		public bool IsFalling => Component.IsFalling;
		public double Rising { get => Component.Rising; set => Component.Rising = value; }
		public bool IsRising => Component.IsRising;
		public bool OverRange => Component.OverRange;
		public bool UnderRange => Component.UnderRange;
		public bool Zeroing => Component.Zeroing;

		protected string ZeroNowCaption { get; set; } = "Zero now";
		protected override void StartContext()
		{
			base.StartContext();
			ContextStart.Add(new Context(ZeroNowCaption));
		}

		public override void Run(string command = "")
		{
			if (command == ZeroNowCaption)
				Component?.ZeroNow();
			base.Run(command);
		}
	}
}
