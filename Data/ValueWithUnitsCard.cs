using AeonHacs.Components;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AeonHacs.Wpf.Data;

public class MassUnits
{
    public static readonly MassUnits Grams = new MassUnits("g");
    public static readonly MassUnits Milligrams = new MassUnits("mg", 1000);
    public static readonly MassUnits Micrograms = new MassUnits("μg", 1000000);
    public static readonly MassUnits MicromolesCarbon = new MassUnits("μmol", 1000000 / CegsPreferences.GramsCarbonPerMole);
    //public static MassUnits SI => Grams;

    public string Symbol { get; }

    public double Multiplier { get; }

    public double FromGrams(double grams) => grams * Multiplier;

    public double ToGrams(double value) => value / Multiplier;

    public MassUnits(string symbol, double multiplier = 1)
    {
        Symbol = symbol;
        Multiplier = multiplier;
    }
}

public class MassWithUnits : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    double grams;
    public double Grams
    {
        get => grams;
        set => Ensure(ref grams, value);
    }

    public double Milligrams
    {
        get => MassUnits.Milligrams.FromGrams(grams);
        set => Ensure(ref grams, MassUnits.Milligrams.ToGrams(value));
    }

    public double Micrograms
    {
        get => MassUnits.Micrograms.FromGrams(grams);
        set => Ensure(ref grams, MassUnits.Micrograms.ToGrams(value));
    }

    public double MicromolesCarbon
    {
        get => MassUnits.MicromolesCarbon.FromGrams(grams);
        set => Ensure(ref grams, MassUnits.MicromolesCarbon.ToGrams(value));
    }

    public double Mass
    {
        get => Units.FromGrams(grams);
        set => Ensure(ref grams, Units.ToGrams(value));
    }

    MassUnits units = MassUnits.Grams;
    public MassUnits Units
    {
        get => units;
        set
        {
            if (Ensure(ref units, value))
                PropertyChanged?.Invoke(this, new(nameof(Mass)));
        }
    }

    protected bool Ensure<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (!field?.Equals(value) ?? value != null)
        {
            field = value;
            PropertyChanged?.Invoke(this, new(propertyName));
            return true;
        }
        return false;
    }
}

public class ValueWithUnitsCard : PropertyCard
{
    public static readonly DependencyProperty StandardValuesProperty = SelectorCard.StandardValuesProperty.AddOwner(typeof(ValueWithUnitsCard));

    public static readonly DependencyProperty UnitsProperty = DependencyProperty.Register(
        nameof(Units),
        typeof(object),
        typeof(ValueWithUnitsCard),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
    );

    static ValueWithUnitsCard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ValueWithUnitsCard), new FrameworkPropertyMetadata(typeof(ValueWithUnitsCard)));
    }

    public IEnumerable StandardValues
    {
        get => (IEnumerable)GetValue(StandardValuesProperty);
        set => SetValue(StandardValuesProperty, value);
    }

    public object Units
    {
        get => (object)GetValue(UnitsProperty);
        set => SetValue(UnitsProperty, value);
    }
}
