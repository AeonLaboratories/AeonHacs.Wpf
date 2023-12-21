using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace AeonHacs.Wpf.Data
{
	public class NumericValidationRule : ValidationRule
	{
		public static NumericValidationRule Int = new NumericValidationRule() { ValidationType = typeof(int) };
		public static NumericValidationRule Double = new NumericValidationRule() { ValidationType = typeof(double) };

		protected ValidationResult Valid = new ValidationResult(true, null);

		protected Type ValidationType { get; set; }

		protected NumericValidationRule() { }

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (value is string str)
			{
				if (str.IsBlank())
					return new ValidationResult(false, "Invalid input type.");

				switch (ValidationType.Name)
				{
					case nameof(Int32):
						return int.TryParse(str, out int intResult) ? Valid : new ValidationResult(false, "Input should be of type 'int'");
					case nameof(Double):
						return double.TryParse(str, out double doubleResult) ? Valid : new ValidationResult(false, "Input should be of type 'double'");
					default:
						break;
				}
			}

			return new ValidationResult(false, "Invalid input type.");
		}
	}
}
