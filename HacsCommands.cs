using System.Windows.Input;

namespace AeonHacs.Wpf;

public class HacsCommands
{
    public static readonly RoutedUICommand EditSample = new("Edit Sample", nameof(EditSample), typeof(HacsCommands));
}
