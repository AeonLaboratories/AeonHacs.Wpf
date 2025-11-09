using AeonHacs.Wpf.Converters;
using AeonHacs.Wpf.ViewModels;
using AeonHacs.Wpf.Views;
using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
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

    readonly ToStringConverter _toString = ToStringConverter.Default;

    string toString(INotifyPropertyChanged component) =>
        _toString.Convert(component, typeof(string), null,
            CultureInfo.CurrentUICulture) as string;
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
            Converter = _toString
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


/// <summary>
/// (ChatGPT suggested this alternative, after reviewing the version above)
/// Visual-Studio-style "Quick Info" behavior:
/// - Finds the best element under the mouse (Text inlines, template parts, etc.)
/// - Computes HelpText (static) and ToolTip text (dynamic)
/// - Opens only when text is non-empty; keeps open while content updates
/// - Debounced open/close to avoid micro flashes during tiny transitions
/// - Never blocks the UI thread; async show/hide await the popup events
/// 
/// Assumptions:
/// - Tooltip content = ToString(component) via ToStringConverter
/// - HelpText = NamedObject.Description OR AutomationProperties.HelpText
/// </summary>
//public class ComponentToolTipBehavior : Behavior<Window>
//{
//    // ----- Policy knobs (tune for feel/latency) ---------------------------

//    private const int HoverOpenDelayMs = 200;  // wait before opening on new target
//    private const int CloseDebounceMs = 80;   // small grace when leaving (prevents flash)
//    private const int EmptyGraceMs = 200;  // allow dynamic content to go empty briefly
//    private const int DynamicUpdateMs = 250;  // refresh ToString() while open

//    // ----- Core objects ---------------------------------------------------

//    private readonly ToolTip _tip = new ToolTip
//    {
//        Placement = System.Windows.Controls.Primitives.PlacementMode.Relative,
//        StaysOpen = true,
//        IsHitTestVisible = false,
//        HasDropShadow = false,
//        SnapsToDevicePixels = true
//    };

//    private readonly DispatcherTimer _dynamicUpdate =
//        new DispatcherTimer(DispatcherPriority.DataBind)
//        { Interval = TimeSpan.FromMilliseconds(DynamicUpdateMs) };

//    private BindingExpression _contentBinding;
//    private INotifyPropertyChanged _currentComponent;
//    private bool HasContent() => _tip.Content is string s && !s.IsBlank();

//    // Small state machine
//    private enum PopupState { Closed, Opening, Open, Closing }
//    private PopupState _state = PopupState.Closed;

//    // Awaiters for popup events
//    private TaskCompletionSource<bool> _tcsOpened;
//    private TaskCompletionSource<bool> _tcsClosed;

//    // Cancellation for pending open/close delays
//    private CancellationTokenSource _hoverCts;
//    private CancellationTokenSource _closeCts;
//    private DateTime _lastNonEmptySeen = DateTime.MinValue;

//    private readonly ToStringConverter _toString = ToStringConverter.Default;

//    private string toString(INotifyPropertyChanged component) =>
//        _toString.Convert(component, typeof(string), null,
//            CultureInfo.CurrentUICulture) as string;


//    // ----- Attach / Detach -----------------------------------------------

//    protected override void OnAttached()
//    {
//        base.OnAttached();

//        _tip.PlacementTarget = AssociatedObject;
//        _tip.Opened += Tip_Opened;
//        _tip.Closed += Tip_Closed;

//        var binding = new Binding
//        {
//            Path = new PropertyPath(View.ComponentProperty),
//            Source = AssociatedObject,
//            Converter = _toString
//        };
//        _tip.SetBinding(ContentControl.ContentProperty, binding);
//        _contentBinding = BindingOperations.GetBindingExpression(_tip, ContentControl.ContentProperty);

//        _dynamicUpdate.Tick += DynamicUpdate_Tick;

//        AssociatedObject.PreviewMouseMove += OnPreviewMouseMove;
//        AssociatedObject.MouseLeave += OnMouseLeave;
//    }

//    protected override void OnDetaching()
//    {
//        base.OnDetaching();

//        AssociatedObject.PreviewMouseMove -= OnPreviewMouseMove;
//        AssociatedObject.MouseLeave -= OnMouseLeave;

