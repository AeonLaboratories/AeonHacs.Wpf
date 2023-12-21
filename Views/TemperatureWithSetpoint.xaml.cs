using AeonHacs.Wpf.Behaviors;
using AeonHacs.Wpf.Data;
using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AeonHacs.Wpf.Views
{
    /// <summary>
    /// Interaction logic for TemperatureWithSetpoint.xaml
    /// </summary>
    public partial class TemperatureWithSetpoint : Gauge
	{
		//TODO simplify/automate this?

		#region TargetSetpoint

		public static readonly DependencyProperty TargetSetpointProperty = DependencyProperty.Register(
			"TargetSetpoint", typeof(double?), typeof(TemperatureWithSetpoint), new FrameworkPropertyMetadata(null));

		Binding targetSetpointBinding = new Binding($"{nameof(Component)}.{nameof(ViewModels.Heater.TargetSetpoint)}");

		protected double? TargetSetpoint => (double?)GetValue(TargetSetpointProperty);

		#endregion TargetSetpoint

		#region TargetPowerLevel

		public static readonly DependencyProperty TargetPowerLevelProperty = DependencyProperty.Register(
			"TargetPowerLevel", typeof(double?), typeof(TemperatureWithSetpoint), new FrameworkPropertyMetadata(null));

		Binding targetPowerLevelBinding = new Binding($"{nameof(Component)}.{nameof(ViewModels.Heater.TargetPowerLevel)}");

		protected double? TargetPowerLevel => (double?)GetValue(TargetPowerLevelProperty);

		#endregion TargetPowerLevel

		#region TargetManualMode

		public static readonly DependencyProperty TargetManualModeProperty = DependencyProperty.Register(
			"TargetManualMode", typeof(bool?), typeof(TemperatureWithSetpoint), new FrameworkPropertyMetadata(null));

		Binding targetManualModeBinding = new Binding($"{nameof(Component)}.{nameof(ViewModels.Heater.TargetManualMode)}");

		protected bool? TargetManualMode => (bool?)GetValue(TargetManualModeProperty);

		#endregion TargetManualMode

		TextBox SetpointTextBox;

		//static TemperatureWithSetpoint()
		//{
		//	DefaultStyleKeyProperty.OverrideMetadata(typeof(TemperatureWithSetpoint), new FrameworkPropertyMetadata(typeof(TemperatureWithSetpoint)));
		//}

		public TemperatureWithSetpoint()
		{
			InitializeComponent();

			targetSetpointBinding.Source = this;
			targetSetpointBinding.ValidationRules.Add(NumericValidationRule.Int);
			targetSetpointBinding.NotifyOnValidationError = true;
			SetBinding(TargetSetpointProperty, targetSetpointBinding);

			targetPowerLevelBinding.Source = this;
			targetPowerLevelBinding.ValidationRules.Add(NumericValidationRule.Double);
			targetPowerLevelBinding.NotifyOnValidationError = true;
			SetBinding(TargetPowerLevelProperty, targetPowerLevelBinding);

			targetManualModeBinding.Source = this;
			SetBinding(TargetManualModeProperty, targetManualModeBinding);
		}

		protected override void OnComponentTypeChanged()
		{
			base.OnComponentTypeChanged();

			BindingOperations.GetBindingExpression(this, TargetSetpointProperty).UpdateTarget();
			BindingOperations.GetBindingExpression(this, TargetPowerLevelProperty).UpdateTarget();
			BindingOperations.GetBindingExpression(this, TargetManualModeProperty).UpdateTarget();

			if (TargetSetpoint != null || TargetPowerLevel != null)
			{
				if (SetpointTextBox is null)
				{
					SetpointTextBox = new TextBox();

					SetpointTextBox.Width = TemperatureLabel.Width;
					SetpointTextBox.Height = TemperatureLabel.Height;
					SetpointTextBox.VerticalContentAlignment = TemperatureLabel.VerticalContentAlignment;
					SetpointTextBox.HorizontalContentAlignment = TemperatureLabel.HorizontalContentAlignment;
					SetpointTextBox.VerticalAlignment = TemperatureLabel.VerticalAlignment;
					SetpointTextBox.HorizontalAlignment = TemperatureLabel.HorizontalAlignment;
					SetpointTextBox.Margin = TemperatureLabel.Margin;
					SetpointTextBox.Padding = TemperatureLabel.Padding;
					SetpointTextBox.BorderBrush = TemperatureLabel.BorderBrush;
					SetpointTextBox.BorderThickness = TemperatureLabel.BorderThickness;

					var textBoxBehaviors = Interaction.GetBehaviors(SetpointTextBox);
					SetpointTextBox.SetBinding(TextBox.TextProperty, TargetManualMode ?? false ? targetPowerLevelBinding : targetSetpointBinding);
					textBoxBehaviors.Add(new TextBoxExitKeyBehavior());
					textBoxBehaviors.Add(new TextBoxValidationBehavior());
					textBoxBehaviors.Add(new SwapVisibilityBehavior(TemperatureLabel));
					Panel.SetZIndex(SetpointTextBox, -1);
					SetpointTextBox.Visibility = Visibility.Collapsed;
					Container.Children.Add(SetpointTextBox);
				}
			}
			else if(SetpointTextBox != null)
			{
				var textBoxBehaviors = Interaction.GetBehaviors(SetpointTextBox);
				BindingOperations.ClearBinding(SetpointTextBox, TextBox.TextProperty);
				textBoxBehaviors.Clear();
				SetpointTextBox = null;
			}
		}

		protected override void OnComponentUpdated(object sender, PropertyChangedEventArgs e)
		{
			if (TargetManualMode is bool targetManualMode)
				SetpointTextBox.SetBinding(TextBox.TextProperty, targetManualMode ? targetPowerLevelBinding : targetSetpointBinding);

			base.OnComponentUpdated(sender, e);
		}
	}
}
