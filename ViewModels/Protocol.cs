using System.ComponentModel;
using System.Collections.Generic;

namespace AeonHacs.Wpf.ViewModels;

public class Protocol : HacsComponent
{
    [Browsable(false)]
    public new Components.IProtocol Component
    {
        get => base.Component as Components.IProtocol;
        protected set => base.Component = value;
    }
    public AeonHacs.InletPortType PortType { get => Component.PortType; set => Component.PortType = value; }
    public List<string> CheckList { get => Component.CheckList; set => Component.CheckList = value; }
    public List<Components.ProtocolStep> Steps { get => Component.Steps; set => Component.Steps = value; }
}
