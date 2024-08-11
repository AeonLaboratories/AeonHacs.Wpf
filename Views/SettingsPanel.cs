using AeonHacs.Wpf.Behaviors;
using AeonHacs.Wpf.Data;
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
using System.Windows.Input;

namespace AeonHacs.Wpf.Views
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

        protected Stack<object> Sources { get; set; } = new Stack<object>();

        /// <summary>
        /// Shows or hides properties called 'Name'
        /// </summary>
        public bool HideNames { get; set; } = true;

        public bool ExpandAll { get; set; } = false;

        public SettingsPanel() { }
        public SettingsPanel(bool expandAll) { ExpandAll = expandAll; }
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
                /* !p.GetGetMethod().IsStatic && */
                Browsable(p) &&
                !typeof(Delegate).IsAssignableFrom(p.PropertyType)
            ).ToList();
        }

        protected virtual void PopulateView()
        {
            if (Source is ICollection collection)
            {
                PopulateFrom(collection);
            }
            else
            {
                var properties = GetPropertiesToShow(Source.GetType());
                properties.ForEach(Populate);
            }
        }

        protected virtual void PopulateFrom(ICollection collection)
        {
            // generate a TextBox or Button for each item in the collection
            var enumerator = collection.GetEnumerator();
            enumerator.Reset();
            for (int i = 0; i < collection.Count; ++i)
            {
                enumerator.MoveNext();
                var item = enumerator.Current;
                if (item != null)
                {
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
                        var component = item;

                        if (collection is IDictionary)
                        {
                            var dictionaryEnumerator = enumerator as IDictionaryEnumerator;
                            content = dictionaryEnumerator.Key.ToString();

                            if (dictionaryEnumerator.Value is AeonHacs.INamedObject namedObject)
                            {
                                content += $" ({namedObject?.Name})";
                                component = namedObject;
                            }
                        }
                        else if (item is AeonHacs.INamedObject namedObject)
                        {
                            content = $"{namedObject?.Name}";
                            component = namedObject;
                        }
                        else
                            content = $"{item}";

                        button.Content = content.Replace("_", "__");
                        View.SetComponent(button, component as INotifyPropertyChanged);
                        button.Click += (sender, e) =>
                        {
                            var topLevelPanel = button.GetSelfAndAncestors().Where(obj => obj is SettingsPanel).Cast<SettingsPanel>().Last();
                            topLevelPanel.Source = component;
                        };
                        Children.Add(button);
                    }
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

            if (HideNames && propertyName == nameof(AeonHacs.INamedObject.Name))
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
            //    1. the value should be a control (button?)
            //         that opens the object
            //    2. if the value itself is editable (i.e. not its properties)
            //        the 'label' should be a control (button? combobox?)
            //        that lets you select (or create? or remove?) the object

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
                else if (valueType.IsEnum || valueType.IsPrimitive || valueType == typeof(string) || valueType == typeof(DateTime))
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
                        valueBinding.StringFormat = "G10";
                    }
                    else if (valueType == typeof(DateTime))
                    {
                        valueBinding.ValidationRules.Add(DateTimeValidationRule.Shared);
                        Interaction.GetBehaviors(valueControl).Add(new TextBoxValidationBehavior());
                        valueBinding.NotifyOnValidationError = true;
                        valueBinding.StringFormat = "dd/MM/yyyy HH:mm:ss";
                    }

                    dependencyProperty = TextBox.TextProperty;
                }
                else
                {
                    if (value is INotifyPropertyChanged model && !ExpandAll)
                    {
                        var button = new Button();
                        if (model is AeonHacs.INamedObject)
                        {
                            var contentBinding =
                                new Binding($"{valueBinding.Path.Path}.{nameof(AeonHacs.INamedObject.Name)}")
                                {
                                    Source = this,
                                    Converter = Converters.PlainContentConverter.Default,
                                    FallbackValue = displayName,
                                    TargetNullValue = displayName
                                };
                            button.SetBinding(ContentControl.ContentProperty, contentBinding);
                        }
                        else
                            button.Content = displayName;

                        //TODO is there a better property to use? Maybe we should make an attached property?
                        dependencyProperty = View.ComponentProperty;
                        button.Click += (sender, e) => { Source = View.GetComponent(button); };
                        valueControl = button;
                    }
                    else
                    {
                        valueControl = new SettingsPanel(ExpandAll) { Source = value };
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

                //TODO add support for multiline textboxes
                if (valueControl is TextBox tb && !tb.AcceptsReturn && editable)
                {
                    var be = tb.GetBindingExpression(dependencyProperty);
                    tb.KeyDown += CaptureEnter;
                    tb.TextChanged += MaintainCaret;

                    void CaptureEnter(object sender, KeyEventArgs e)
                    {
                        if (e.Key == Key.Enter)
                        {
                            be.UpdateSource();
                            e.Handled = true;
                        }
                    }

                    void MaintainCaret(object sender, TextChangedEventArgs e)
                    {
                        if (e.Changes.LastOrDefault() is TextChange tc)
                        {
                            tb.CaretIndex = tc.Offset + tc.AddedLength;
                        }
                    }
                }
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
            //    Grid.SetIsSharedSizeScope(valueControl, true);
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
