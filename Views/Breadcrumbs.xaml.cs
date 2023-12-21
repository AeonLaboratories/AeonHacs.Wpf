using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AeonHacs.Wpf.Views
{
    /// <summary>
    /// Interaction logic for Breadcrumbs.xaml
    /// </summary>
    public partial class Breadcrumbs : UserControl
    {
        #region RootObject
        public static readonly DependencyProperty RootObjectProperty = DependencyProperty.Register
            (nameof(RootObject), typeof(object), typeof(Breadcrumbs), new PropertyMetadata(RootObjectChanged));

        private static void RootObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Breadcrumbs b)
                b.Refresh();
        }

        public object RootObject { get => GetValue(RootObjectProperty); set => SetValue(RootObjectProperty, value); }
        #endregion RootObject

        #region SelectedItem
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register
            (nameof(SelectedItem), typeof(object), typeof(Breadcrumbs), new FrameworkPropertyMetadata(SelectedItemChanged));

        private static void SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Breadcrumbs b)
                b.Update();
        }

        public object SelectedItem { get => GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }
        #endregion SelectedItem

        public Breadcrumbs()
        {
            InitializeComponent();
        }

        protected virtual void Refresh()
        {
            Container.Children.Clear();
            var button = new Button();
            View.SetComponent(button, RootObject as INotifyPropertyChanged);
            button.Click += SelectItem;
            Container.Children.Add(button);
            SelectedItem = RootObject;
        }

        public virtual void Update()
        {
            var count = Container.Children.Count;
            if (count != 0)
            {
                for (int i = 0; i < count; ++i)
                {
                    var element = Container.Children[i];
                    if (View.GetComponent(element) == SelectedItem)
                    {
                        if (++i < count)
                            Container.Children.RemoveRange(i, count - i);
                        ScrollViewer.ScrollToRightEnd();
                        return;
                    }
                }
                Container.Children.Add(new TextBlock());
            }
            var button = new Button();
            View.SetComponent(button, SelectedItem as INotifyPropertyChanged);
            button.Click += SelectItem;
            Container.Children.Add(button);
            ScrollViewer.ScrollToRightEnd();
        }

        private void SelectItem(object sender, RoutedEventArgs e)
        {
            if (sender is Button b)
            {
                SelectedItem = View.GetComponent(b);
            }
        }

        private void HandleScroll(object sender, MouseWheelEventArgs e)
        {
            var movement = e.Delta == 0 ? 0 : e.Delta > 0 ? 20 : -20;
            ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset - movement);
        }
    }
}
