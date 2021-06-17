using System.Collections.Generic;
using System.ComponentModel;

namespace HACS.WPF.ViewModels
{
	public class Section : Chamber
	{
		[Browsable(false)]
		public new Components.ISection Component
		{
			get => base.Component as Components.ISection;
			protected set => base.Component = value;
		}


		public List<ViewModel> Chambers
		{
			get => Component?.Chambers?.ConvertAll(x => GetFromModel(x));
			set { if (Component != null) Component.Chambers = value?.ConvertAll(x => x.Component as Components.IChamber); }
		}

		public List<ViewModel> Ports
		{
			get => Component?.Ports?.ConvertAll(x => GetFromModel(x));
			set { if (Component != null) Component.Ports = value?.ConvertAll(x => x.Component as Components.IPort); }
		}

		public List<ViewModel> PathToVacuum
		{
			get => Component?.PathToVacuum?.ConvertAll(x => GetFromModel(x));
			set { if (Component != null) Component.PathToVacuum = value?.ConvertAll(x => x.Component as Components.IValve); }
		}

		public List<ViewModel> Isolation
		{
			get => Component?.Isolation?.ConvertAll(x => GetFromModel(x));
			set { if (Component != null) Component.Isolation = value?.ConvertAll(x => x.Component as Components.IValve); }
		}

		public List<ViewModel> InternalValves
		{
			get => Component?.InternalValves?.ConvertAll(x => GetFromModel(x));
			set { if (Component != null) Component.InternalValves = value?.ConvertAll(x => x.Component as Components.IValve); }
		}

		public ViewModel VacuumSystem => GetFromModel(Component?.VacuumSystem);

		public new double MilliLiters => Component.MilliLiters;
		public ViewModel FlowValve => GetFromModel(Component?.FlowValve);
		public ViewModel FlowManager => GetFromModel(Component?.FlowManager);
		public bool IsOpened => Component.IsOpened;

		protected string IsolateCaption { get; set; } = "Isolate";
		protected string EvacuateCaption { get; set; } = "Evacuate";
		protected string OpenAndEvacuateCaption { get; set; } = "Open and Evacuate";
		protected string OpenCaption { get; set; } = "Open";
		protected string CloseCaption { get; set; } = "Close";
		protected string OpenPortsCaption { get; set; } = "Open ports";
		protected string ClosePortsCaption { get; set; } = "Close ports";
		protected string IsolateFromVacuumCaption { get; set; } = "Isolate from vacuum";
		protected string JoinToVacuumCaption { get; set; } = "Join to vacuum";

		protected override void StartContext()
		{
			ContextStart.Clear();
			ContextStart.Add(new Context(IsolateCaption));
			ContextStart.Add(new Context(EvacuateCaption));
			ContextStart.Add(new Context(OpenAndEvacuateCaption));
			ContextStart.Add(new Context(OpenCaption));
			ContextStart.Add(new Context(CloseCaption));
			ContextStart.Add(new Context(OpenPortsCaption));
			ContextStart.Add(new Context(ClosePortsCaption));
			ContextStart.Add(new Context(IsolateFromVacuumCaption));
			ContextStart.Add(new Context(JoinToVacuumCaption));
		}

		public override void Run(string command = "")
		{
			if (command == IsolateCaption)
				Component?.Isolate();
			else if (command == EvacuateCaption)
				Component?.Evacuate();
			else if (command == OpenAndEvacuateCaption)
				Component?.OpenAndEvacuate();
			else if (command == OpenCaption)
				Component?.Open();
			else if (command == CloseCaption)
				Component?.Close();
			else if (command == OpenPortsCaption)
				Component?.OpenPorts();
			else if (command == ClosePortsCaption)
				Component?.ClosePorts();
			else if (command == IsolateFromVacuumCaption)
				Component?.IsolateFromVacuum();
			else if (command == JoinToVacuumCaption)
				Component?.JoinToVacuum();
			base.Run(command);
		}

	}
}