//        _ = HideAsync(); // fire-and-forget, but it awaits Closed internally
//        _tip.Opened -= Tip_Opened;
//        _tip.Closed -= Tip_Closed;

//        _dynamicUpdate.Stop();
//        _dynamicUpdate.Tick -= DynamicUpdate_Tick;

//        BindingOperations.ClearBinding(_tip, ContentControl.ContentProperty);
//    }

//    // ----- Popup event awaiters ------------------------------------------

//    private void Tip_Opened(object s, RoutedEventArgs e) => _tcsOpened?.TrySetResult(true);
//    private void Tip_Closed(object s, RoutedEventArgs e) => _tcsClosed?.TrySetResult(true);

//    private Task WhenOpenedAsync()
//    {
//        _tcsOpened = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
//        return _tcsOpened.Task;
//    }
//    private Task WhenClosedAsync()
//    {
//        _tcsClosed = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
//        return _tcsClosed.Task;
//    }

//    // ----- Dynamic update (runs only while open) --------------------------

//    private void DynamicUpdate_Tick(object sender, EventArgs e)
//    {
//        if (_currentComponent == null) return;

//        // Refresh tooltip text from ToString(component)
//        _contentBinding?.UpdateTarget();

//        // Allow brief empty transitions (e.g., live status list flickers)
//        if (HasContent())
//        {
//            _lastNonEmptySeen = DateTime.UtcNow;
//        }
//        else if (_state == PopupState.Open &&
//                 (DateTime.UtcNow - _lastNonEmptySeen).TotalMilliseconds > EmptyGraceMs)
//        {
//            _ = HideAsync(); // content has stayed empty past grace -> close
//        }
//    }

//    // ----- Mouse plumbing -------------------------------------------------

//    private bool _previewing;

//    private void OnPreviewMouseMove(object sender, MouseEventArgs e)
//    {
//        if (_previewing) return;
//        _previewing = true;

//        try
//        {
//            var mouse = e.GetPosition(AssociatedObject);
//            var hit = Mouse.DirectlyOver as DependencyObject
//                      ?? AssociatedObject.InputHitTest(mouse) as DependencyObject;

//            // Find from first ancestor with Help + ToolTip eligibility
//            string help = null;
//            INotifyPropertyChanged component = null;
//            for (var node = hit; node != null && node != AssociatedObject; node = GetAncestor(node))
//            {
//                // Is there a component?
//                component = View.GetComponent(node);
//                if (component != null)
//                {
//                    if (help.IsBlank())
//                    {
//                        if (component is ViewModel vm)
//                            help = vm.Description;
//                        else if (component is NamedObject obj)
//                            help = obj.Description;
//                    }

//                    // If ToString() is meaningful, this component provides the tooltip.
//                    // If not, keep looking.
//                    if (toString(component).IsBlank()) component = null;
//                }

//                // If help is still empty, check for AutomationProperties.HelpText
//                if (help.IsBlank())
//                    help = node.GetValue(AutomationProperties.HelpTextProperty) as string;

//                if (component != null) break; // We have a viable tooltip source; stop searching.
//            }

//            // Status bar text
//            AutomationProperties.SetHelpText(AssociatedObject, help ?? "");

//            // Cursor tracking for the open popup
//            if (_state == PopupState.Open || _state == PopupState.Opening)
//            {
//                _tip.HorizontalOffset = mouse.X + SystemParameters.CursorWidth / 2;
//                _tip.VerticalOffset = mouse.Y + SystemParameters.CursorHeight / 2 + 5;
//            }

//            // If component changed, schedule open/close with debounce
//            if (!ReferenceEquals(component, _currentComponent))
//            {
//                _ = SwitchSourceAsync(component, mouse);
//            }
//        }
//        finally
//        {
//            _previewing = false;
//        }
//    }

//    private async void OnMouseLeave(object sender, MouseEventArgs e)
//    {
//        await HideAsync();
//        View.SetComponent(AssociatedObject, null);
//        _currentComponent = null;
//        AutomationProperties.SetHelpText(AssociatedObject, "");
//    }

//    // ----- Source switching with debounce/hysteresis ----------------------

