using System.ComponentModel;
namespace AeonHacs.Wpf.ViewModels
{
    public class StateManager : HacsComponent
    {
        [Browsable(false)]
        public new Components.IStateManager Component
        {
            get => base.Component as Components.IStateManager;
            protected set => base.Component = value;
        }

        public bool Ready => Component.Ready;
        public bool HasWork => Component.HasWork;
        public bool Busy => Component.Busy;
        public bool Stopping => Component.Stopping;
        public bool LogEverything { get => Component.LogEverything; set => Component.LogEverything = value; }
        public int IdleTimeout { get => Component.IdleTimeout; set => Component.IdleTimeout = value; }
        public int StateLoopTimeout { get => Component.StateLoopTimeout; set => Component.StateLoopTimeout = value; }
    }
}
