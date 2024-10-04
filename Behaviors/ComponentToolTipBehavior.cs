using AeonHacs.Wpf.Converters;
using AeonHacs.Wpf.Views;
using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Windows;
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

        updateTimer.Tick += (s, e) => toolTipBindingExpression?.UpdateTarget();

        AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
        AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;

        base.OnAttached();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
        AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
        AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;

        HideToolTip();
        BindingOperations.ClearBinding(toolTip, ContentControl.ContentProperty);
    }

    protected virtual void UpdateToolTipPosition()
    {
        if (toolTip.IsOpen)
        {
            var mousePos = Mouse.GetPosition(AssociatedObject);
            toolTip.HorizontalOffset = mousePos.X + SystemParameters.CursorWidth / 2;
            toolTip.VerticalOffset = mousePos.Y + SystemParameters.CursorHeight / 2 + 5;
        }
    }

    protected virtual void ShowToolTip()
    {
        if (!toolTip.IsOpen)
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
        VisualTreeHelper.HitTest(AssociatedObject,
            null,
            r =>
            {
                if (r.VisualHit is not UIElement visual || visual.IsHitTestVisible == false)
                    return HitTestResultBehavior.Continue;
                if (visual is FrameworkElement fe && fe.TemplatedParent == AssociatedObject)
                {
                    HideToolTip();
                    return HitTestResultBehavior.Stop;
                }
                if (View.GetComponent(visual) is INotifyPropertyChanged component)
                {
                    View.SetComponent(AssociatedObject, component);
                    ShowToolTip();
                    return HitTestResultBehavior.Stop;
                }
                if (visual is FrameworkElement fe2 && fe2.TemplatedParent is DependencyObject d2 && View.GetComponent(d2) is INotifyPropertyChanged component2)
                {
                    View.SetComponent(AssociatedObject, component2);
                    ShowToolTip();
                    return HitTestResultBehavior.Stop;
                }
                return HitTestResultBehavior.Continue;
            },
            new PointHitTestParameters(e.GetPosition(AssociatedObject))
        );
    }

    // TODO should this be done in PreviewMouseMove instead?
    private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
    {
        UpdateToolTipPosition();
    }

    private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
    {
        HideToolTip();
    }
}
