using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Threading;

namespace HACS.WPF.Data
{
	/// <summary>
	/// A one-way Source to Target Binding that explicitly updates on an periodic basis.
	/// </summary>
	[MarkupExtensionReturnType(typeof(object))]
	[Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable, Readability = Readability.Unreadable)]
	public class PeriodicBinding : MarkupExtension
	{
		protected Binding internalBinding = new Binding();
		protected DispatcherTimer updateTimer;

		public virtual PropertyPath Path { get => internalBinding.Path; set => internalBinding.Path = value; }

		public virtual IValueConverter Converter { get => internalBinding.Converter; set => internalBinding.Converter = value; }

		public virtual object Source { get => internalBinding.Source; set => internalBinding.Source = value; }

		public virtual RelativeSource RelativeSource { get => internalBinding.RelativeSource; set => internalBinding.RelativeSource = value; }

		public virtual string ElementName { get => internalBinding.ElementName; set => internalBinding.ElementName = value; }

		public virtual TimeSpan Interval { get; set; }

		~PeriodicBinding() =>
			updateTimer?.Stop();

		protected virtual void UpdateBinding(object sender, EventArgs e) =>
			((sender as DispatcherTimer)?.Tag as BindingExpression)?.UpdateTarget();

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			internalBinding.Mode = BindingMode.OneWay;
			internalBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;

			var result = internalBinding.ProvideValue(serviceProvider);

			if (!Project.IsInDesignMode)
				updateTimer = new DispatcherTimer(Interval, DispatcherPriority.DataBind, UpdateBinding, Dispatcher.CurrentDispatcher) { Tag = result };

			return result;
		}
	}
}
