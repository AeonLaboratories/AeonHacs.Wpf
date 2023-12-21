using AeonHacs.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace AeonHacs.Wpf.Converters
{
	public class ViewModelConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
			sourceType == typeof(string);

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (!(value is string key))
				return base.ConvertFrom(context, culture, value);

			if (ViewModel.GetFromKey(key) is ViewModel vm)
				return vm;
			return AeonHacs.NamedObject.Find<AeonHacs.INamedObject>(key);
		}
	}
}
