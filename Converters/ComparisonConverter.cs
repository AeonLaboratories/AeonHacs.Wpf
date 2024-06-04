using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters;

public enum ComparisonType
{
    EqualTo,
    NotEqualTo,
    GreaterThan,
    GreaterThanOrEqualTo,
    LessThan,
    LessThanOrEqualTo
}

// TODO should this be a MultiValueConverter?
public class ComparisonConverter : IValueConverter
{
    public static ComparisonConverter EqualTo = new ComparisonConverter(ComparisonType.EqualTo);
    public static ComparisonConverter NotEqualTo = new ComparisonConverter(ComparisonType.NotEqualTo);
    public static ComparisonConverter GreaterThan = new ComparisonConverter(ComparisonType.GreaterThan);
    public static ComparisonConverter GreaterThanOrEqualTo = new ComparisonConverter(ComparisonType.GreaterThanOrEqualTo);
    public static ComparisonConverter LessThan = new ComparisonConverter(ComparisonType.LessThan);
    public static ComparisonConverter LessThanOrEqualTo = new ComparisonConverter(ComparisonType.LessThanOrEqualTo);

    public ComparisonType ComparisonType { get; set; }

    public ComparisonConverter(ComparisonType comparisonType)
    {
        ComparisonType = comparisonType;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int comparison = Comparer.Default.Compare(value, TypeDescriptor.GetConverter(value).ConvertFrom(parameter));
        switch (ComparisonType)
        {
            case ComparisonType.EqualTo:
                return comparison == 0;
            case ComparisonType.NotEqualTo:
                return comparison != 0;
            case ComparisonType.GreaterThan:
                return comparison > 0;
            case ComparisonType.GreaterThanOrEqualTo:
                return comparison >= 0;
            case ComparisonType.LessThan:
                return comparison < 0;
            case ComparisonType.LessThanOrEqualTo:
                return comparison <= 0;
            default:
                throw new ArgumentException("Unable to compare.");
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
