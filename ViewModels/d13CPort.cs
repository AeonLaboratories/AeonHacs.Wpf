using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
    public class d13CPort : LinePort 
    {
        [Browsable(false)]
        public new Components.Id13CPort Component
        {
            get => base.Component as Components.Id13CPort;
            protected set => base.Component = value;
        }
    }
}
