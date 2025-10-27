using System;
using System.Globalization;
using System.Windows.Controls;

namespace AeonHacs.Wpf.Data;
public class DateTimeValidationRule : ValidationRule
{
    public static DateTimeValidationRule Shared = new();

    protected ValidationResult Valid = new ValidationResult(true, null);

    public DateTimeValidationRule() { }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is string str)
        {
            if (str.IsBlank())
                return new ValidationResult(false, "Invalid input type.");

            return DateTime.TryParse(str, null, out DateTime result) ? Valid : new ValidationResult(false, "Input should be of type 'DateTime'");
        }

        return new ValidationResult(false, "Invalid input type.");
    }
}