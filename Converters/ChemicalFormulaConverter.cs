using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters;

[ValueConversion(typeof(string), typeof(string))]
public class ChemicalFormulaConverter : IValueConverter
{
    public static ChemicalFormulaConverter Default { get; } = new();

    private static Dictionary<char, char> ToSubscript { get; } = new()
    {
        {'1', '₁'},
        {'2', '₂'},
        {'3', '₃'},
        {'4', '₄'},
        {'5', '₅'},
        {'6', '₆'},
        {'7', '₇'},
        {'8', '₈'},
        {'9', '₉'}
    };

    private static Dictionary<char, char> FromSubscript { get; } = new()
    {
        {'₁', '1'},
        {'₂', '2'},
        {'₃' , '3'},
        {'₄' , '4'},
        {'₅' , '5'},
        {'₆' , '6'},
        {'₇' , '7'},
        {'₈' , '8'},
        {'₉' , '9'}
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string formula)
            return DependencyProperty.UnsetValue;
        
        StringBuilder sb = new StringBuilder(formula);

        for (int i = 0; i < sb.Length; i++)
        {
            if (ToSubscript.TryGetValue(sb[i], out char subscript))
                sb[i] = subscript;
        }

        return sb.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string formula)
            return DependencyProperty.UnsetValue;

        StringBuilder sb = new StringBuilder(formula);

        for (int i = 0; i < sb.Length; i++)
        {
            if (FromSubscript.TryGetValue(sb[i], out char normal))
                sb[i] = normal;
        }

        return sb.ToString();
    }
}
