using AeonHacs.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace AeonHacs.Wpf.Data;
public class ParametersCard : Control
{
    public static readonly ICommand AddParameter = new RoutedUICommand("Add", nameof(AddParameter), typeof(ParametersCard));
    public static readonly ICommand RemoveParameter = new RoutedUICommand("Remove", nameof(RemoveParameter), typeof(ParametersCard));

    public static IEnumerable<Parameter> DefaultParameters { get; }

    public static readonly DependencyProperty DisplayNameProperty = PropertyCard.DisplayNameProperty.AddOwner(typeof(ParametersCard));

    public static readonly DependencyProperty ParametersProperty = DependencyProperty.Register(
        nameof(Parameters),
        typeof(ObservableCollection<Parameter>),
        typeof(ParametersCard)
    );

    static ParametersCard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ParametersCard), new FrameworkPropertyMetadata(typeof(ParametersCard)));
        DefaultParameters = NamedObject.FindAll<CegsPreferences>().FirstOrDefault()?.DefaultParameters;
    }

    public string DisplayName
    {
        get => (string)GetValue(DisplayNameProperty);
        set => SetValue(DisplayNameProperty, value);
    }

    public ObservableCollection<Parameter> Parameters
    {
        get => (ObservableCollection<Parameter>)GetValue(ParametersProperty);
        set => SetValue(ParametersProperty, value);
    }

    public ParametersCard()
    {
        InitializeCommands();

        Parameters = new();

        AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnButtonClick));
        AddHandler(Selector.SelectionChangedEvent, new SelectionChangedEventHandler(ParameterChanged));
    }

    private void ParameterChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.RemovedItems.Count > 0 && e.OriginalSource is ComboBox cb && cb.DataContext is Parameter p)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Parameter newParam)
                p.Value = newParam.Value;
        }
    }

    protected virtual void InitializeCommands()
    {
        CommandBindings.Add(new CommandBinding(AddParameter, Add));
        CommandBindings.Add(new CommandBinding(RemoveParameter, Remove));
    }

    private void Add(object sender, ExecutedRoutedEventArgs e)
    {
        var defaultParameter = DefaultParameters.FirstOrDefault();

        var newParam = new Parameter()
        {
            ParameterName = defaultParameter.ParameterName,
            Value = defaultParameter.Value
        };

        Parameters.Add(newParam);
    }

    private void Remove(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is Parameter parameter)
            Parameters.Remove(parameter);
    }

    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        ;
    }
}
