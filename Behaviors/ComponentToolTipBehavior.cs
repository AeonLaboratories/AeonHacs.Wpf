using AeonHacs.Wpf.Converters;
using AeonHacs.Wpf.ViewModels;
using AeonHacs.Wpf.Views;
using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;

namespace AeonHacs.Wpf.Behaviors;

public class ComponentToolTipBehavior : Behavior<Window>
{
    ToolTip toolTip = new ToolTip() { Placement = System.Windows.Controls.Primitives.PlacementMode.Relative };
    DispatcherTimer updateTimer = new DispatcherTimer(DispatcherPriority.DataBind) { Interval = TimeSpan.FromMilliseconds(250) };

    public ComponentToolTipBehavior()
    {
        toolTip.StaysOpen = true;
        toolTip.IsHitTestVisible = false;
        toolTip.HasDropShadow = false;
        toolTip.SnapsToDevicePixels = true;

        updateTimer.Tick += (s, e) =>
        {
            if (currentComponent == null) return;
            if (changingToolTipSource) return;
            toolTipBindingExpression?.UpdateTarget();

            // Hide tooltip if content became empty, open if it became non-empty
            if (toolTip.IsOpen && !HasContent())
                _ = HideToolTipAsync();
            else if (!toolTip.IsOpen && HasContent())
                _ = ShowToolTipAsync();
        };
    }


    bool HasContent() => toolTip.Content is string s && !s.IsBlank();


    [System.Diagnostics.Conditional("DEBUG")]
    void Debug(string dbgmsg) => System.Diagnostics.Debug.WriteLine(dbgmsg);


    bool showingOrHiding = false;
    private async Task ShowToolTipAsync()
    {
        if (showingOrHiding) return;
        if (toolTip.IsOpen || !HasContent()) return;
        showingOrHiding = true;
        try
        {
            Debug("Opening ToolTip...");
            tcsOpened = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            toolTip.IsOpen = true;
            await tcsOpened.Task;
            tcsOpened = null;
        }
        finally { showingOrHiding = false; }
    }

    TaskCompletionSource<bool> tcsOpened { get; set; }
    void onToolTipOpened(object sender, RoutedEventArgs e)
    {
        Debug("ToolTip Opened");
        tcsOpened?.TrySetResult(true);
    }

    private async Task HideToolTipAsync()
    {
        if (showingOrHiding) return;
        if (!toolTip.IsOpen) return;
        showingOrHiding = true;
        try
        {
            Debug("Closing ToolTip...");
            tcsClosed = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            toolTip.IsOpen = false;
            await tcsClosed.Task;
            tcsClosed = null;
        }
        finally { showingOrHiding = false; }
    }

    TaskCompletionSource<bool> tcsClosed { get; set; }
    void onToolTipClosed(object sender, RoutedEventArgs e)
    {
        Debug("ToolTip Closed");
        tcsClosed?.TrySetResult(true);
    }


    INotifyPropertyChanged currentComponent = null;
    bool changingToolTipSource = false;
    async Task SetToolTipSourceAsync(INotifyPropertyChanged component)
    {
        if (component == currentComponent) return;      // redundant; omit?
        if (changingToolTipSource) return;
        changingToolTipSource = true;

        try
        {
            Debug($"component was: {currentComponent}");
            if (component == null) await HideToolTipAsync();
            View.SetComponent(AssociatedObject, component);
            currentComponent = component;
            Debug($"component is: {component}");
            if (component == null)
            {
                updateTimer.Stop();
            }
            else
            {
                toolTipBindingExpression?.UpdateTarget();
                updateTimer.Start();
                await ShowToolTipAsync();
            }
        }
        finally { changingToolTipSource = false; }
    }

    string toString(INotifyPropertyChanged component) =>
        ToStringConverter.Default.Convert(component, typeof(string), null,
            System.Globalization.CultureInfo.CurrentUICulture ) as string;


