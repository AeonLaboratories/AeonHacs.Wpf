using AeonHacs.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace AeonHacs.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ProtocolEditor.xaml
    /// </summary>
    public partial class ProtocolEditor : UserControl
    {
        //TODO add checklist compatability

        protected static IDictionary<string, Type> ParameterizedSteps { get; set; }

        public ProcessManager ProcessManager { get; protected set; }

        protected ObservableCollection<Protocol> Protocols { get; set; } = new ObservableCollection<Protocol>();

        protected Popup NewStepSelector { get; set; }

        static ProtocolEditor()
        {
            ParameterizedSteps = new Dictionary<string, Type>()
            {
                { "Parameter", typeof(ParameterStep) },
                { "Combustion", typeof(CombustionStep) }
            };
            // Parameterized steps (except for Parameter) are deprecated and being phased out.
            //ParameterizedSteps = AppDomain.CurrentDomain.GetAssemblies().Where(
            //    assembly => !assembly.IsDynamic).SelectMany(
            //    assembly => assembly.GetTypes()).Where(
            //    type => type.IsClass && !type.IsAbstract && typeof(ParameterizedStep).IsAssignableFrom(type)).ToDictionary(
            //        type => type.Name.EndsWith("Step") ? type.Name[0..^4] : type.Name, type => type
            //    );
        }

        public ProtocolEditor()
        {
            InitializeComponent();
        }

        public ProtocolEditor(ProcessManager processManager) : this()
        {
            ProcessManager = processManager;

            LoadSources();
            CreateStepSelectorPopup();

            LoadProtocols();

            if (ProtocolComboBox.HasItems)
                ProtocolComboBox.SelectedIndex = 0;
        }

        protected virtual void LoadSources()
        {
            ProtocolTypeComboBox.ItemsSource = Enum.GetValues(typeof(AeonHacs.InletPortType));

            var selectedProtocolTypeBinding = new Binding($"{nameof(Selector.SelectedItem)}.{nameof(Protocol.PortType)}") { Source = ProtocolComboBox, UpdateSourceTrigger = UpdateSourceTrigger.Explicit };
            ProtocolTypeComboBox.SetBinding(Selector.SelectedItemProperty, selectedProtocolTypeBinding);
        }

        protected string GetDescription(string stepName)
        {
            if (ParameterizedSteps.TryGetValue(stepName, out Type type))
                return StepTypeDescription(type);
            return StepMethodDescription(stepName);
        }

        protected string GetDescription(ProtocolStep step)
        {
            if (!step.Description.IsBlank())
                return step.Description;

            var desc = StepTypeDescription(step.GetType());
            if (!desc.IsBlank()) return desc;

            desc = StepMethodDescription(step.Name);
            return desc;
        }

        protected virtual string StepTypeDescription(Type type) => (type
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute)?.Description;
        protected virtual string StepMethodDescription(string stepName)
        {
            if (ProcessManager.ProcessDictionary.ContainsKey(stepName))
                return (ProcessManager.ProcessDictionary[stepName].Method.
                    GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute)?.Description;

            Notify.Announce($"Invalid Protocol Step Name: " +
                $"\r\n{stepName}." +
                $"\r\nCheck spelling and correct settings file.");

            return stepName;
        }

        protected virtual void CreateStepSelectorPopup()
        {
            var stepNames = new List<string>();
            stepNames.AddRange(ProcessManager.ProcessNames);
            stepNames.RemoveAll(step => ProcessManager.ProcessDictionary[step] == null);
            stepNames.Sort();
            stepNames.InsertRange(0, ParameterizedSteps.Keys);

            var lb = new ListBox() { BorderBrush = SystemColors.MenuHighlightBrush };
            foreach (var stepName in stepNames)
            {
                var displayItem = new ListBoxItem() { Content = stepName };
                if (GetDescription(stepName) is string desc)
                    displayItem.ToolTip = desc;
                lb.Items.Add(displayItem);
            }
            lb.MouseDoubleClick += (sender, e) =>
            {
                if (lb.SelectedItem == null)
                    return;
                if (lb.ItemContainerGenerator.ContainerFromItem(lb.SelectedItem) is ListBoxItem lbi && !lbi.IsFocused)
                    return;

                ProtocolStep pss;
                string stepName = (lb.SelectedItem as ListBoxItem).Content as string;
                if (ParameterizedSteps.TryGetValue(stepName, out Type type))
                    pss = (ProtocolStep)Activator.CreateInstance(type);
                else
                    pss = new ProtocolStep(stepName);

                AddStep(pss);
            };
            lb.SetBinding(HeightProperty, new Binding("ActualWidth") { Source = lb });
            VirtualizingPanel.SetScrollUnit(lb, ScrollUnit.Pixel);

            NewStepSelector = new Popup() { Child = lb };
            NewStepSelector.AllowsTransparency = true;
            NewStepSelector.PlacementTarget = AddButton;
            NewStepSelector.Placement = PlacementMode.Relative;
            NewStepSelector.StaysOpen = false;
        }

        protected virtual void LoadProtocols()
        {
            Protocols.Clear();
            foreach (var protocol in ProcessManager.Protocols.Values)
            {
                AddProtocol(protocol);
            }
            ProtocolComboBox.ItemsSource = Protocols;
            ProtocolComboBox.DisplayMemberPath = "Name";
        }

        protected virtual void AddProtocol(Protocol newProtocol)
        {
            Protocols.Add(newProtocol);
        }

        protected virtual void LoadSteps()
        {
            ProtocolStepsList.Items.Clear();
            if (ProtocolComboBox.SelectedItem is Protocol selectedProtocol)
            {
                foreach (var step in selectedProtocol.Steps)
                {
                    AddStep(step);
                }
            }
        }

        protected virtual void AddStep(ProtocolStep step)
        {
            ListBoxItem displayItem = new();
            if (step is ParameterStep)
            {
                var newStep = (ParameterStep)step.Clone();
                newStep.Description = "";       // save space in settings file.
                var panel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Tag = newStep
                };

                var comboBox = new ComboBox
                {
                    ItemsSource = NamedObject.FindAll<CegsPreferences>().First().DefaultParameters.OrderBy(p => p.ParameterName),
                    DisplayMemberPath = nameof(Parameter.ParameterName),
                    SelectedValuePath = nameof(Parameter.ParameterName),
                    SelectedValue = newStep.Name,
                    IsEditable = true,
                    IsTextSearchEnabled = false
                };

                var comboBoxItemStyle = new Style(typeof(ComboBoxItem));
                comboBoxItemStyle.Setters.Add(new Setter(ToolTipProperty, new Binding(nameof(Parameter.Description))));

                comboBox.Resources.Add(typeof(ComboBoxItem), comboBoxItemStyle);

                // TODO clean this up.
                comboBox.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler((sender, e) =>
                {
                    var text = comboBox.Text;
                    if (comboBox.SelectedItem is Parameter parameter && parameter.ParameterName != text)
                    {
                        var index = 0;
                        foreach (TextChange tc in e.Changes)
                            index = tc.Offset + tc.AddedLength;
                        comboBox.SelectedItem = null;
                        comboBox.Text = text;
                        var tb = (TextBox)comboBox.Template.FindName("PART_EditableTextBox", comboBox);
                        tb.CaretIndex = index;
                    }
                    else if (comboBox.SelectedItem == null)
                    {
                        newStep.Name = text;
                        newStep.Description = null;
                    }
                }));

                comboBox.SelectionChanged += (sender, e) =>
                {
                    if (comboBox.SelectedItem is Parameter parameter)
                    {
                        newStep.Name = parameter.ParameterName;
                        newStep.Description = parameter.Description;
                        newStep.Value = parameter.Value;
                    }
                };
                if (string.IsNullOrWhiteSpace(newStep.Name))
                    comboBox.SelectedIndex = 0;
                else if (comboBox.SelectedItem == null)
                    comboBox.Text = newStep.Name;

                var textBox = new TextBox() { VerticalContentAlignment = VerticalAlignment.Center };
                textBox.SetBinding(TextBox.TextProperty, new Binding(nameof(ParameterStep.Value)) { Source = newStep });

                TextBlock setTextBlock = new TextBlock() { Text = "Set ", LineStackingStrategy = LineStackingStrategy.BlockLineHeight, VerticalAlignment = VerticalAlignment.Center };
                TextBlock toTextBlock = new TextBlock() { Text = " to ", LineStackingStrategy = LineStackingStrategy.BlockLineHeight, VerticalAlignment = VerticalAlignment.Center };
                panel.Children.Add(setTextBlock);
                panel.Children.Add(comboBox);
                panel.Children.Add(toTextBlock);
                panel.Children.Add(textBox);

                displayItem.Content = panel;
                // TODO: Update "ToolTip" to be a popup that contains a textbox so description can be edited
                displayItem.SetBinding(ToolTipProperty, new Binding(nameof(ParameterStep.Description)) { Source = newStep });
                displayItem.ToolTip = newStep;
            }
            else if (step is ParameterizedStep)
            {
                var newStep = (ParameterizedStep)step.Clone();
                newStep.Description = "";       // save space in settings file.

                var headerTextBox = new TextBox() { BorderThickness = new Thickness(0), Background = Brushes.Transparent };
                headerTextBox.SetBinding(TextBox.TextProperty, new Binding(nameof(ParameterizedStep.Name)) { Source = newStep });
                displayItem.Content = new GroupBox()
                {
                    Header = headerTextBox,
                    Content = new SettingsPanel(true) { Source = newStep }
                };
                displayItem.ToolTip = GetDescription(newStep);
            }
            else
            {
                displayItem.Content = step;
                displayItem.ToolTip = GetDescription(step);
            }

            if (ProtocolStepsList.SelectedIndex >= 0)
                ProtocolStepsList.Items.Insert(ProtocolStepsList.SelectedIndex, displayItem);
            else
                ProtocolStepsList.Items.Add(displayItem);
        }

        protected virtual void LoadChecklist()
        {
            ProtocolChecklistTextBox.Clear();
            var sb = new StringBuilder();
            if (ProtocolComboBox.SelectedItem is Protocol selectedProtocol && selectedProtocol.CheckList != null)
            {
                foreach (var check in selectedProtocol.CheckList)
                {
                    sb.AppendLine(check);
                }
            }
            ProtocolChecklistTextBox.Text = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
            ProtocolChecklistTextBox.InvalidateMeasure();
        }

        protected virtual void Save()
        {
            List<ProtocolStep> newSteps = new List<ProtocolStep>();
            foreach (var item in ProtocolStepsList.Items.OfType<ListBoxItem>())
            {
                if (item.Content is StackPanel p)
                {
                    if (p.Tag is ParameterStep ps)
                        newSteps.Add(ps);
                }
                else if (item.Content is GroupBox gb)
                {
                    if (gb.Content is SettingsPanel sp && sp.Source is ProtocolStep step)
                        newSteps.Add(step);
                }
                else if (item.Content is ProtocolStep step)
                    newSteps.Add(step);
            }
            (ProtocolComboBox.SelectedItem as Protocol).Steps = newSteps;
            (ProtocolComboBox.SelectedItem as Protocol).CheckList = ProtocolChecklistTextBox.Text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
            BindingOperations.GetBindingExpression(ProtocolTypeComboBox, Selector.SelectedItemProperty).UpdateSource();
            (ProtocolComboBox.SelectedItem as Protocol).Name = ProtocolComboBox.Text;
            ProcessManager.Protocols = Protocols.ToDictionary(protocol => protocol.Name, protocol => protocol);
        }

        protected virtual void Close()
        {
            Window.GetWindow(this).Close();
        }

        private void ProtocolComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSteps();
            LoadChecklist();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            string name = "new protocol";
            int suffix = 1;
            while (Protocols.Where(protocol => protocol.Name == name + suffix).Count() > 0)
                suffix++;

            Protocols.Add(new Protocol(name + suffix));
            ProtocolComboBox.SelectedIndex = ProtocolComboBox.Items.Count - 1;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProtocolComboBox.SelectedItem is Protocol protocol)
            {
                int index = ProtocolComboBox.Items.IndexOf(protocol);
                Protocols.Remove(protocol);
                ProtocolComboBox.SelectedIndex = -1;
                if (index < ProtocolComboBox.Items.Count)
                    ProtocolComboBox.SelectedIndex = index;
                else if (ProtocolComboBox.Items.Count > 0)
                    ProtocolComboBox.SelectedIndex = ProtocolComboBox.Items.Count - 1;
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProtocolStepsList.SelectedItem is ListBoxItem selectedItem)
            {
                int index = ProtocolStepsList.Items.IndexOf(selectedItem);
                if (index == 0)
                {
                    FocusManager.SetFocusedElement(Window.GetWindow(this), ProtocolStepsList);
                    return;
                }
                ProtocolStepsList.Items.RemoveAt(index);
                ProtocolStepsList.Items.Insert(index - 1, selectedItem);
                ProtocolStepsList.SelectedItem = selectedItem;
                FocusManager.SetFocusedElement(Window.GetWindow(this), ProtocolStepsList);
            }
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProtocolStepsList.SelectedItem is ListBoxItem selectedItem)
            {
                int index = ProtocolStepsList.Items.IndexOf(selectedItem);
                if (index == ProtocolStepsList.Items.Count - 1)
                {
                    FocusManager.SetFocusedElement(Window.GetWindow(this), ProtocolStepsList);
                    return;
                }
                ProtocolStepsList.Items.RemoveAt(index);
                ProtocolStepsList.Items.Insert(index + 1, selectedItem);
                ProtocolStepsList.SelectedItem = selectedItem;
                FocusManager.SetFocusedElement(Window.GetWindow(this), ProtocolStepsList);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NewStepSelector.HorizontalOffset = AddButton.Width;
            NewStepSelector.IsOpen = true;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProtocolStepsList.SelectedItem is ListBoxItem selectedItem)
            {
                int index = ProtocolStepsList.Items.IndexOf(selectedItem);
                ProtocolStepsList.Items.Remove(selectedItem);
                if (index < ProtocolStepsList.Items.Count && ProtocolStepsList.Items.GetItemAt(index) is ListBoxItem next)
                    ProtocolStepsList.SelectedItem = next;
                else if (ProtocolStepsList.Items.Count > 0)
                    ProtocolStepsList.SelectedIndex = ProtocolStepsList.Items.Count - 1;
                FocusManager.SetFocusedElement(Window.GetWindow(this), ProtocolStepsList);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
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
            Task.Delay(1000).ContinueWith((task) => Dispatcher.Invoke(() => SaveButton.IsEnabled = true));
        }

        private void ProtocolStepsList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listBox = (ListBox)sender;
            var origin = e.OriginalSource as DependencyObject;

            // If the click is inside an editor (TextBox / ComboBox), don't mess with it.
            if (origin != null)
            {
                if (FindAncestor<TextBoxBase>(origin) != null ||
                    FindAncestor<ComboBox>(origin) != null)
                {
                    return; // let normal editing / selection happen
                }
            }

            // Find the ListBoxItem that was clicked, if any
            var item = ItemsControl.ContainerFromElement(listBox, origin) as ListBoxItem;

            if (item == null)
            {
                // Optional: click on empty space clears selection too
                // listBox.SelectedIndex = -1;
                return;
            }

            if (item.IsSelected)
            {
                // Toggle off: no selection
                item.IsSelected = false;
                listBox.SelectedIndex = -1;
                e.Handled = true;   // stop normal selection logic
            }
            // else: let WPF handle selection normally
        }

        // Simple visual-tree helper
        private static T FindAncestor<T>(DependencyObject d) where T : DependencyObject
        {
            while (d != null && d is not T)
            {
                d = VisualTreeHelper.GetParent(d);
            }
            return d as T;
        }

    }
}
