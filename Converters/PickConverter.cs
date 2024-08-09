using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters;

public class PickConverter : IMultiValueConverter
{
    public enum PickTypes
    {
        Max,
        Min
    }

    public static readonly PickConverter Max = new PickConverter(PickTypes.Max);
    public static readonly PickConverter Min = new PickConverter(PickTypes.Min);

    public PickTypes PickType { get; set; }

    public PickConverter(PickTypes pickType)
    {
        PickType = pickType;
    }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        switch (PickType)
        {
            case PickTypes.Max:
                return values.Max();
            case PickTypes.Min:
                return values.Min();
            default:
                throw new InvalidOperationException();
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
