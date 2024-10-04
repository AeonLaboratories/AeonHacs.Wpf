using AeonHacs.Components;
using AeonHacs.Wpf.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AeonHacs.Wpf.Data;

public class AliquotsCard : PropertyCard
{
    public static readonly RoutedUICommand AddAliquot = new("Add Aliquot", nameof(AddAliquot), typeof(AliquotsCard));
    public static readonly RoutedUICommand RemoveAliquot = new("Remove Aliquot", nameof(RemoveAliquot), typeof(AliquotsCard));

    public static readonly DependencyProperty IDsProperty = DependencyProperty.Register(
        nameof(IDs),
        typeof(ObservableCollection<AliquotId>),
        typeof(AliquotsCard)
    );

    static AliquotsCard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(AliquotsCard), new FrameworkPropertyMetadata(typeof(AliquotsCard)));
    }

    public ObservableCollection<AliquotId> IDs
    {
        get => (ObservableCollection<AliquotId>)GetValue(IDsProperty);
        set => SetValue(IDsProperty, value);
    }

    public AliquotsCard()
    {
        InitializeCommands();
    }

    protected virtual void InitializeCommands()
    {
        CommandBindings.Add(new(AddAliquot, (_, _) => IDs.Add(new AliquotId()), CanAddAliquot));
        CommandBindings.Add(new(RemoveAliquot, Remove));
    }

    private void Remove(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is AliquotId id)
        {
            if (IDs.Count == 1)
                id.Id = "";
            else
                IDs.Remove(id);
        }
    }

    private void CanAddAliquot(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = IDs != null && IDs.Count < CegsPreferences.MaximumAliquotsPerSample;
    }
}
