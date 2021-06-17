using System.Windows;
using System.Windows.Data;
using GRSize = HACS.Components.GraphiteReactor.Sizes;

namespace HACS.WPF.Views
{
    /// <summary>
    /// Interaction logic for GraphiteReactor.xaml
    /// </summary>
    public partial class GraphiteReactor : View
    {
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
