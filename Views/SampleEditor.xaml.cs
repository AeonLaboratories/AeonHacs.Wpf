using AeonHacs.Components;
using AeonHacs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace AeonHacs.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SampleEditor.xaml
    /// </summary>
    public partial class SampleEditor : UserControl
    {
        public delegate void SampleUpdated();

        public event SampleUpdated Updated;

        public ISample Sample
        {
            get => sample ??= new Sample()
            {
                Take_d13C = CegsPreferences.Take13CDefault
            };
            set
            {
                if (value != null)
                {
                    sample = value;
                    SampleIsVirtual = false;
                    MassTextBox.Text = GetMassInUnits(sample.Grams).ToString();
                }
            }
        }
        ISample sample;
        bool SampleIsVirtual = true;

        public MassUnits MassUnits { get; set; } = CegsPreferences.DefaultMassUnits;

        public SampleEditor()
        {
            InitializeComponent();
            InletPortComboBox.ItemsSource = NamedObject.FindAll<IInletPort>();
            InletPortComboBox.DisplayMemberPath = "Name";
            PortTypeComboBox.ItemsSource = Enum.GetValues(typeof(AeonHacs.InletPortType));
            MassUnitsComboBox.ItemsSource = Enum.GetValues(typeof(MassUnits));
            ProcessComboBox.ItemsSource = NamedObject.CachedList<ProcessSequence>();
            ProcessComboBox.DisplayMemberPath = "Name";
            d13CRow.Visibility = NamedObject.FindAll<Id13CPort>().Count() > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public SampleEditor(ISample sample) : this()
        {
            Sample = sample;
        }

        public SampleEditor(IInletPort inletPort) : this(inletPort?.Sample)
        {
            Sample.InletPort = inletPort;
        }

        protected double GetMassInUnits(double grams)
        {
            return MassUnits switch
            {
                MassUnits.μmol => grams / CegsPreferences.GramsCarbonPerMole * 1000000,
                MassUnits.μg => grams * 1000000,
                MassUnits.mg => grams * 1000,
                _ => grams,
            };
        }

        protected double GetGrams(double mass, MassUnits units)
        {
            return units switch
            {
                MassUnits.μmol => mass / 1000000 * CegsPreferences.GramsCarbonPerMole,
                MassUnits.μg => mass / 1000000,
                MassUnits.mg => mass / 1000,
                _ => mass,
            };
        }

        protected virtual void Save()
        {
            SampleIsVirtual = false;

            if (InletPortComboBox.SelectedItem is IInletPort ip)
            {
                // if the user changed the Sample's InletPort
                // and the old inlet port still contains Sample,
                // remove it from there
                if (Sample.InletPort is IInletPort oldIp && ip != oldIp && oldIp.Sample == Sample)
                    oldIp.Aliquot = null;    // clear LinePort.Sample by clearing LinePort.Aliquot

                var ipIsFree =
                    ip.State == LinePort.States.Complete ||
                    ip.State == LinePort.States.Loaded ||
                    ip.State == LinePort.States.Empty;

                if (ipIsFree)
                {
                    if (ip.Sample != Sample)
                        ip.Sample = Sample;

                    if (ip.State == LinePort.States.Empty || LabIDTextBox.Text != Sample.LabId)
                        ip.State = LinePort.States.Loaded;
                }

                if (ip.State == LinePort.States.Loaded &&
                    NamedObject.FirstOrDefault<Cegs>() is Cegs cegs &&
                    (cegs.InletPorts?.Contains(ip) ?? false))
                {
                    if (!cegs.Busy)
                        cegs.InletPort = ip;
                }
            }

            BindingOperations.GetBindingExpression(LabIDTextBox, TextBox.TextProperty)?.UpdateSource();

            //BindingOperations.GetBindingExpression(MassTextBox, TextBox.TextProperty)?.UpdateSource();
            if (double.TryParse(MassTextBox.Text, out double mass))
                Sample.Grams = GetGrams(mass, MassUnits);

            BindingOperations.GetBindingExpression(InletPortComboBox, Selector.SelectedItemProperty)?.UpdateSource();
            BindingOperations.GetBindingExpression(PortTypeComboBox, Selector.SelectedItemProperty)?.UpdateSource();
            BindingOperations.GetBindingExpression(ProcessComboBox, Selector.SelectedValueProperty)?.UpdateSource();
            BindingOperations.GetBindingExpression(NotifyRaiseCheckBox, ToggleButton.IsCheckedProperty)?.UpdateSource();
            BindingOperations.GetBindingExpression(Taked13CCheckBox, ToggleButton.IsCheckedProperty)?.UpdateSource();
            BindingOperations.GetBindingExpression(SulfurSuspectedCheckBox, ToggleButton.IsCheckedProperty)?.UpdateSource();
            BindingOperations.GetBindingExpression(AliquotsTextBox, TextBox.TextProperty)?.UpdateSource();

            Updated?.Invoke();
        }

        protected virtual void Close()
        {
            Window.GetWindow(this).Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (sample != null && SampleIsVirtual)
                sample.Name = null;    // delete it
            Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Save();
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Save();

            // Give some indication to the user that the save button has actually saved. This is just visual.
            SaveButton.IsEnabled = false;
            Task.Delay(200).ContinueWith((task) => Dispatcher.Invoke(() => SaveButton.IsEnabled = true));
        }

        private void MassUnitsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count < 1)
                return;
            if (double.TryParse(MassTextBox.Text, out double mass))
                MassTextBox.Text = GetMassInUnits(GetGrams(mass, (MassUnits)e.RemovedItems[0])).ToString();
        }

        private void InletPortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(InletPortComboBox.SelectedItem is IInletPort ip)) return;

            PortTypeComboBox.ItemsSource = ip.SupportedPortTypes;
            PortTypeComboBox.SelectedValue = ip.PortType;
            if (PortTypeComboBox.SelectedIndex == -1 && PortTypeComboBox.Items.Count > 0)
                PortTypeComboBox.SelectedIndex = 0;
        }

        private void PortTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionView v = (CollectionView)CollectionViewSource.GetDefaultView(ProcessComboBox.ItemsSource);
            v.Filter = (item) =>
            {
                return item is ProcessSequence ps && 
                    PortTypeComboBox.SelectedItem is AeonHacs.InletPortType portType &&
                    ps.PortType == portType;
            };

            ProcessComboBox.SelectedValue = Sample.Process;
            if (ProcessComboBox.SelectedIndex == -1 && ProcessComboBox.Items.Count > 0)
                ProcessComboBox.SelectedValue = ProcessComboBox.Items[0];
            
            // TODO: Should this functionality be completely removed or should 'independentFurnaces' be a
            //           system property that can be set in the settings file.
            var isCombustion = PortTypeComboBox.SelectedItem is AeonHacs.InletPortType portType && 
                portType == AeonHacs.InletPortType.Combustion;
            var independentFurnaces = false;
            NotifyRaiseCheckBox.Visibility = isCombustion && independentFurnaces ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AliquotsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var lines = AliquotsTextBox.Text.Split(Environment.NewLine);
            if (lines.Count() > 3)
            {
                AliquotsTextBox.Text = string.Join(Environment.NewLine, lines[0..3]);
                e.Handled = true;
            }
        }
    }
}
