using HACS.Components;
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

namespace HACS.WPF.Views
{
    /// <summary>
    /// Interaction logic for ProcessSequenceEditor.xaml
    /// </summary>
    public partial class ProcessSequenceEditor : UserControl
	{
		//TODO add checklist compatability

		protected static IDictionary<string, Type> ParameterizedSteps { get; set; }

		public ProcessManager ProcessManager { get; protected set; }

		protected ObservableCollection<ProcessSequence> ProcessSequences { get; set; } = new ObservableCollection<ProcessSequence>();

		protected Popup NewStepSelector { get; set; }

		static ProcessSequenceEditor()
		{
			ParameterizedSteps = AppDomain.CurrentDomain.GetAssemblies().Where(
				assembly => !assembly.IsDynamic).SelectMany(
				assembly => assembly.GetTypes()).Where(
				type => type.IsClass && !type.IsAbstract && typeof(ParameterizedStep).IsAssignableFrom(type)).ToDictionary(
					type => type.Name.EndsWith("Step") ? type.Name[0..^4] : type.Name, type => type
				);
		}

		public ProcessSequenceEditor()
		{
			InitializeComponent();
		}

		public ProcessSequenceEditor(ProcessManager processManager) : this()
		{
			ProcessManager = processManager;

			LoadSources();
			CreateStepSelectorPopup();

			LoadSequences();

			if (ProcessComboBox.HasItems)
				ProcessComboBox.SelectedIndex = 0;
		}

        protected virtual void LoadSources()
		{
			SourceComboBox.ItemsSource = Enum.GetValues(typeof(InletPort.Type));

			var selectedSourceBinding = new Binding($"{nameof(Selector.SelectedItem)}.{nameof(ProcessSequence.PortType)}") { Source = ProcessComboBox, UpdateSourceTrigger = UpdateSourceTrigger.Explicit };
			SourceComboBox.SetBinding(Selector.SelectedItemProperty, selectedSourceBinding);
		}

		protected string GetDescription(string stepName)
		{
			if (ParameterizedSteps.TryGetValue(stepName, out Type type))
				return (type.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute)?.Description;
			return (ProcessManager.ProcessDictionary[stepName].Method.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute)?.Description;
		}

		protected virtual void CreateStepSelectorPopup()
		{
			var stepNames = new List<string>();
			stepNames.AddRange(ProcessManager.ProcessNames);
			stepNames.RemoveAll(step => ProcessManager.ProcessDictionary[step] == null);
			stepNames.InsertRange(0, ParameterizedSteps.Keys);

			var lb = new ListBox();
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

				ProcessSequenceStep pss;
				string stepName = (lb.SelectedItem as ListBoxItem).Content as string;
				if (ParameterizedSteps.TryGetValue(stepName, out Type type))
					pss = (ProcessSequenceStep)Activator.CreateInstance(type);
				else
					pss = new ProcessSequenceStep(stepName);

				AddStep(pss);
			};
			lb.BorderThickness = new Thickness(0);

			var sv = new ScrollViewer() { Content = lb };
			sv.SetBinding(HeightProperty, new Binding("ActualWidth") { Source = sv });
			sv.PreviewMouseWheel += (sender, e) =>
			{
				sv.ScrollToVerticalOffset(sv.VerticalOffset - e.Delta);
				e.Handled = true;
			};

			var b = new Border()
			{
				BorderBrush = SystemColors.MenuHighlightBrush,
				BorderThickness = new Thickness(1)
			};
			b.Child = sv;

			NewStepSelector = new Popup() { Child = b };
			NewStepSelector.AllowsTransparency = true;
			NewStepSelector.PlacementTarget = AddButton;
			NewStepSelector.Placement = PlacementMode.Relative;
			NewStepSelector.StaysOpen = false;
		}

		protected virtual void LoadSequences()
		{
			ProcessSequences.Clear();
			foreach (var sequence in ProcessManager.ProcessSequences.Values)
			{
				AddProcess(sequence);
			}
			ProcessComboBox.ItemsSource = ProcessSequences;
			ProcessComboBox.DisplayMemberPath = "Name";
		}

		protected virtual void AddProcess(ProcessSequence newSequence)
		{
			ProcessSequences.Add(newSequence);
		}

		protected virtual void LoadSteps()
		{
			ProcessStepsList.Items.Clear();
			if (ProcessComboBox.SelectedItem is ProcessSequence selectedSequence)
			{
				foreach (var step in selectedSequence.Steps)
				{
					AddStep(step);
				}
			}
		}

		protected virtual void AddStep(ProcessSequenceStep step)
		{
			ListBoxItem displayItem = new ListBoxItem();
			if (step is ParameterizedStep)
			{
				var headerTextBox = new TextBox() { BorderThickness = new Thickness(0), Background = Brushes.Transparent };
				headerTextBox.SetBinding(TextBox.TextProperty, new Binding(nameof(ParameterizedStep.Name)) { Source = step, UpdateSourceTrigger = UpdateSourceTrigger.Explicit });
				displayItem.Content = new GroupBox()
				{
					Header = headerTextBox,
					Content = new SettingsPanel() { UpdateSourceTrigger = UpdateSourceTrigger.Explicit, Source = step }
				};
			}
			else
				displayItem.Content = step;

			ProcessStepsList.Items.Add(displayItem);
		}

