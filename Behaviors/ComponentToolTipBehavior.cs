using AeonHacs.Wpf.Converters;
using AeonHacs.Wpf.Views;
using Microsoft.Xaml.Behaviors;
using System;
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
        toolTip.SetBinding(ContentControl.ContentProperty, new Binding() { Path = new PropertyPath(View.ComponentProperty), RelativeSource = RelativeSource.Self, Converter = new ToStringConverter() });
        var bindingExpression = BindingOperations.GetBindingExpression(toolTip, ContentControl.ContentProperty);

        updateTimer.Tick += (s, e) => bindingExpression?.UpdateTarget();

        AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
        AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;

        base.OnAttached();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        toolTip.IsOpen = false;
        updateTimer.Stop();
        BindingOperations.ClearBinding(toolTip, ContentControl.ContentProperty);
        AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
        AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
        AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;
    }

    protected virtual void UpdateToolTipPosition()
    {
        var mousePos = Mouse.GetPosition(AssociatedObject);
        toolTip.HorizontalOffset = mousePos.X + SystemParameters.CursorWidth / 2;
        toolTip.VerticalOffset = mousePos.Y + SystemParameters.CursorHeight / 2 + 5;
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
        // Get hovered component.
        VisualTreeHelper.HitTest(
            AssociatedObject,
            d =>
            {
                if (d is UIElement e && View.GetComponent(e) is IHacsComponent c)
                {
                    View.SetComponent(toolTip, c);
                    ShowToolTip();
                    return HitTestFilterBehavior.Stop;
                }
                return HitTestFilterBehavior.Continue;
            },
            r =>
            {
                if (r.VisualHit is FrameworkElement fe && fe.TemplatedParent == AssociatedObject)
                {
                    HideToolTip();
                    return HitTestResultBehavior.Stop;
                }
                return HitTestResultBehavior.Continue;
            },
            new PointHitTestParameters(e.GetPosition(AssociatedObject))
        );
    }

    private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
    {
        if (toolTip.IsOpen)
            UpdateToolTipPosition();
    }

    private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
    {
        HideToolTip();
    }
}