//    private async Task SwitchSourceAsync(INotifyPropertyChanged next, Point mouse)
//    {
//        // Cancel any pending hovers/closes
//        _hoverCts?.Cancel();
//        _closeCts?.Cancel();

//        if (next == null)
//        {
//            // leaving a component: debounce close slightly
//            _closeCts = new CancellationTokenSource();
//            try
//            {
//                await Task.Delay(CloseDebounceMs, _closeCts.Token);
//            }
//            catch (TaskCanceledException) { return; }

//            // After debounce, really clear
//            await HideAsync();
//            if (_currentComponent != null)
//            {
//                View.SetComponent(AssociatedObject, null);
//                _currentComponent = null;
//            }
//            return;
//        }

//        // New candidate: wait a small hover delay before opening
//        _hoverCts = new CancellationTokenSource();
//        try
//        {
//            await Task.Delay(HoverOpenDelayMs, _hoverCts.Token);
//        }
//        catch (TaskCanceledException) { return; }

//        // Set component, refresh content; only open if non-empty
//        if (!ReferenceEquals(_currentComponent, next))
//        {
//            View.SetComponent(AssociatedObject, next);
//            _currentComponent = next;
//        }

//        _contentBinding?.UpdateTarget();

//        // Position before opening (avoids “jump” on first frame)
//        _tip.HorizontalOffset = mouse.X + SystemParameters.CursorWidth / 2;
//        _tip.VerticalOffset = mouse.Y + SystemParameters.CursorHeight / 2 + 5;

//        if (_state == PopupState.Closed || _state == PopupState.Closing)
//            await ShowAsync();
//        else
//            _lastNonEmptySeen = DateTime.UtcNow; // we have content; keep alive
//    }

//    // ----- Show / Hide with event-await ----------------------------------

//    private async Task ShowAsync()
//    {
//        if (_state == PopupState.Open || _state == PopupState.Opening) return;
//        if (!HasContent()) return;

//        _state = PopupState.Opening;
//        _lastNonEmptySeen = DateTime.UtcNow;

//        var opened = WhenOpenedAsync();
//        _tip.IsOpen = true;
//        await opened;

//        _state = PopupState.Open;
//        _dynamicUpdate.Start();
//    }

//    private async Task HideAsync()
//    {
//        if (_state == PopupState.Closed || _state == PopupState.Closing) return;

//        _state = PopupState.Closing;
//        _dynamicUpdate.Stop();

//        var closed = WhenClosedAsync();
//        _tip.IsOpen = false;
//        await closed;

//        _state = PopupState.Closed;
//    }

//    // ----- Helpers: ancestry + immediate text resolution ------------------

//    private static DependencyObject GetAncestor(DependencyObject d)
//    {
//        if (d == null) return null;

//        if (d is FrameworkContentElement fce)
//            return fce.Parent ?? LogicalTreeHelper.GetParent(fce);

//        if (d is Visual || d is System.Windows.Media.Media3D.Visual3D)
//        {
//            var v = VisualTreeHelper.GetParent(d);
//            if (v != null) return v;

//            if (d is FrameworkElement fe && fe.TemplatedParent is DependencyObject tp)
//                return tp;
//        }
//        return LogicalTreeHelper.GetParent(d);
//    }

//    /// <summary>
//    /// Immediate element only (no walking): 
//    /// HelpText = NamedObject.Description or AutomationProperties.HelpText
//    /// ToolTip  = ToString(component) (dynamic)
//    /// </summary>
//    private (string helpText, string toolTip) GetHelpAndTip(DependencyObject element)
//    {
//        if (element == null) return (null, null);

//        string help = null;
//        string tip = null;

//        if (View.GetComponent(element) is INotifyPropertyChanged comp)
//        {
//            if (comp is NamedObject no) help = no.Description;
//            else if (comp is ViewModel vm) help = vm.Description; // safe if you keep it
//            tip = toString(comp);
//        }

//        if (help.IsBlank())
//            help = element.GetValue(AutomationProperties.HelpTextProperty) as string;

//        if (help.IsBlank()) help = null;
//        if (tip.IsBlank()) tip = null;

//        return (help, tip);
//    }
//}

