using AeonHacs;
using AeonHacs.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AeonHacs.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        #region FilterText
        public static readonly DependencyProperty FilterTextProperty = DependencyProperty.Register
            (nameof(FilterText), typeof(string), typeof(SettingsPage), new PropertyMetadata("", FilterTextChanged));

        private static void FilterTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SettingsPage p)
                p.Filter();
        }

        public string FilterText { get => (string)GetValue(FilterTextProperty); set => SetValue(FilterTextProperty, value); }
        #endregion FilterText

        #region MatchCase
        public static readonly DependencyProperty MatchCaseProperty = DependencyProperty.Register
            (nameof(MatchCase), typeof(bool), typeof(SettingsPage), new PropertyMetadata(false, MatchCaseChanged));

        private static void MatchCaseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SettingsPage p)
                p.Filter();
        }

        public bool MatchCase { get => (bool)GetValue(MatchCaseProperty); set => SetValue(MatchCaseProperty, value); }
        #endregion MatchCase

        protected AeonHacs.HacsBase Hacs { get; set; }

        protected List<List<AeonHacs.HacsComponent>> Components { get; set; } = new List<List<AeonHacs.HacsComponent>>();

        protected SettingsPage()
        {
            InitializeComponent();
        }

        public SettingsPage(AeonHacs.HacsBase hacs) : this()
        {
            Hacs = hacs;
            GetComponents();
            CreateTree();
            SetupFilter();
        }

        protected virtual void GetComponents()
        {
            foreach (var componentsOfType in AeonHacs.NamedObject.CachedList<AeonHacs.HacsComponent>().OrderBy(x => x.Name, new AlphanumericComparer()).OrderBy(x => x.GetType().Name).GroupBy(x => x.GetType().Name))
            {
                Components.Add(componentsOfType.ToList());
            }
        }

        protected virtual void CreateTree()
        {
            SettingsTree.Items.Clear();

            Components?.ForEach(listOfComponents =>
            {
                var listItem = new TreeViewItem();
                listOfComponents?.ToList().ForEach(component =>
                {
                    if (component != Hacs)
                        listItem.Items.Add(component);
                });
                if (listItem.Items.Count > 0)
                {
                    listItem.Header = listOfComponents.FirstOrDefault().GetType().Name.Plural();
                    listItem.DisplayMemberPath = "Name";
                    SettingsTree.Items.Add(listItem);
                }
            });
        }

        protected virtual void Filter()
        {
            totalMatches = 0;
            var filter = SettingsTree.Items.Filter;
            SettingsTree.Items.Filter = null;
            foreach (var list in SettingsTree.Items.OfType<TreeViewItem>())
            {
                list.IsExpanded = false;
                list.Items.Filter = list.Items.Filter;
            }
            SettingsTree.Items.Filter = filter;
        }

        int totalMatches { get; set; }
        protected virtual void SetupFilter()
        {
            foreach (var list in SettingsTree.Items.OfType<TreeViewItem>())
            {
                list.Items.Filter = ContainsFilterText;
            }
            SettingsTree.Items.Filter = HasVisibleChildren;

            bool ContainsFilterText(object toFilter)
            {
                if (string.IsNullOrWhiteSpace(FilterText))
                {
                    totalMatches++;
                    return true;
                }
                if (toFilter is INamedObject no && no.Name.Contains(FilterText, MatchCase ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase))
                {
                    totalMatches++;
                    return true;
                }
                return false;
            }

            bool HasVisibleChildren(object toFilter)
            {
                if (string.IsNullOrWhiteSpace(FilterText))
                    return true;
                if (toFilter is TreeViewItem tvi && tvi.Items.Count > 0)
                {
                    if (totalMatches <= 15)
                        tvi.IsExpanded = true;
                    return true;
                }
                return false;
            }
        }

        private void SettingsTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem)
                return;
            if (e.NewValue is INotifyPropertyChanged obj)
            {
                Breadcrumbs.RootObject = obj;
            }
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Clear();
            SearchTextBox.Focus();
        }
    }
}
