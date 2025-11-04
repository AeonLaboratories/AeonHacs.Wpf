using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AeonHacs.Wpf.ViewModels;

public class ProcessManager : HacsBase
{
    [Browsable(false)]
    public new Components.IProcessManager Component
    {
        get => base.Component as Components.IProcessManager;
        protected set => base.Component = value;
    }

    [Browsable(false)]
    public Dictionary<string, Components.Protocol> Protocols { get => Component.Protocols; set => Component.Protocols = value; }

    [Browsable(false)]
    public List<string> ProcessNames => Component.ProcessNames;

    public Components.ProcessManager.ProcessStateCode ProcessState => Component.ProcessState;
    public TimeSpan ProcessTime => Component.ProcessTime;
    public StatusChannel ProcessStep => Component.ProcessStep;
    public StatusChannel ProcessSubStep => Component.ProcessSubStep;

    [Editable(false)]
    public string ProcessToRun { get => Component.ProcessToRun; set => Component.ProcessToRun = value; }
    public Components.ProcessManager.ProcessTypeCode ProcessType => Component.ProcessType;
    public bool RunCompleted => Component.RunCompleted;
    public bool Busy => Component.Busy;

    // Context menu?
    //public void RunProcess(string processToRun);
    //public void AbortRunningProcess();

}
