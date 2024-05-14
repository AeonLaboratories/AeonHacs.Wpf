using System.ComponentModel;

namespace AeonHacs.Wpf.ViewModels;
public class SableCA10 : ViewModel
{
    private Components.SableCA10 analyzer => Component as Components.SableCA10;

    public double CO2Ppm => analyzer?.CO2Ppm.Value ?? double.NaN;

    protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(sender, e);
        if (e.PropertyName == nameof(Components.SableCA10.CO2Percent))
            NotifyPropertyChanged(nameof(CO2Ppm));
    }
}
