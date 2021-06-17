using HACS.Core;
using HACS.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HACS.WPF.Views
{
	/// <summary>
	/// Interaction logic for SettingsPage.xaml
	/// </summary>
	public partial class SettingsPage : UserControl
	{
		protected Core.HacsBase Hacs { get; set; }

		protected List<List<Core.HacsComponent>> Components { get; set; } = new List<List<Core.HacsComponent>>();

		protected SettingsPage()
		{
			InitializeComponent();
		}

		public SettingsPage(Core.HacsBase hacs) : this()
		{
			Hacs = hacs;
			GetComponents();
			CreateTree();
		}

		protected virtual void GetComponents()
		{
			foreach (var componentsOfType in Core.NamedObject.CachedList<Core.HacsComponent>().OrderBy(x => x.Name, new AlphanumericComparer()).OrderBy(x => x.GetType().Name).GroupBy(x => x.GetType().Name))
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

		string prevFilter = "";
		protected virtual void Filter(string filter)
		{
			if (!filter.StartsWith(prevFilter))
			{
				SettingsTree.Items.Filter += (list) => { return true; };
				foreach (var list in SettingsTree.Items.OfType<TreeViewItem>())
				{
					list.Items.Filter += (component) => { return true; };
				}
			}

			if (!string.IsNullOrWhiteSpace(filter))
			{
				foreach (var list in SettingsTree.Items.OfType<TreeViewItem>())
				{
					list.Items.Filter += (component) => { return (component as INamedObject).Name.ToLower().Contains(filter.ToLower()); };
				}
				SettingsTree.Items.Filter += (item) =>
				{
					if (item is TreeViewItem type)
						return type.Items.Count != 0;
					if (item is INamedObject component)
						return component.Name.ToLower().Contains(filter.ToLower());
					return false;
				};
			}

			prevFilter = filter;
		}

		private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			Filter((sender as TextBox).Text);
		}

		private void SettingsTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (e.NewValue is TreeViewItem)
				return;
			if (e.NewValue is INotifyPropertyChanged obj)
				SettingsPanel.Source = obj;
		}
	}
}
