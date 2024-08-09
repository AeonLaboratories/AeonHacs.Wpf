using AeonHacs.Wpf.Behaviors;
using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace AeonHacs.Wpf.Views;
public class SetpointAdorner : Adorner
{
    public static readonly DependencyProperty ComponentProperty = View.ComponentProperty.AddOwner(typeof(SetpointAdorner),
        new FrameworkPropertyMetadata() { PropertyChangedCallback = ComponentChanged });

    private static void ComponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SetpointAdorner adorner)
            adorner.UpdateBindings();
    }

    public static readonly DependencyProperty TargetManualModeProperty = DependencyProperty.Register(
        "TargetManualMode", typeof(bool?), typeof(SetpointAdorner), new FrameworkPropertyMetadata(null, TargetManualModeChanged));

    private static void TargetManualModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SetpointAdorner adorner)
            adorner.UpdateTextBoxBinding();
    }

    public static readonly DependencyProperty TargetSetpointProperty = DependencyProperty.Register(
        "TargetSetpoint", typeof(double?), typeof(SetpointAdorner), new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty TargetPowerLevelProperty = DependencyProperty.Register(
        "TargetPowerLevel", typeof(double?), typeof(SetpointAdorner), new FrameworkPropertyMetadata(null));

    public INotifyPropertyChanged Component  { get => (INotifyPropertyChanged)GetValue(ComponentProperty); set => SetValue(ComponentProperty, value); }

    public bool? TargetManualMode { get => (bool?)GetValue(TargetManualModeProperty); set => SetValue(TargetManualModeProperty, value); }

    public double? TargetSetpoint { get => (double?)GetValue(TargetSetpointProperty); set => SetValue(TargetSetpointProperty, value); }

    public double? TargetPowerLevel { get => (double?)GetValue(TargetPowerLevelProperty); set => SetValue(TargetPowerLevelProperty, value); }

    VisualCollection visualChildren;
    TextBox setpointTextBox;

    protected override int VisualChildrenCount => 1;

    private bool valid = false;

    public SetpointAdorner(Gauge adornedElement) : base(adornedElement)
    {
        setpointTextBox = new TextBox()
        {
            Visibility = Visibility.Collapsed,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        setpointTextBox.LostFocus += (s, e) => setpointTextBox.Visibility = Visibility.Collapsed;
        Interaction.GetBehaviors(setpointTextBox).Add(new TextBoxExitKeyBehavior());

        var componentBinding = new Binding() { Source = adornedElement, Path = new PropertyPath(View.ComponentProperty) };
        setpointTextBox.SetBinding(View.ComponentProperty, componentBinding);
        SetBinding(ComponentProperty, componentBinding);

        visualChildren = new VisualCollection(this);

        adornedElement.PreviewMouseLeftButtonDown += (s, e) =>
        {
            if (e.ClickCount == 2 && valid)
            {
                setpointTextBox.Visibility = Visibility.Visible;
                setpointTextBox.Focus();
                setpointTextBox.SelectAll();
                e.Handled = true;
            }
        };

        visualChildren = new VisualCollection(this)
        {
            setpointTextBox
        };
    }

    protected virtual void UpdateBindings()
    {
        string redirect = Component is ViewModels.VTColdfinger ? ".Heater" : "";

        var targetManualModeBinding = new Binding() { Source = AdornedElement, Path = new PropertyPath($"Component{redirect}.TargetManualMode") };
        SetBinding(TargetManualModeProperty, targetManualModeBinding);

        var targetSetpointBinding = new Binding() { Source = AdornedElement, Path = new PropertyPath($"Component{redirect}.TargetSetpoint") };
        SetBinding(TargetSetpointProperty, targetSetpointBinding);

        var targetPowerLevelBinding = new Binding() { Source = AdornedElement, Path = new PropertyPath($"Component{redirect}.TargetPowerLevel") };
        SetBinding(TargetPowerLevelProperty, targetPowerLevelBinding);

        UpdateTextBoxBinding();
    }

    protected virtual void UpdateTextBoxBinding()
    {
        Binding textBinding;
        string redirect = Component is ViewModels.VTColdfinger ? ".Heater" : "";

        if (TargetManualMode is bool targetManualMode)
            textBinding = new Binding() { Source = AdornedElement, Path = new PropertyPath(targetManualMode ? $"Component{redirect}.TargetPowerLevel" : $"Component{redirect}.TargetSetpoint") };
        else if (TargetSetpoint is double targetSetpoint)
            textBinding = new Binding() { Source = AdornedElement, Path = new PropertyPath($"Component{redirect}.TargetSetpoint") };
        else if (TargetPowerLevel is double targetPowerLevel)
            textBinding = new Binding() { Source = AdornedElement, Path = new PropertyPath($"Component{redirect}.TargetPowerLevel") };
        else
        {
            BindingOperations.ClearBinding(setpointTextBox, TextBox.TextProperty);
            valid = false;
            return;
        }
        
        setpointTextBox.SetBinding(TextBox.TextProperty, textBinding);
        valid = true;
    }


    protected override Visual GetVisualChild(int index)
    {
        return visualChildren[index];
    }

    protected override Size MeasureOverride(Size constraint)
    {
        setpointTextBox.Measure(AdornedElement.RenderSize);
        return base.MeasureOverride(constraint);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        setpointTextBox.Arrange(new Rect(finalSize));
        return base.ArrangeOverride(finalSize);
    }
}
