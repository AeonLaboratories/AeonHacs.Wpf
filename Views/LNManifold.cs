using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace AeonHacs.Wpf.Views;

[ContentProperty("Valves")]
public class LNManifold : View
{
    public static readonly DependencyProperty ValvesProperty = DependencyProperty.Register(
        nameof(Valves), typeof(ObservableCollection<Valve>), typeof(LNManifold), new UIPropertyMetadata(null));

    static LNManifold()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LNManifold), new FrameworkPropertyMetadata(typeof(LNManifold)));
    }
    
    public ObservableCollection<Valve> Valves { get => (ObservableCollection<Valve>)GetValue(ValvesProperty); set => SetValue(ValvesProperty, value); }

    public LNManifold() => SetValue(ValvesProperty, new ObservableCollection<Valve>());
}