using System.Windows;

namespace AeonHacs.Wpf.Views;

public class VacuumSystem : Section
{
    static VacuumSystem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(VacuumSystem), new FrameworkPropertyMetadata(typeof(VacuumSystem)));
    }
}
