using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace HACS.WPF.Views
{
    /// <summary>
    /// Interaction logic for IP.xaml
    /// </summary>
    public partial class InletPort : View
	{
		#region PortType

		public static readonly DependencyProperty PortTypeProperty = DependencyProperty.Register(
			nameof(PortType), typeof(HACS.Core.InletPortType), typeof(InletPort));

		public HACS.Core.InletPortType PortType
		{
			get => (HACS.Core.InletPortType)GetValue(PortTypeProperty);
			set => SetValue(PortTypeProperty, value);
		}

        #endregion PortType

        public static readonly DependencyProperty GasTypeProperty = DependencyProperty.Register(
            nameof(GasType), typeof(object), typeof(InletPort));

        public object GasType
		{
            get => GetValue(GasTypeProperty);
            set => SetValue(GasTypeProperty, value);
		}

        #region View Visiblity

        public static readonly DependencyProperty CombustionVisibilityProperty = DependencyProperty.Register(
            nameof(CombustionVisibility), typeof(Visibility), typeof(InletPort));
        public Visibility CombustionVisibility
        {
            get => (Visibility)GetValue(CombustionVisibilityProperty);
            set => SetValue(CombustionVisibilityProperty, value);
        }


        public static readonly DependencyProperty NeedleVisibilityProperty = DependencyProperty.Register(
            nameof(NeedleVisibility), typeof(Visibility), typeof(InletPort));
        public Visibility NeedleVisibility
        {
            get => (Visibility)GetValue(NeedleVisibilityProperty);
            set => SetValue(NeedleVisibilityProperty, value);
        }


        public static readonly DependencyProperty BreaksealVisibilityProperty = DependencyProperty.Register(
            nameof(BreaksealVisibility), typeof(Visibility), typeof(InletPort));
        public Visibility BreaksealVisibility
        {
            get => (Visibility)GetValue(BreaksealVisibilityProperty);
            set => SetValue(BreaksealVisibilityProperty, value);
        }

        public static readonly DependencyProperty GasSupplyVisibilityProperty = DependencyProperty.Register(
            nameof(GasSupplyVisibility), typeof(Visibility), typeof(InletPort));
        public Visibility GasSupplyVisibility
        {
            get => (Visibility)GetValue(GasSupplyVisibilityProperty);
            set => SetValue(GasSupplyVisibilityProperty, value);
        }

        [ValueConversion(typeof(Core.InletPortType), typeof(Visibility))]
        class SelectedVisibilityConverter : IValueConverter
        {
            public static SelectedVisibilityConverter Default = new SelectedVisibilityConverter();

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var result =
                    value != null && value.Equals(parameter) ? Visibility.Visible : Visibility.Hidden;
                return result;
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
        #endregion View Visiblity

        public InletPort()
		{
			InitializeComponent();
		}

		protected override void CreateBindings()
		{
			base.CreateBindings();

			SetBinding(PortTypeProperty, new Binding("Component.PortType") { Source = this, FallbackValue = HACS.Core.InletPortType.Combustion, TargetNullValue = HACS.Core.InletPortType.Combustion });
		}

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
        }
    }
}
