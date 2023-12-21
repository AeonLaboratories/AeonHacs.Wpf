using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AeonHacs.Wpf
{
	public class ConditionalBinding : Binding
	{
		[ConstructorArgument("trueValue")]
		public object TrueValue { get; set; } = DoNothing;

		[ConstructorArgument("falseValue")]
		public object FalseValue { get; set; } = DoNothing;

		public ConditionalBinding() : base() { }

		public ConditionalBinding(string path) : base(path) =>
			Initialize();

		public ConditionalBinding(string path, object trueValue, object falseValue) : base(path)
		{
			TrueValue = trueValue;
			FalseValue = falseValue;
		}

		protected virtual void Initialize() =>
			Converter = new ConditionalConverter(this);

		internal class ConditionalConverter : IValueConverter
		{
			ConditionalBinding binding;

			public ConditionalConverter(ConditionalBinding binding) =>
				this.binding = binding;

			#region IValueConverter

			public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				try
				{
					bool b = System.Convert.ToBoolean(value);
					return b ? binding.TrueValue : binding.FalseValue;
				}
				catch
				{
					return DependencyProperty.UnsetValue;
				}
			}

			public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				if (value == binding.TrueValue)
					return true;
				else if (value == binding.FalseValue)
					return false;
				else
					return DependencyProperty.UnsetValue;
			}

			#endregion IValueConverter
		}
	}

	//public class ConditionalResourceBinding : ConditionalBinding { }

	//public class ConditionalBinding<T> : Binding
	//{
	//	public Func<T, object>[] Conditions { get; set; }

	//	public ConditionalBinding(string path, params Func<T, object>[] conditions) : base(path)
	//	{
	//		Conditions = conditions;
	//		Initialize();
	//	}

	//	protected virtual void Initialize() =>
	//		Converter = new ConditionalConverter(this);

	//	internal class ConditionalConverter : IValueConverter
	//	{
	//		ConditionalBinding<T> binding;

	//		public ConditionalConverter(ConditionalBinding<T> binding) =>
	//			this.binding = binding;

	//		#region IValueConverter

	//		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
	//		{
	//			try
	//			{
	//				if (value is T val)
	//				{
	//					foreach (var condition in binding.Conditions)
	//					{
	//						object result = condition(val);
	//						if (result != DoNothing)
	//							return result;
	//					}
	//				}
	//				return DoNothing;
	//			}
	//			catch
	//			{
	//				return DoNothing;
	//			}
	//		}

	//		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
	//		{
	//			throw new NotImplementedException();
	//		}

	//		#endregion IValueConverter
	//	}
	//}
}
