using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfControls;

namespace AeonHacs.Wpf.Views
{
    // TODO this is just a SolidColorBrush Color Setting; support Brushes, in general?
    public partial class BrushSetting : DockPanel
    {
        public static Window ColorPicker;
        static ColorPicker colorPickerContent;

        public string Description { get; set; }
        public string BrushName { get; set; }

        ResourceDictionary Preferences => (ResourceDictionary)Application.Current.Resources["PreferencesDictionary"];
        void SetPreference(string name, object value)
        {
            Application.Current.Resources[name] = value;
            Preferences[name] = value;
        }

        public Brush Brush
        {
            get => (Brush)Application.Current.Resources[BrushName];
            set
            {
                Swatch.Background = value;
                SetPreference(BrushName, value);
            }
        }



        public Color Color
        {
            get => (Color)Application.Current.Resources[ColorName];
            set
            {
                SetPreference(ColorName, value);
                Brush = new SolidColorBrush(value);
            }
        }

        bool IsBrushSetting() =>
            !BrushName.IsBlank() &&
            Application.Current.Resources[BrushName] is SolidColorBrush &&
            !ColorName.IsBlank() &&
            Application.Current.Resources[ColorName] is Color;

        string ColorName =>
            !BrushName.IsBlank() && BrushName.EndsWith("Brush") ? BrushName[0..^5] + "Color" :
            null;

        public BrushSetting()
        {
            InitializeComponent();
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            if (ColorPicker == null)
            {
                ColorPicker = new Window();
                ColorPicker.Content = colorPickerContent = new ColorPicker(Color);

                var mouse = PointToScreen(Mouse.GetPosition(this));
                ColorPicker.Top = mouse.Y;
                ColorPicker.Left = mouse.X;
                ColorPicker.SizeToContent = SizeToContent.WidthAndHeight;
                ColorPicker.Closed += (sender, e) =>
                {
                    Color = colorPickerContent.Color;
                    colorPickerContent = null;
                    ColorPicker = null;
                };
                ColorPicker.Show();
            }
            else
            {
                if (ColorPicker.WindowState == WindowState.Minimized)
                    ColorPicker.WindowState = WindowState.Normal;
                ColorPicker.Activate();
            }

            ColorPicker.Title = BrushName;
            colorPickerContent.Margin = new Thickness(5);
            colorPickerContent.OnColorChanged += (sender, e) => Color = e.Color;
        }
    }
}
