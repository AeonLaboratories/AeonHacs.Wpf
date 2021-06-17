using HACS.WPF.Behaviors;
using HACS.WPF.Data;
using HACS.WPF.ViewModels;
using Microsoft.Xaml.Behaviors;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace HACS.WPF.Views
{
	public class SettingsPanel : StackPanel
	{
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
			nameof(Source), typeof(object), typeof(SettingsPanel), new FrameworkPropertyMetadata(null, SourceChanged));

		public static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is SettingsPanel sp)
				sp.SourceChanged(e.OldValue, e.NewValue);
		}

		public static readonly DependencyProperty UpdateSourceTriggerProperty = DependencyProperty.Register(
			nameof(UpdateSourceTrigger), typeof(UpdateSourceTrigger), typeof(SettingsPanel), new FrameworkPropertyMetadata(UpdateSourceTrigger.Default, UpdateSourceTriggerChanged));

		public static void UpdateSourceTriggerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is SettingsPanel sp)
				sp.Refresh();
		}

		public object Source { get => GetValue(SourceProperty); set => SetValue(SourceProperty, value); }

		public UpdateSourceTrigger UpdateSourceTrigger { get => (UpdateSourceTrigger)GetValue(UpdateSourceTriggerProperty); set => SetValue(UpdateSourceTriggerProperty, value); }

		protected List<DependencyObject> BoundObjects { get; private set; } = new List<DependencyObject>();

		public SettingsPanel FirstPanel { get; set; }
		protected Stack<object> Sources { get; set; } = new Stack<object>();

		protected virtual void SourceChanged(object oldSource, object newSource)
		{
			bool refresh = false;
			if (!oldSource?.GetType().Equals(newSource?.GetType()) ?? newSource != null)
				refresh = true;
			else if (newSource is ICollection)
				refresh = true;

			if (oldSource is INotifyCollectionChanged oldCollection)
			{
				oldCollection.CollectionChanged -= Refresh;
				refresh = true;
			}
			if (newSource is INotifyCollectionChanged newCollection)
			{
				newCollection.CollectionChanged += Refresh;
				refresh = true;
			}

			if (!refresh)
				return;

			if (newSource is null)
				ResetLayout();
			else
				Refresh();
		}

		protected virtual void Refresh(object sender, NotifyCollectionChangedEventArgs e)
		{
			Refresh();
		}

		protected virtual void Refresh()
		{
			ResetLayout();
			if (Source == null) 
				return;
			PopulateView();

		}

		protected virtual void ResetLayout()
		{
			BoundObjects.ForEach(obj => BindingOperations.ClearAllBindings(obj));
			BoundObjects.Clear();

			Children.Clear();
		}

		// Default is true, property must be marked [Browsable(false)] to hide from Properties window
		bool Browsable(PropertyInfo property) =>
			(property.GetCustomAttribute(typeof(BrowsableAttribute)) as BrowsableAttribute)?.Browsable ?? true;

		bool Editable(PropertyInfo property) =>
			(property.GetCustomAttribute(typeof(EditableAttribute)) as EditableAttribute)?.AllowEdit ??
				property.GetSetMethod()?.IsPublic ?? false;

		protected List<PropertyInfo> GetPropertiesToShow(Type type)
		{
			var properties = type.GetProperties();
			return properties.Where(p =>
				!p.GetGetMethod().IsStatic &&
				Browsable(p) &&
				!typeof(Delegate).IsAssignableFrom(p.PropertyType)
			).ToList();
		}

		protected virtual void PopulateView()
		{
			var firstPanel = FirstPanel ?? this;
			if (this == firstPanel && Sources.Count > 0)
			{
				var button = new Button() { Content = "Back" };
				button.Click += (sender, e) => { firstPanel.Source = firstPanel.Sources.Pop(); };
				Children.Add(button);
			}

			if (Source is ICollection collection)
			{
				PopulateFrom(collection);
			}
			else
			{
				var properties = GetPropertiesToShow(Source.GetType());
				properties.ForEach(property => Populate(property));
			}
		}

		protected virtual void PopulateFrom(ICollection collection)
		{
			var firstPanel = FirstPanel ?? this;

			// generate a TextBox or Button for each item in the collection
			var enumerator = collection.GetEnumerator();
			enumerator.Reset();
			for (int i = 0; i < collection.Count; ++i)
			{
				enumerator.MoveNext();
				var item = enumerator.Current;
				var itemType = item.GetType();
				if (itemType.IsEnum)
				{
					var comboBox = new ComboBox() { ItemsSource = Enum.GetValues(itemType) };
					if (item is null)
						comboBox.SelectedIndex = 0;
					else
						comboBox.SelectedItem = item;

					// todo Bind...

					Children.Add(comboBox);
				}
				else if (itemType.IsPrimitive || itemType == typeof(string))
				{
					var textBox = new TextBox() { Padding = new Thickness(3, 1, 3, 1) };

					// TODO what's the path to a particular Child ?
					//textBox.SetBinding(TextBox.TextProperty, new Binding("???") { Source = this });
					//BoundObjects.Add(textBox);
					textBox.Text = item.ToString();

					Children.Add(textBox);
				}
				else
				{
					var button = new Button();
					var content = "";
					if (collection is IDictionary)
					{
						var dictionaryEnumerator = enumerator as IDictionaryEnumerator;
						content = dictionaryEnumerator.Key.ToString();

						if (dictionaryEnumerator.Value is Core.INamedObject namedObject)
							content += $" ({namedObject?.Name})";
					}
					else if (item is Core.INamedObject namedObject)
						content = $"{namedObject?.Name}";
					else
						content = $"{item}";

					button.Content = content.Replace("_", "__");
					button.Click += (sender, e) => { firstPanel.Sources.Push(firstPanel.Source); firstPanel.Source = item; };
					Children.Add(button);
				}
			}
		}

		protected virtual void Populate(PropertyInfo property)
		{
			//TODO manage exceptions instead of crashing?
			if (property is null)
				return;

			var propertyName = property.Name;
			var displayName = (property.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute)?.DisplayName ??
				(property.GetCustomAttribute(typeof(JsonPropertyAttribute)) as JsonPropertyAttribute)?.PropertyName ?? propertyName;
			var valueType = property.PropertyType;

			//TODO setting to remove or keep the Name properties?
			//Currently remove them
			if (propertyName == nameof(Core.INamedObject.Name))
				return;

			var value = property.GetValue(Source);
			if (value == Source) return;
			var description = (property.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description;
			var editable = Editable(property);

			string bindingPath = $"{nameof(Source)}.{propertyName}";
			var valueBinding = new Binding(bindingPath) { Source = this };
			valueBinding.Mode = editable ? BindingMode.TwoWay : BindingMode.OneWay;
			valueBinding.UpdateSourceTrigger = UpdateSourceTrigger;

			var layoutGrid = new Grid();
			layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto, SharedSizeGroup = "Name" });
			layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
			layoutGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto, SharedSizeGroup = "StandardRow" });

			layoutGrid.Margin = new Thickness(0, 1, 0, 1);
			if (description != null)
				layoutGrid.ToolTip = description;

			// TODO editability of complex objects?
			// if value is a complex object,
			//	1. the value should be a control (button?)
			// 		that opens the object
			//	2. if the value itself is editable (i.e. not its properties)
			//		the 'label' should be a control (button? combobox?)
			//		that lets you select (or create? or remove?) the object

			Label nameLabel = null;
			FrameworkElement valueControl = null;
			DependencyProperty dependencyProperty = null;

			if (valueType == typeof(bool))
			{
				valueControl = new CheckBox() { Content = displayName, IsEnabled = editable };
				dependencyProperty = ToggleButton.IsCheckedProperty;
			}
			else
			{
				if (valueType.IsEnum && editable)   // use a TextBox if not editable
				{
					valueControl = new ComboBox() { ItemsSource = Enum.GetValues(valueType) };
					dependencyProperty = Selector.SelectedItemProperty;
				}
				else if (valueType.IsEnum || valueType.IsPrimitive || valueType == typeof(string))
				{
					valueControl = new TextBox() { Padding = new Thickness(3, 1, 3, 1), IsEnabled = editable };
					valueBinding.UpdateSourceTrigger = UpdateSourceTrigger;
					
					if (valueType == typeof(int))
					{
						valueBinding.ValidationRules.Add(NumericValidationRule.Int);
						Interaction.GetBehaviors(valueControl).Add(new TextBoxValidationBehavior());
						valueBinding.NotifyOnValidationError = true;
					}
					else if (valueType == typeof(double))
					{
						valueBinding.ValidationRules.Add(NumericValidationRule.Double);
						Interaction.GetBehaviors(valueControl).Add(new TextBoxValidationBehavior());
						valueBinding.NotifyOnValidationError = true;
						valueBinding.StringFormat = "0.##########";
					}

					dependencyProperty = TextBox.TextProperty;
				}
				else
				{
					var firstPanel = FirstPanel ?? this;
					if (value is INotifyPropertyChanged model)
					{
						var button = new Button();
						if (model is Core.INamedObject)
						{
							var contentBinding = 
								new Binding($"{valueBinding.Path.Path}.{nameof(Core.INamedObject.Name)}")
								{ 
									Source = this, 
									Converter = Converters.PlainContentConverter.Default,
									FallbackValue = "<nameless>", 
									TargetNullValue = "<nameless>" 
								};
							button.SetBinding(ContentControl.ContentProperty, contentBinding);
						}
						else
							button.Content = "<nameless>";

						//TODO is there a better property to use? Maybe we should make an attached property?
						dependencyProperty = TagProperty;
						button.Click += (sender, e) =>
						{
							firstPanel.Sources.Push(firstPanel.Source);
							firstPanel.Source = button.Tag;
						};
						valueControl = button;
					}
					else
					{
						valueControl = new SettingsPanel() { FirstPanel = firstPanel, Source = value };
						dependencyProperty = SourceProperty;
					}
				}

				nameLabel = new Label() { Content = displayName };
				nameLabel.Padding = new Thickness(0, 0, 10, 0);
				if (valueControl is SettingsPanel)
					nameLabel.VerticalContentAlignment = VerticalAlignment.Top;
				else
					nameLabel.VerticalContentAlignment = VerticalAlignment.Center;

			}

			if (valueControl == null)
				return;

			if (dependencyProperty != null)
			{
				valueControl.SetBinding(dependencyProperty, valueBinding);
				BoundObjects.Add(valueControl);
			}

			if (valueControl is ComboBox cb && value is null)
				cb.SelectedIndex = 0;

			valueControl.HorizontalAlignment = HorizontalAlignment.Stretch;

			if (valueControl is Control contentControl)
				contentControl.VerticalContentAlignment = VerticalAlignment.Center;

			if (nameLabel == null)
			{
				Grid.SetColumn(valueControl, 0);
				Grid.SetColumnSpan(valueControl, 2);
			}
			else
			{
				Grid.SetRow(nameLabel, 0);
				Grid.SetColumn(nameLabel, 0);
				Grid.SetColumn(valueControl, 1);
				layoutGrid.Children.Add(nameLabel);
			}
			Grid.SetRow(valueControl, 0);
			layoutGrid.Children.Add(valueControl);

			Children.Add(layoutGrid);
			//if (!(valueControl is SettingsPanel))
			//	Grid.SetIsSharedSizeScope(valueControl, true);
		}

		public virtual void UpdateSource()
		{
			BoundObjects.ForEach(obj =>
			{
				if (obj is SettingsPanel sp)
					sp.UpdateSource();
				else
				{
					foreach (var expression in BindingOperations.GetSourceUpdatingBindings(obj))
					{
						expression.UpdateSource();
					}
				}
			});
		}
	}
}
