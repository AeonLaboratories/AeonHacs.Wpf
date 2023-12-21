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
    /// Interaction logic for OvenTemperature.xaml
    /// </summary>
    public partial class OvenTemperature : Gauge
    {
        #region TargetSetpoint

        public static readonly DependencyProperty TargetSetpointProperty = DependencyProperty.Register(
            "TargetSetpoint", typeof(double?), typeof(OvenTemperature), 
            new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true });

        Binding targetSetpointBinding = new Binding($"{nameof(Component)}.{nameof(ViewModels.Oven.TargetSetpoint)}");

        protected double? TargetSetpoint => (double?)GetValue(TargetSetpointProperty);

        #endregion TargetSetpoint

        TextBox SetpointTextBox;

        public OvenTemperature()
        {
            InitializeComponent();

            targetSetpointBinding.Source = this;
            //targetSetpointBinding.ValidationRules.Add(NumericValidationRule.Int);
            //targetSetpointBinding.NotifyOnValidationError = true;
            SetBinding(TargetSetpointProperty, targetSetpointBinding);

        }

        protected override void OnComponentTypeChanged()
        {
            base.OnComponentTypeChanged();

            BindingOperations.GetBindingExpression(this, TargetSetpointProperty).UpdateTarget();

            if (TargetSetpoint != null)
            {
                if (SetpointTextBox is null)
                {
                    SetpointTextBox = new TextBox()
                    {
                        Width = TemperatureLabel.Width,
                        Height = TemperatureLabel.Height,
                        VerticalContentAlignment = TemperatureLabel.VerticalContentAlignment,
                        HorizontalContentAlignment = TemperatureLabel.HorizontalContentAlignment,
                        VerticalAlignment = TemperatureLabel.VerticalAlignment,
                        HorizontalAlignment = TemperatureLabel.HorizontalAlignment,
                        Margin = TemperatureLabel.Margin,
                        Padding = TemperatureLabel.Padding,
                        BorderBrush = TemperatureLabel.BorderBrush,
                        BorderThickness = TemperatureLabel.BorderThickness
                    };

                    var textBoxBehaviors = Interaction.GetBehaviors(SetpointTextBox);
                    SetpointTextBox.SetBinding(TextBox.TextProperty, new Binding(nameof(TargetSetpoint)) { Source = this });
                    textBoxBehaviors.Add(new TextBoxExitKeyBehavior());
                    //textBoxBehaviors.Add(new TextBoxValidationBehavior());
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
    }
}
