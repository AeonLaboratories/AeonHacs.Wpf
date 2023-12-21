using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
	public class VTColdfinger : StateManager
	{
		[Browsable(false)]
		public new Components.IVTColdfinger Component
		{
			get => base.Component as Components.IVTColdfinger;
			protected set => base.Component = value;
		}

		public ViewModel Heater => GetFromModel(Component?.Heater);
		public ViewModel Coldfinger => GetFromModel(Component?.Coldfinger);
		public ViewModel TopThermometer => GetFromModel(Component?.TopThermometer);
		public ViewModel WireThermometer => GetFromModel(Component?.WireThermometer);

		public int WireTemperatureLimit { get => Component.WireTemperatureLimit; set => Component.WireTemperatureLimit = value; }
		public double Setpoint { get => Component.Setpoint; set => Component.Setpoint = value; }
		public double MaximumHeaterPower { get => Component.MaximumHeaterPower; set => Component.MaximumHeaterPower = value; }

		public PidSetup HeaterPid
		{
			get => heaterPid;
			set { heaterPid = value; Component.HeaterPid = heaterPid?.Component; }
		}
		PidSetup heaterPid;

		public double MaximumWarmHeaterPower { get => Component.MaximumWarmHeaterPower; set => Component.MaximumWarmHeaterPower = value; }

		public PidSetup WarmHeaterPid
		{
			get => warmHeaterPid;
			set { warmHeaterPid = value; Component.WarmHeaterPid = warmHeaterPid?.Component; }
		}
		PidSetup warmHeaterPid;


		public int ColdTemperature { get => Component.ColdTemperature; set => Component.ColdTemperature = value; }

		public int CleanupTemperature { get => Component.CleanupTemperature; set => Component.CleanupTemperature = value; }

		public string HeaterOnTrickle { get => Component.HeaterOnTrickle; set => Component.HeaterOnTrickle = value; }

		public string HeaterOffTrickle { get => Component.HeaterOffTrickle; set => Component.HeaterOffTrickle = value; }

		public bool Frozen => Component.Frozen;

		public double Temperature => Component.Temperature;

		public double ColdfingerTemperature => Component.ColdfingerTemperature;

		public bool IsOn => Component.IsOn;

		public AeonHacs.OnOffState OnOffState => Component.OnOffState;

		public AeonHacs.StopAction StopAction { get => Component.StopAction; set => Component.StopAction = value; }


		protected string StandbyCaption { get; set; } = "Standby";
		protected string ThawCaption { get; set; } = "Thaw";
		protected string FreezeCaption { get; set; } = "Freeze";
		protected string RegulateCaption { get; set; } = "Regulate";
		protected override void StartContext()
		{
			ContextStart.Clear();
			ContextStart.Add(new Context(StandbyCaption));
			ContextStart.Add(new Context(ThawCaption));
			ContextStart.Add(new Context(FreezeCaption));
			ContextStart.Add(new Context(RegulateCaption));
		}

		public override void Run(string command = "")
		{
			if (command == StandbyCaption)
				Component?.Standby();
			else if (command == ThawCaption)
				Component?.Thaw();
			else if (command == FreezeCaption)
				Component?.Freeze();
			else if (command == RegulateCaption)
				Component?.Regulate();
		}

	}
}
