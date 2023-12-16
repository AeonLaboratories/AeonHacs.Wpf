using System.ComponentModel;
using System.Collections.Generic;

namespace HACS.WPF.ViewModels
{
	public class ProcessSequence : HacsComponent
	{
		[Browsable(false)]
		public new Components.IProcessSequence Component
		{
			get => base.Component as Components.IProcessSequence;
			protected set => base.Component = value;
		}
		public HACS.Core.InletPortType PortType { get => Component.PortType; set => Component.PortType = value; }
		public List<string> CheckList { get => Component.CheckList; set => Component.CheckList = value; }
		public List<Components.ProcessSequenceStep> Steps { get => Component.Steps; set => Component.Steps = value; }
	}
}
