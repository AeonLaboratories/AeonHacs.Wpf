﻿using AeonHacs;
using AeonHacs.Wpf.Behaviors;
using AeonHacs.Wpf.Converters;
using AeonHacs.Wpf.ViewModels;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace AeonHacs.Wpf.Views
{
    public class View : ContentControl
    {
        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
        }

        #region Visual

        protected virtual void CreateBindings() { }

        public static void ShowProperties(UIElement uiElement, object model)
        {
            if (!(model is INotifyPropertyChanged component)) return;

            if (ViewModel.SettingsWindow == null)
            {
                var panel = new SettingsPanel
                {
                    Source = component is ViewModel vm ? vm.Component : component
                };

                var b = new Breadcrumbs()
                {
                    FontSize = 18,
                    Margin = new Thickness(5)
                };
                b.SetValue(DockPanel.DockProperty, Dock.Top);
                b.SetBinding(Breadcrumbs.SelectedItemProperty, new Binding() { Path = new PropertyPath(SettingsPanel.SourceProperty), Source = panel, Mode = BindingMode.TwoWay });

                var separator = new Border()
                {
                    Height = 1,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                separator.SetResourceReference(BackgroundProperty, SystemColors.ActiveBorderBrushKey);
                separator.SetValue(DockPanel.DockProperty, Dock.Top);

                var sv = new ScrollViewer()
                {
                    Content = panel,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                    Padding = new Thickness(5),
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                };

                var dp = new DockPanel()
                {
                    LastChildFill = true
                };
                dp.Children.Add(b);
                dp.Children.Add(separator);
                dp.Children.Add(sv);

                var window = ViewModel.SettingsWindow = new Window();
                Interaction.GetBehaviors(window).Add(new ComponentToolTipBehavior());

                window.Content = dp;
                window.MaxHeight = 0.7 * SystemParameters.PrimaryScreenHeight;
                window.MaxWidth = 0.25 * SystemParameters.PrimaryScreenWidth;

                var nameBinding = new Binding($"{nameof(SettingsPanel.Source)}.Name") { Source = panel, FallbackValue = "Properties" };
                window.SetBinding(Window.TitleProperty, nameBinding);
                window.ContentRendered += (sender, e) =>
                {
                    window.ClearValue(MaxWidthProperty);
                    window.ClearValue(MaxHeightProperty);
                };
                window.Closed += (sender, e) => ViewModel.SettingsWindow = null;

                //TODO revisit Settings window behaviors
                Interaction.GetBehaviors(window).Add(new SettingsWindowBehavior());
                window.Show();
            }
            else
            {
                if (ViewModel.SettingsWindow.Content is DockPanel d && d.Children.OfType<Breadcrumbs>().FirstOrDefault() is Breadcrumbs b)
                {
                    if (ViewModel.SettingsWindow.WindowState == WindowState.Minimized)
                        ViewModel.SettingsWindow.WindowState = WindowState.Normal;
                    b.RootObject = component is ViewModel vm ? vm.Component : component;
                    ViewModel.SettingsWindow.Activate();
                }
            }
        }

        public static void CreateContextMenu(FrameworkElement fe)
        {
            if (fe == null)
                return;

            if (!(fe.GetValue(ComponentProperty) is INotifyPropertyChanged component))
                return;

            var viewModel = component as ViewModel ?? ViewModel.GetFromModel(component);
            string header = (component as INamedObject)?.Name?.Replace("_", "__");

            ContextMenu contextMenu = fe.ContextMenu;
            contextMenu.Items.Clear();
            if (header != null)
                contextMenu.Items.Add(new MenuItem() { Header = header, IsHitTestVisible = false, Background = SystemColors.ActiveCaptionBrush, FontWeight = FontWeights.Bold });

            if (viewModel?.Context() is List<Context> contextList)
            {
                foreach (var context in contextList)
                {
                    MenuItem menuItem = new MenuItem();
                    if (context.Style == Context.DisplayStyle.Toggle)
                    {
                        menuItem.Header = context.Label;
                        menuItem.IsCheckable = true;
                        menuItem.SetBinding(MenuItem.IsCheckedProperty, context.Binding);
                        if (context.Binding.Mode == BindingMode.OneWay)
                            menuItem.Click += (sender, e) => viewModel.Dispatch(context.Label);
                    }
                    else
                    {
                        if (context.Binding != null)
                            menuItem.SetBinding(HeaderedItemsControl.HeaderProperty, context.Binding);
                        else
                            menuItem.Header = context.Label;

                        if (context.Style == Context.DisplayStyle.Content)
                        {
                            var extraMenuItem = new MenuItem
                            {
                                Visibility = Visibility.Collapsed
                            };
                            contextMenu.Items.Add(extraMenuItem);
                            var textBox = new TextBox();
                            textBox.SetBinding(TextBox.TextProperty, context.Binding);
                            extraMenuItem.Header = textBox;
                            menuItem.StaysOpenOnClick = true;
                            menuItem.HeaderStringFormat = $"{context.Label}: 0.##";
                            Interaction.GetBehaviors(textBox).Add(new TextBoxExitKeyBehavior());
                            Interaction.GetBehaviors(extraMenuItem).Add(new SwapVisibilityBehavior(menuItem));
                        }
                        else
                        {
                            if (context.Dispatch)
                                menuItem.Click += (sender, e) => viewModel.Dispatch(context.Label);
                            else
                                menuItem.Click += (sender, e) => viewModel.Run(context.Label);
                        }
                    }
                    contextMenu.Items.Add(menuItem);
                }
                if (contextMenu.Items.Count > 1)
                    contextMenu.Items.Add(new Separator() { Height = 1, Margin = new Thickness(0), Padding = new Thickness(0) });
            }

            var propertiesMenuItem = new MenuItem() { Header = "Properties" };
            propertiesMenuItem.Click += (sender, e) => ShowProperties(sender as MenuItem, component);
            contextMenu.Items.Add(propertiesMenuItem);

            ContextMenuService.SetPlacement(fe, PlacementMode.MousePoint);
            contextMenu.HorizontalOffset = SystemParameters.CursorWidth / 2;
        }

        #endregion Visual

        #region Component

        public static readonly DependencyProperty ComponentProperty = DependencyProperty.RegisterAttached(
            nameof(Component), typeof(INotifyPropertyChanged), typeof(View), new PropertyMetadata(null, ComponentChanged));

        public static void SetComponent(DependencyObject element, INotifyPropertyChanged component) =>
            element.SetValue(ComponentProperty, component);

        [TypeConverter(typeof(ViewModelConverter))]
        public static INotifyPropertyChanged GetComponent(DependencyObject element) =>
            (INotifyPropertyChanged)element.GetValue(ComponentProperty);

        static void ComponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement fe))
                return;

            var oldComponent = e.OldValue as INotifyPropertyChanged;
            var newComponent = e.NewValue as INotifyPropertyChanged;

            static void Open(object sender, ContextMenuEventArgs e)
            {
                if (sender is FrameworkElement element)
                {
                    CreateContextMenu(element);
                }
            }

            if (oldComponent == null && newComponent != null)
            {
                fe.ContextMenu = new ContextMenu();
                fe.ContextMenu.Closed += (sender, e) =>
                {
                    fe?.ContextMenu?.Items?.Clear();
                };

                fe.ContextMenuOpening += Open;
            }
            else if (oldComponent != null && newComponent == null)
            {
                fe.ContextMenuOpening -= Open;
                fe.ContextMenu = null;
            }

            if (d is View view)
                view.OnComponentChanged(e);
        }

        [TypeConverter(typeof(ViewModelConverter))]
        public INotifyPropertyChanged Component
        {
            get => (INotifyPropertyChanged)GetValue(ComponentProperty);
            set => SetValue(ComponentProperty, value);
        }

        public void OnComponentChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue?.GetType() != e.NewValue?.GetType())
                OnComponentTypeChanged();

            if (e.OldValue is INotifyPropertyChanged oldComponent)
                oldComponent.PropertyChanged -= OnComponentPropertyChanged;

            INotifyPropertyChanged newComponent = e.NewValue as INotifyPropertyChanged;
            if (newComponent != null)
                newComponent.PropertyChanged += OnComponentPropertyChanged;

            OnComponentUpdated(newComponent, BindableObject.PropertyChangedEventArgs(string.Empty));
        }

        protected virtual void OnComponentTypeChanged() { }

        protected void OnComponentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!CheckAccess())
                try { Dispatcher.Invoke(() => OnComponentUpdated(sender, e)); } catch { }
            else
                OnComponentUpdated(sender, e);
        }

        protected virtual void OnComponentUpdated(object sender, PropertyChangedEventArgs e) { }

        #endregion Component

        #region Events

        // TODO Update click checking
        // The ClickEvent is not fired when more than one click happens within the double click time.

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        protected static extern int GetDoubleClickTime();

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(View));

        protected DispatcherTimer ClickWaitTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(GetDoubleClickTime()) };

        [Category("Behavior")]
        public event RoutedEventHandler Click { add { AddHandler(ClickEvent, value); } remove { RemoveHandler(ClickEvent, value); } }

        protected virtual void OnClick() =>
            RaiseEvent(new RoutedEventArgs(ClickEvent, this));

        protected int PendingLeftClicks { get; set; }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            PendingLeftClicks = e.ClickCount;

            if (e.ClickCount == 2 && Component is ViewModel vm && vm.RunHasDefault)
            {
                vm.Dispatch();
                e.Handled = true;
            }
            else
                base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (PendingLeftClicks == 1)
            {
                PendingLeftClicks = 0;
                ClickWaitTimer.Start();
            }
            else if (PendingLeftClicks > 1)
            {
                PendingLeftClicks = 0;
                ClickWaitTimer.Stop();
            }

            base.OnMouseLeftButtonUp(e);
        }

        private void ClickWaitTimer_Tick(object sender, EventArgs e)
        {
            ClickWaitTimer.Stop();
            OnClick();
        }

        #endregion Events

        public View()
        {
            CreateBindings();

            ClickWaitTimer.Tick += ClickWaitTimer_Tick;
        }
    }
}
