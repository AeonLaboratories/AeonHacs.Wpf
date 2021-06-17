using HACS.Components;
using HACS.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HACS.WPF.Views
{
    /// <summary>
    /// Interaction logic for RunSamplesSelector.xaml
    /// </summary>
    public partial class SampleSelector : UserControl
    {
        public List<IInletPort> SelectedSamples { get; protected set; } = new List<IInletPort>();

        protected ObservableCollection<IInletPort> Available = new ObservableCollection<IInletPort>();
        protected ObservableCollection<IInletPort> Selected = new ObservableCollection<IInletPort>();

        protected ICollectionView AvailableView { get; set; }

        protected ObservableCollection<CheckBox> Checks = new ObservableCollection<CheckBox>();

        protected AlphanumericComparer Comparer = new AlphanumericComparer();

        public SampleSelector(bool selectAll = false)
        {
            InitializeComponent();

            Available = new ObservableCollection<IInletPort>(GetLoadedIPs());

            if (selectAll)
            {
                foreach (IInletPort ip in Available)
                    Selected.Add(ip);
            }
            
            SelectedListBox.ItemsSource = Selected;
            AvailableListBox.ItemsSource = Available;

            AvailableView = CollectionViewSource.GetDefaultView(Available);
            AvailableView.Filter = (ip) => !Selected.Contains(ip);

            ChecklistPanel.ItemsSource = Checks;
            UpdateChecklist();
        }

        protected virtual void UpdateChecklist()
        {
            var processStrings = new List<string>();
            var processes = new List<ProcessSequence>();
            var checklist = new List<string>();
            foreach (var ip in Selected)
                processStrings.Add(ip.Sample.Process);
            processes = NamedObject.FindAll<ProcessSequence>(processStrings);
            processes.ForEach(p =>
            {
                if (p != null)
                    checklist = checklist.Union(p.CheckList).ToList();
            });

            for (int i = Checks.Count - 1; i >= 0; --i)
            {
                string check = Checks[i].Content as string;

                if (checklist.Contains(check))
                    checklist.Remove(check);
                else
                    Checks.RemoveAt(i);
            }

            checklist.ForEach(checkListItem =>
            {
                var cb = new CheckBox() { Content = checkListItem };
                cb.Checked += (sender, e) => EnableDisableOK();
                cb.Unchecked += (sender, e) => EnableDisableOK();
                Checks.Add(cb);
            });

            EnableDisableOK();
        }

        protected virtual void EnableDisableOK() =>
            OKButton.IsEnabled = Checks.All(cb => cb.IsChecked ?? false);

        protected virtual IEnumerable<IInletPort> GetLoadedIPs() =>
            NamedObject.FindAll<IInletPort>(
                ip => (!ip?.Sample?.LabId.IsBlank() ?? false) &&
                (ip.State == LinePort.States.Loaded ||
                ip.State == LinePort.States.Prepared))
            .OrderBy(ip => ip.Name, Comparer);

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = AvailableListBox.SelectedIndex;
            if (selectedIndex == -1)
                return;

            foreach (IInletPort ip in AvailableListBox.SelectedItems)
                Selected.Add(ip);

            AvailableView.Refresh();

            var total = AvailableListBox.Items.Count;

            if (selectedIndex >= total && total > 0)
                AvailableListBox.SelectedIndex = total - 1;
            else if (selectedIndex < 0 && total > 0)
                AvailableListBox.SelectedIndex = 0;
            else
                AvailableListBox.SelectedIndex = selectedIndex;

            UpdateChecklist();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = SelectedListBox.SelectedIndex;
            if (selectedIndex == -1)
                return;

            var list = SelectedListBox.SelectedItems;
            for (int i = 0; i < list.Count;)
                Selected.Remove((IInletPort)list[i]);

            AvailableView.Refresh();

            var total = SelectedListBox.Items.Count;

            if (selectedIndex >= total && total > 0)
                SelectedListBox.SelectedIndex = total - 1;
            else if (selectedIndex < 0 && total > 0)
                SelectedListBox.SelectedIndex = 0;
            else
                SelectedListBox.SelectedIndex = selectedIndex;

            UpdateChecklist();
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(SelectedListBox.SelectedItem is IInletPort ip)) return;

            var index = Selected.IndexOf(ip);
            if (index - 1 >= 0)
                Selected.Move(index, index - 1);

            UpdateChecklist();
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(SelectedListBox.SelectedItem is IInletPort ip)) return;

            var index = Selected.IndexOf(ip);
            if (index + 1 < Selected.Count)
                Selected.Move(index, index + 1);

            UpdateChecklist();
        }

        protected virtual void Save() =>
            SelectedSamples = Selected.ToList();

        protected virtual void OK()
        {
            Save();
            Close();
        }

        protected virtual void Close() =>
            Window.GetWindow(this).Close();

        private void OKButton_Click(object sender, RoutedEventArgs e) =>
            OK();

        private void CancelButton_Click(object sender, RoutedEventArgs e) =>
            Close();

        private void ListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var lb = sender as ListBox;
            if (lb.SelectedItem == null)
                return;
            if (lb.ItemContainerGenerator.ContainerFromItem(lb.SelectedItem) is ListBoxItem lbi && !lbi.IsFocused)
                return;

            if (lb == AvailableListBox)
                AddButton_Click(lb, e);
            else if (lb == SelectedListBox)
                RemoveButton_Click(lb, e);
        }
    }
}
