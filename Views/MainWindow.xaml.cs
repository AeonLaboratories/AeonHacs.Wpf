using AeonHacs.Wpf.Behaviors;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using static AeonHacs.Notify;

namespace AeonHacs.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static class NativeMethods
        {
            // Import SetThreadExecutionState Win32 API and necessary flags
            [DllImport("kernel32.dll")]
            public static extern uint SetThreadExecutionState(uint esFlags);
            public const uint ES_CONTINUOUS = 0x80000000;
            public const uint ES_SYSTEM_REQUIRED = 0x00000001;
        }

        static MainWindow()
        {
            // Fixes an issue where windows appear blank when
            // shown while the screensaver is active.
            System.Windows.Media.RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;
        }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            nameof(Scale), typeof(double), typeof(MainWindow));

        public double Scale { get => (double)GetValue(ScaleProperty); set => SetValue(ScaleProperty, value); }

        public bool IsClosed { get; protected set; }

        public ControlPanel ControlPanel { get; protected set; }
        protected HacsBase Hacs => ControlPanel?.Hacs;

        Window sampleManager;
        Window protocolEditor;
        Window deviceSettings;
        Window preferences;

        public MainWindow()
        {
            InitializeComponent();
            InitializeCommands();
            SubscribeNotices();

            var preventSleep = NativeMethods.ES_CONTINUOUS | NativeMethods.ES_SYSTEM_REQUIRED;
            if (0 == NativeMethods.SetThreadExecutionState(preventSleep))
            {
                MessageBox.Show("Call to SetThreadExecutionState failed unexpectedly.",
                    Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void InitializeCommands()
        {
            CommandBindings.Add(new(HacsCommands.EditSample, (_, e) => EditSample(e.Parameter)));
        }

        public virtual void LoadControlPanel(ControlPanel controlPanel)
        {
            ControlPanel = controlPanel;

            MainContent.Child = controlPanel;

            Title = Hacs?.Name ?? "Aeon Hacs";
        }

        static readonly Dictionary<string, SoundPlayer> sounds = new();

        public static void PlaySound(Notice notice)
        {
            try
            {
                if (sounds.TryGetValue(notice.Message, out var sound))
                    sound.Play();
                else
                {
                    //TODO support full path in message?
                    var player = new SoundPlayer(@$"C:\Windows\Media\{notice.Message}.wav");
                    player.Play();
                    sounds.Add(notice.Message, player);
                }
                return;
            }
            catch { }

            SystemSounds.Beep.Play();
        }

        public virtual void ShowNotice(Notice notice)
        {
            if (notice.Type == NoticeType.Sound)
                PlaySound(notice);
            else
                Dispatcher.BeginInvoke(() => NoticeWindow.Show(notice));
        }

        protected virtual async Task<Notice> ShowNoticeAsync(Notice notice) =>
            await Dispatcher.Invoke(() => NoticeWindow.ShowDialog(notice));

        public virtual Notice RequestResponse(Notice notice) =>
            ShowNoticeAsync(notice).Result;

        protected virtual void SubscribeNotices()
        {
            OnNotice += ShowNotice;
            OnPrompt += RequestResponse;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (AeonHacs.Hacs.Stopped || ControlPanel == null)
                base.OnClosing(e);
            else if (!AeonHacs.Hacs.Stopping)
            {
                e.Cancel = true;
                Task.Run(AeonHacs.Hacs.Stop).ContinueWith(t => Dispatcher.Invoke(Close));
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsClosed = true;
            NativeMethods.SetThreadExecutionState(NativeMethods.ES_CONTINUOUS);
            if (AeonHacs.Hacs.RestartRequested && !restarting)
            {
                restarting = true;
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            }
        }
        bool restarting = false;


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.ChangedButton == MouseButton.Left)
                FocusManager.SetFocusedElement(this, this);
        }

        //TODO static method in some? class or is there a better way?
        void MoveWindowToMouse(Window window)
        {
            var mousePos = PointToScreen(Mouse.GetPosition(this));
            var source = PresentationSource.FromVisual(this);
            Point dpi = new(source.CompositionTarget.TransformToDevice.M11, source.CompositionTarget.TransformToDevice.M22);

            window.Left = mousePos.X / dpi.X;
            window.Top = mousePos.Y / dpi.Y;
        }

        void EditSample(object source)
        {
            SampleEditor editor;
            if (source is Components.Sample sample)
                editor = new(sample);
            else if (source is Components.InletPort ip)
                editor = new(ip);
            else
                editor = new();
            var w = new Window
            {
                Content = editor,
                SizeToContent = SizeToContent.WidthAndHeight
            };
            w.SetBinding(TitleProperty, new Binding("Sample.Name") { Source = editor, FallbackValue = "New Sample" });
            w.CommandBindings.Add(new(FormCommands.Close, (_, _) => w.Close()));

            w.Show();
        }

        /// <summary>
        /// Show a Sample Manager window, from which a sample can be added, edited, or deleted
        /// </summary>
        void ShowSampleManager()
        {
            if (sampleManager == null)
            {
                var w = sampleManager = new Window();
                MoveWindowToMouse(sampleManager);
                w.Title = Title + " Samples";
                var sm = new SampleManager();
                w.Content = sm;
                w.SizeToContent = SizeToContent.WidthAndHeight;
                sm.SampleList.SizeChanged += (sender, e) =>
                    w.SizeToContent = SizeToContent.WidthAndHeight;
                w.Closed += (sender, e) =>
                    sampleManager = null;
                w.ResizeMode = ResizeMode.NoResize;
                w.Show();
            }
            else
            {
                if (sampleManager.WindowState == WindowState.Minimized)
                    sampleManager.WindowState = WindowState.Normal;
                sampleManager.Activate();
            }
        }


        void ShowProtocolEditor()
        {
            if (protocolEditor == null)
            {
                var w = protocolEditor = new Window();
                w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                w.Title = Title + " Protocols";
                w.Content = new ProtocolEditor(Hacs as Components.ProcessManager);
                w.SizeToContent = SizeToContent.Width;
                w.ContentRendered += (_, _) =>
                {
                    w.MinHeight = w.MinWidth = w.ActualWidth;
                };
                w.Closed += (sender, e) => protocolEditor = null;
                w.Show();
            }
            else
            {
                if (protocolEditor.WindowState == WindowState.Minimized)
                    protocolEditor.WindowState = WindowState.Normal;
                protocolEditor.Activate();
            }
        }


        void ShowSettings()
        {
            if (Hacs == null) return;
            if (deviceSettings == null)
            {
                var w = deviceSettings = new Window();
                MoveWindowToMouse(deviceSettings);
                w.Title = Title + " Settings";
                w.Content = new SettingsPage(Hacs);
                w.SizeToContent = SizeToContent.WidthAndHeight;
                w.MaxHeight = 0.7 * SystemParameters.PrimaryScreenHeight;
                w.ContentRendered += (sender, e) =>
                {
                    deviceSettings.SizeToContent=SizeToContent.Width;
                    deviceSettings.ClearValue(MaxHeightProperty);
                };
                w.Closed += (sender, e) => deviceSettings = null;
                Interaction.GetBehaviors(w).Add(new SettingsWindowBehavior());
                Interaction.GetBehaviors(w).Add(new ComponentToolTipBehavior());
                w.Show();
            }
            else
            {
                if (deviceSettings.WindowState == WindowState.Minimized)
                    deviceSettings.WindowState = WindowState.Normal;
                deviceSettings.Activate();
            }
        }

        void ShowPreferencesWindow()
        {
            if (preferences == null)
            {
                var w = preferences = new Preferences();
                MoveWindowToMouse(w);
                w.Title = Title + " Preferences";
                w.SizeToContent = SizeToContent.WidthAndHeight;
                //w.ContentRendered += (sender, e) =>
                //    w.ClearValue(SizeToContentProperty);
                w.Closed += (sender, e) => preferences = null;
                w.Show();
            }
            else
            {
                if (preferences.WindowState == WindowState.Minimized)
                    preferences.WindowState = WindowState.Normal;
                preferences.Activate();
            }
        }

        private void Samples_Click(object sender, RoutedEventArgs e) =>
            ShowSampleManager();

        private void Protocols_Click(object sender, RoutedEventArgs e) =>
            ShowProtocolEditor();

        private void Settings_Click(object sender, RoutedEventArgs e) =>
            ShowSettings();
        private void Preferences_Click(object sender, RoutedEventArgs e) =>
            ShowPreferencesWindow();

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to restart the application?", "Restart", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AeonHacs.Hacs.RestartRequested = true;
                Close();
            }
        }
    }
}
