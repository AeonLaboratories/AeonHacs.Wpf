using AeonHacs.Components;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AeonHacs.Wpf.Data;

public class AliquotsCard : PropertyCard
{
    public static readonly RoutedUICommand PasteAliquots = new("Paste Aliquots", nameof(PasteAliquots), typeof(AliquotsCard));
    public static readonly RoutedUICommand AddAliquot = new("Add Aliquot", nameof(AddAliquot), typeof(AliquotsCard));
    public static readonly RoutedUICommand RemoveAliquot = new("Remove Aliquot", nameof(RemoveAliquot), typeof(AliquotsCard));

    public static readonly DependencyProperty MaximumAliquotsProperty = DependencyProperty.Register(
        nameof(MaximumAliquots),
        typeof(int),
        typeof(AliquotsCard),
        new PropertyMetadata(CegsPreferences.MaximumAliquotsPerSample)
    );

    public static readonly DependencyProperty IDsProperty = DependencyProperty.Register(
        nameof(IDs),
        typeof(ObservableCollection<AliquotId>),
        typeof(AliquotsCard)
    );

    static AliquotsCard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(AliquotsCard), new FrameworkPropertyMetadata(typeof(AliquotsCard)));
    }

    public int MaximumAliquots
    {
        get => (int)GetValue(MaximumAliquotsProperty);
        set => SetValue(MaximumAliquotsProperty, value);
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
        CommandBindings.Add(new(PasteAliquots, Paste));
        CommandBindings.Add(new(AddAliquot, (_, _) => IDs.Add(new AliquotId()), CanAddAliquot));
        CommandBindings.Add(new(RemoveAliquot, Remove));
    }

    private void Paste(object sender, ExecutedRoutedEventArgs e)
    {
        if (!Clipboard.ContainsText(TextDataFormat.Text))
            return;
        var ids = Clipboard.GetText().Split(Environment.NewLine);
        var index = Convert.ToInt32(e.Parameter);
        foreach (var id in ids)
        {
            if (index >= CegsPreferences.MaximumAliquotsPerSample)
                break;

            if (index >= IDs.Count)
                IDs.Add(new AliquotId() { Id = id });
            else
                IDs[index].Id = id;

            index++;
        }

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
        if (IDs == null || CegsPreferences.MaximumAliquotsPerSample <= 0)
        {
            e.CanExecute = false;
            return;
        }

        // TODO providing notifications for CegsPreferences Changes would be better
        if (IDs.Count < CegsPreferences.MaximumAliquotsPerSample)
            e.CanExecute = true;
        else
        {
            while (IDs.Count > CegsPreferences.MaximumAliquotsPerSample)
                IDs.RemoveAt(IDs.Count - 1);
            e.CanExecute = false;
        }
    }
}