    BindingExpression toolTipBindingExpression = null;
    protected override void OnAttached()
    {
        toolTip.PlacementTarget = AssociatedObject;

        toolTip.Opened += onToolTipOpened;
        toolTip.Closed += onToolTipClosed;

        var toolTipBinding = new Binding()
        {
            Path = new PropertyPath(View.ComponentProperty),
            Source = AssociatedObject,
            Converter = new ToStringConverter()
        };

        toolTip.SetBinding(ContentControl.ContentProperty, toolTipBinding);
        toolTipBindingExpression = BindingOperations.GetBindingExpression(toolTip, ContentControl.ContentProperty);

        AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
        AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;

        base.OnAttached();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        updateTimer.Stop();

        AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
        AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;

        _ = HideToolTipAsync();

        toolTip.Opened -= onToolTipOpened;
        toolTip.Closed -= onToolTipClosed;

        BindingOperations.ClearBinding(toolTip, ContentControl.ContentProperty);
    }

    /// <summary>
    /// Returns the logical/visual ancestor of <paramref name="d"/> using WPF’s real-world parent
    /// routes in this order:
    /// 1) Visual/Visual3D parent (fast path for normal controls/templates)
    /// 2) TemplatedParent (when inside a ControlTemplate/DataTemplate)
    /// 3) Logical parent (Panels, ContentPresenter, etc.)
    /// 4) For text inlines (Run/Span/Hyperlink), use their content/logical Parent
    /// </summary>
    private static DependencyObject GetAncestor(DependencyObject d)
    {
        if (d == null) return null;

        // Text inlines and flow content (no visual parent)
        if (d is FrameworkContentElement fce)
            return fce.Parent ?? LogicalTreeHelper.GetParent(fce);

        // Visual/Visual3D path (covers most FrameworkElements and template parts)
        if (d is Visual || d is System.Windows.Media.Media3D.Visual3D)
        {
            var visualParent = VisualTreeHelper.GetParent(d);
            if (visualParent != null)
                return visualParent;

            // Inside a template: climb to the control owning this template
            if (d is FrameworkElement fe && fe.TemplatedParent is DependencyObject tp)
                return tp;

            // Fallback for elements that only participate in the logical tree
            return LogicalTreeHelper.GetParent(d);
        }

        // Last-resort logical parent (covers ContentElement base and rare cases)
        return LogicalTreeHelper.GetParent(d);
    }


    bool previewing = false;
    private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (previewing) return;
        previewing = true;

        try
        {
            Debug("Previewing...");

            // Start from the actual element under the pointer.
            var mousePos = e.GetPosition(AssociatedObject);
            var hit = Mouse.DirectlyOver as DependencyObject
                      ?? AssociatedObject.InputHitTest(mousePos) as DependencyObject;

            string help = null;
            INotifyPropertyChanged component = null;

            // Walk up through visuals/templates/logical/content until we reach the Window.
            for (var node = hit; node != null && node != AssociatedObject; node = GetAncestor(node))
            {
                // Is there a component?
                component = View.GetComponent(node);
                if (component != null)
                {
                    if (help.IsBlank())
                    {
                        if (component is ViewModel vm)
                            help = vm.Description;
                        else if (component is NamedObject obj)
                            help = obj.Description;
                    }

                    // If ToString() is meaningful, this component provides the tooltip.
                    // If not, keep looking.
                    if (toString(component).IsBlank()) component = null;
                }

                // If help is still empty, check for AutomationProperties.HelpText
                if (help.IsBlank())
                    help = node.GetValue(AutomationProperties.HelpTextProperty) as string;

                if (component != null) break; // We have a viable tooltip source; stop searching.
            }

            // Update the status bar HelpText.
            AutomationProperties.SetHelpText(AssociatedObject, help ?? "");

            // Keep the tooltip tracking the cursor.
            if (toolTip.IsOpen)
            {
                toolTip.HorizontalOffset = mousePos.X + SystemParameters.CursorWidth / 2;
                toolTip.VerticalOffset = mousePos.Y + SystemParameters.CursorHeight / 2 + 5;
            }

            if (component != currentComponent)
                _ = SetToolTipSourceAsync(component);

            Debug("...Previewed");
        }
        finally { previewing = false; }
    }

    private async void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
    {
        await HideToolTipAsync();
        View.SetComponent(AssociatedObject, null);
        currentComponent = null;
        AutomationProperties.SetHelpText(AssociatedObject, "");
    }
}
