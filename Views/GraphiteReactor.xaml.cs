﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GRSize = AeonHacs.Components.GraphiteReactor.Sizes;

namespace AeonHacs.Wpf.Views
{
    /// <summary>
    /// Interaction logic for GraphiteReactor.xaml
    /// </summary>
    public partial class GraphiteReactor : View
    {
        //#region Orientation
        //public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(
        //    typeof(GraphiteReactor));

        //public Orientation Orientation { get => (Orientation)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }
        //#endregion Orientation

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            nameof(Size), typeof(GRSize), typeof(GraphiteReactor));

        public GRSize Size { get => (GRSize)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }

        public GraphiteReactor()
        {
            InitializeComponent();
        }

        protected override void OnComponentTypeChanged()
        {
            base.OnComponentTypeChanged();

            if (Component is ViewModels.GraphiteReactor gr)
                SetBinding(SizeProperty, new Binding(nameof(gr.Size)) { Source = gr });
            else
                BindingOperations.ClearBinding(this, SizeProperty);
        }
    }
}
