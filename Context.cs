using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows.Threading;

namespace HACS.WPF
{
	public class Context
	{
		public enum DisplayStyle { Default, Toggle, Content }

		public string Label { get; }

		public Binding Binding { get; }

		public DisplayStyle Style { get; }
		
		public bool Dispatch { get; }

		public Context(string label, Binding binding = null, DisplayStyle style = DisplayStyle.Default, bool dispatch = true)
		{
			Label = label;
			Binding = binding;
			Style = style;
			Dispatch = dispatch;
		}
	}
}