		protected virtual void LoadChecklist()
        {
			ProcessChecklistTextBox.Clear();
			var sb = new StringBuilder();
			if (ProcessComboBox.SelectedItem is ProcessSequence selectedSequence && selectedSequence.CheckList != null)
            {
				foreach (var check in selectedSequence.CheckList)
                {
					sb.AppendLine(check);
                }
            }
			ProcessChecklistTextBox.Text = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
			ProcessChecklistTextBox.InvalidateMeasure();
        }

		protected virtual void Save()
		{
			List<ProcessSequenceStep> newSteps = new List<ProcessSequenceStep>();
			foreach (var item in ProcessStepsList.Items.OfType<ListBoxItem>())
			{
				if (item.Content is GroupBox gb)
				{
					if (gb.Header is TextBox tb)
						BindingOperations.GetBindingExpression(tb, TextBox.TextProperty).UpdateSource();
					if (gb.Content is SettingsPanel sp)
					{
						sp.UpdateSource();
						if (sp.Source is ProcessSequenceStep step)
							newSteps.Add(step);
					}
				}
				else if (item.Content is ProcessSequenceStep step)
					newSteps.Add(step);
			}
			(ProcessComboBox.SelectedItem as ProcessSequence).Steps = newSteps;
			(ProcessComboBox.SelectedItem as ProcessSequence).CheckList = ProcessChecklistTextBox.Text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
			BindingOperations.GetBindingExpression(SourceComboBox, Selector.SelectedItemProperty).UpdateSource();
			(ProcessComboBox.SelectedItem as ProcessSequence).Name = ProcessComboBox.Text;
			ProcessManager.ProcessSequences = ProcessSequences.ToDictionary(sequence => sequence.Name, sequence => sequence);
		}

		protected virtual void Close()
		{
			Window.GetWindow(this).Close();
		}

		private void ProcessComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			LoadSteps();
			LoadChecklist();
		}

		private void NewButton_Click(object sender, RoutedEventArgs e)
		{
			string name = "sequence";
			int suffix = 1;
			while (ProcessSequences.Where(sequence => sequence.Name == name + suffix).Count() > 0)
				suffix++;

			ProcessSequences.Add(new ProcessSequence(name + suffix));
			ProcessComboBox.SelectedIndex = ProcessComboBox.Items.Count - 1;
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			if (ProcessComboBox.SelectedItem is ProcessSequence sequence)
			{
				int index = ProcessComboBox.Items.IndexOf(sequence);
				ProcessSequences.Remove(sequence);
				ProcessComboBox.SelectedIndex = -1;
				if (index < ProcessComboBox.Items.Count)
					ProcessComboBox.SelectedIndex = index;
				else if (ProcessComboBox.Items.Count > 0)
					ProcessComboBox.SelectedIndex = ProcessComboBox.Items.Count - 1;
			}
		}

		private void UpButton_Click(object sender, RoutedEventArgs e)
		{
			if (ProcessStepsList.SelectedItem is ListBoxItem selectedItem)
			{
				int index = ProcessStepsList.Items.IndexOf(selectedItem);
				if (index == 0)
				{
					FocusManager.SetFocusedElement(Window.GetWindow(this), ProcessStepsList);
					return;
				}
				ProcessStepsList.Items.RemoveAt(index);
				ProcessStepsList.Items.Insert(index - 1, selectedItem);
				ProcessStepsList.SelectedItem = selectedItem;
				FocusManager.SetFocusedElement(Window.GetWindow(this), ProcessStepsList);
			}
		}

		private void DownButton_Click(object sender, RoutedEventArgs e)
		{
			if (ProcessStepsList.SelectedItem is ListBoxItem selectedItem)
			{
				int index = ProcessStepsList.Items.IndexOf(selectedItem);
				if (index == ProcessStepsList.Items.Count - 1)
				{
					FocusManager.SetFocusedElement(Window.GetWindow(this), ProcessStepsList);
					return;
				}
				ProcessStepsList.Items.RemoveAt(index);
				ProcessStepsList.Items.Insert(index + 1, selectedItem);
				ProcessStepsList.SelectedItem = selectedItem;
				FocusManager.SetFocusedElement(Window.GetWindow(this), ProcessStepsList);
			}
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			NewStepSelector.HorizontalOffset = AddButton.Width;
			NewStepSelector.IsOpen = true;
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			if (ProcessStepsList.SelectedItem is ListBoxItem selectedItem)
			{
				int index = ProcessStepsList.Items.IndexOf(selectedItem);
				ProcessStepsList.Items.Remove(selectedItem);
				if (index < ProcessStepsList.Items.Count && ProcessStepsList.Items.GetItemAt(index) is ListBoxItem next)
					ProcessStepsList.SelectedItem = next;
				else if (ProcessStepsList.Items.Count > 0)
					ProcessStepsList.SelectedIndex = ProcessStepsList.Items.Count - 1;
				FocusManager.SetFocusedElement(Window.GetWindow(this), ProcessStepsList);
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
	}
}
