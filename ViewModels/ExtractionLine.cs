using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
    public class ExtractionLine : ProcessManager
    {
        [Browsable(false)]
        public new Components.IExtractionLine Component
        {
            get => base.Component as Components.IExtractionLine;
            protected set => base.Component = value;
        }
        // TODO implementation deferred
    }
}
