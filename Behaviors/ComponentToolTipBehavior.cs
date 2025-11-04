using AeonHacs.Wpf.Converters;
using AeonHacs.Wpf.ViewModels;
using AeonHacs.Wpf.Views;
using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace AeonHacs.Wpf.Behaviors;

public class ComponentToolTipBehavior : Behavior<Window>
{
    ToolTip toolTip = new ToolTip() { Placement = System.Windows.Controls.Primitives.PlacementMode.Relative };
    DispatcherTimer updateTimer = new DispatcherTimer(DispatcherPriority.DataBind) { Interval = TimeSpan.FromMilliseconds(250) };
    
    private bool HasContent() =>
        toolTip.Content is string s && !string.IsNullOrWhiteSpace(s);

    protected override void OnAttached()
    {
        toolTip.PlacementTarget = AssociatedObject;
        toolTip.Closed += (s, e) => View.SetComponent(AssociatedObject, null);
        
        var toolTipBinding = new Binding()
        {
            Path = new PropertyPath(View.ComponentProperty),
            Source = AssociatedObject,
            Converter = new ToStringConverter()
        };

        toolTip.SetBinding(ContentControl.ContentProperty, toolTipBinding);
        var toolTipBindingExpression = BindingOperations.GetBindingExpression(toolTip, ContentControl.ContentProperty);

        updateTimer.Tick += (s, e) =>
        {
            toolTipBindingExpression?.UpdateTarget();

            // Close tooltip if content became empty, open if it became non-empty
            if (toolTip.IsOpen && !HasContent())
                HideToolTip();
            else if (!toolTip.IsOpen && HasContent())
                ShowToolTip();
        };

        AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
        AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;

        base.OnAttached();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
        AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;

        HideToolTip();
        BindingOperations.ClearBinding(toolTip, ContentControl.ContentProperty);
    }

    protected virtual void ShowToolTip()
    {
        if (!toolTip.IsOpen && HasContent())
        {
            toolTip.IsOpen = true;
            updateTimer.Start();
        }
    }

    protected virtual void HideToolTip()
    {
        if (toolTip.IsOpen)
        {
            toolTip.IsOpen = false;
            updateTimer.Stop();
        }
    }

    private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        string helpText = "";
        HitTestResultBehavior Test(FrameworkElement element)
        {
            if (element == AssociatedObject)
            {
                HideToolTip();
                return HitTestResultBehavior.Stop;
            }
            if (View.GetComponent(element) is INotifyPropertyChanged component)
            {
                View.SetComponent(AssociatedObject, component);
                var expr = BindingOperations.GetBindingExpression(toolTip, ContentControl.ContentProperty);
                expr?.UpdateTarget();

                if (HasContent())
                    ShowToolTip();
                else
                    HideToolTip();

                if (helpText.IsBlank())
                {
                    if (component is ViewModel vm)
                        helpText = vm.Description;
                    else if (component is NamedObject no)
                        helpText = no.Description;
                }

                ShowToolTip();
                return HitTestResultBehavior.Stop;
            }
            if (helpText.IsBlank() && element.GetValue(AutomationProperties.HelpTextProperty) is string ht)
                helpText = ht;
            return HitTestResultBehavior.Continue;
        }

        var mousePos = e.GetPosition(AssociatedObject);
        VisualTreeHelper.HitTest(AssociatedObject,
            null,
            r =>
            {
                if (r.VisualHit is not FrameworkElement element || element.IsHitTestVisible == false)
                    return HitTestResultBehavior.Continue;
                if (Test(element) == HitTestResultBehavior.Stop)
                    return HitTestResultBehavior.Stop;
                if (element.TemplatedParent is FrameworkElement parent)
                    return Test(parent);
                return HitTestResultBehavior.Continue;
            },
            new PointHitTestParameters(mousePos)
        );
        AutomationProperties.SetHelpText(AssociatedObject, helpText ?? "");
        if (toolTip.IsOpen)
        {
            toolTip.HorizontalOffset = mousePos.X + SystemParameters.CursorWidth / 2;
            toolTip.VerticalOffset = mousePos.Y + SystemParameters.CursorHeight / 2 + 5;
        }
    }

    private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
    {
        HideToolTip();
        AutomationProperties.SetHelpText(AssociatedObject, "");
    }
}
