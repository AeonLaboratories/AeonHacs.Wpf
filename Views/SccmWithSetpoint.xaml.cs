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
    public partial class SccmWithSetpoint : Gauge
    {
        //TODO simplify/automate this?

        #region Setpoint

        public static readonly DependencyProperty SetpointProperty = DependencyProperty.Register(
            "Setpoint", typeof(double?), typeof(SccmWithSetpoint), new FrameworkPropertyMetadata(null));

        Binding setpointBinding = new Binding($"{nameof(Component)}.{nameof(ViewModels.MassFlowController.Setpoint)}");

        protected double? Setpoint => (double?)GetValue(SetpointProperty);

        #endregion Setpoint

        TextBox SetpointTextBox;

        public SccmWithSetpoint()
        {
            InitializeComponent();

            setpointBinding.Source = this;
            setpointBinding.ValidationRules.Add(NumericValidationRule.Int);
            setpointBinding.NotifyOnValidationError = true;
            SetBinding(SetpointProperty, setpointBinding);
        }

        protected override void OnComponentTypeChanged()
        {
            base.OnComponentTypeChanged();

            BindingOperations.GetBindingExpression(this, SetpointProperty).UpdateTarget();

            if (Setpoint != null)
            {
                if (SetpointTextBox is null)
                {
                    SetpointTextBox = new TextBox();

                    SetpointTextBox.Width = SccmLabel.Width;
                    SetpointTextBox.Height = SccmLabel.Height;
                    SetpointTextBox.VerticalContentAlignment = SccmLabel.VerticalContentAlignment;
                    SetpointTextBox.HorizontalContentAlignment = SccmLabel.HorizontalContentAlignment;
                    SetpointTextBox.VerticalAlignment = SccmLabel.VerticalAlignment;
                    SetpointTextBox.HorizontalAlignment = SccmLabel.HorizontalAlignment;
                    SetpointTextBox.Margin = SccmLabel.Margin;
                    SetpointTextBox.Padding = SccmLabel.Padding;
                    SetpointTextBox.BorderBrush = SccmLabel.BorderBrush;
                    SetpointTextBox.BorderThickness = SccmLabel.BorderThickness;

                    var textBoxBehaviors = Interaction.GetBehaviors(SetpointTextBox);
                    SetpointTextBox.SetBinding(TextBox.TextProperty, setpointBinding);
                    textBoxBehaviors.Add(new TextBoxExitKeyBehavior());
                    textBoxBehaviors.Add(new TextBoxValidationBehavior());
                    textBoxBehaviors.Add(new SwapVisibilityBehavior(SccmLabel));
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
            base.OnComponentUpdated(sender, e);
        }
    }
}
