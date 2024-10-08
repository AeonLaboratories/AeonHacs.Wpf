﻿using AeonHacs.Components;
using AeonHacs;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace AeonHacs.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SampleManager.xaml
    /// </summary>
    public partial class SampleManager : UserControl
    {
        public SampleManager()
        {
            InitializeComponent();
            RefreshSampleList();
        }

        protected virtual void Close()
        {
            Window.GetWindow(this).Close();
        }


        void RefreshSampleList()
        {
            var value = SampleList.SelectedValue;
            var index = SampleList.SelectedIndex;
            var list = new ObservableCollection<Sample>(NamedObject.FindAll<Sample>());
            if (index < 0) index = 0;
            if (index > list.Count - 1) index = list.Count - 1;
            SampleList.ItemsSource = list;
            if (list.Contains(value))
                SampleList.SelectedValue = value;
            else
                SampleList.SelectedIndex = index;
        }

        void Edit(ISample sample)
        {
            //HacsCommands.EditSample.Execute(sample, Application.Current.MainWindow);

            var editor = new SampleEditor(sample);
            editor.Updated += RefreshSampleList;
            var w = new Window
            {
                Content = editor,
                SizeToContent = SizeToContent.WidthAndHeight
            };
            w.SetBinding(Window.TitleProperty, new Binding("Sample.Name") { Source = editor, FallbackValue = "New Sample" });
            w.CommandBindings.Add(new(FormCommands.Close, (_, _) => w.Close()));

            w.Show();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SampleList.SelectedItem is ISample sample)
                Edit(sample);
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            Edit(null);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SampleList.SelectedItem is ISample sample)
            {
                var ports = NamedObject.FindAll<ILinePort>();
                ports.ForEach(p => { if (p.Sample == sample) p.Aliquot = null; });
                var grs = NamedObject.FindAll<IGraphiteReactor>();
                grs.ForEach(gr => { if (gr.Sample == sample) gr.Aliquot = null; });
                var cegs = NamedObject.FindAll<ICegs>();
                cegs.ForEach(c => { if (c.Sample == sample) c.Sample = null; });
                sample.Name = null; // remove from NamedObjects
            }
            RefreshSampleList();
        }

        private void SampleList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            EditButton_Click(null, null);
    }
}
