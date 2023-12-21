using AeonHacs.Wpf.Converters;
using System.Windows.Automation;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AeonHacs.Wpf.ViewModels;

namespace AeonHacs.Wpf.Views
{
    public class HacsCanvas : System.Windows.Controls.Canvas
    {
        public virtual string HelpText
        {
            get => (string)GetValue(AutomationProperties.HelpTextProperty);
            set => SetValue(AutomationProperties.HelpTextProperty, value);
        }

        [TypeConverter(typeof(ViewModelConverter))]
        public virtual INotifyPropertyChanged Component
        {
            get => (INotifyPropertyChanged)GetValue(View.ComponentProperty);
            set =>  SetValue(View.ComponentProperty, value);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                    e.ClickCount == 2 &&
                    Component is ViewModel vm)
                vm.Run();
            else
                base.OnMouseDown(e);
        }

        public HacsCanvas()
		{

		}
    }
}
