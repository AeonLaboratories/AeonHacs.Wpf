using System.ComponentModel;
using System;
using System.Collections.Generic;
using Utilities;
using System.ComponentModel.DataAnnotations;

namespace HACS.WPF.ViewModels
{
	public class ProcessManager : HacsBase
	{
		[Browsable(false)]
		public new Components.IProcessManager Component
		{
			get => base.Component as Components.IProcessManager;
			protected set => base.Component = value;
		}

		[Browsable(false)]
		public Dictionary<string, Components.ProcessSequence> ProcessSequences { get => Component.ProcessSequences; set => Component.ProcessSequences = value; }

		[Browsable(false)]
		public List<string> ProcessNames => Component.ProcessNames;

		public Components.ProcessManager.ProcessStateCode ProcessState => Component.ProcessState;
		public TimeSpan ProcessTime => Component.ProcessTime;
		public StepTracker ProcessStep => Component.ProcessStep;
		public StepTracker ProcessSubStep => Component.ProcessSubStep;

		[Editable(false)]
		public string ProcessToRun { get => Component.ProcessToRun; set => Component.ProcessToRun = value; }
		public Components.ProcessManager.ProcessTypeCode ProcessType => Component.ProcessType;
		public bool RunCompleted => Component.RunCompleted;
		public bool Busy => Component.Busy;

		// Context menu?
		//public void RunProcess(string processToRun);
		//public void AbortRunningProcess();

	}
}
