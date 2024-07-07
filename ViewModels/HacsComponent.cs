using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels
{
    public class HacsComponent : ViewModel
    {
        [Browsable(false)]
        public new AeonHacs.IHacsComponent Component
        {
            get => base.Component as AeonHacs.IHacsComponent;
            protected set => base.Component = value;
        }
        public bool Connected => Component.Connected;
        public bool Initialized => Component.Initialized;
        public bool Started => Component.Started;
        public bool Stopped => Component.Stopped;
    }

}
