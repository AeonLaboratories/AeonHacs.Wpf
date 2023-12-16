using System;
using System.Collections.Generic;
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
		protected BindingExpression be;

		public virtual PropertyPath Path { get => internalBinding.Path; set => internalBinding.Path = value; }

		public virtual IValueConverter Converter { get => internalBinding.Converter; set => internalBinding.Converter = value; }

		public virtual object Source { get => internalBinding.Source; set => internalBinding.Source = value; }

		public virtual RelativeSource RelativeSource { get => internalBinding.RelativeSource; set => internalBinding.RelativeSource = value; }

		public virtual string ElementName { get => internalBinding.ElementName; set => internalBinding.ElementName = value; }

		TimeSpan interval = TimeSpan.FromSeconds(1);
		public virtual TimeSpan Interval
		{
			get => interval;
			set => SwitchDispatcherTimer(interval, value);
		}

		private Dictionary<TimeSpan, WeakReference<DispatcherTimer>> timers = new Dictionary<TimeSpan, WeakReference<DispatcherTimer>>();

		protected virtual void SwitchDispatcherTimer(TimeSpan currentInterval, TimeSpan newInterval)
		{
			var timer = GetTimerFromInterval(currentInterval);
			timer.Tick -= UpdateBinding;
			timer = GetTimerFromInterval(newInterval);
			timer.Tick += UpdateBinding;
		}

		protected virtual DispatcherTimer GetTimerFromInterval(TimeSpan interval)
		{
			if (!timers.TryGetValue(interval, out var @ref) || !@ref.TryGetTarget(out var timer))
			{
				timers[interval] = new WeakReference<DispatcherTimer>(timer = new DispatcherTimer(DispatcherPriority.DataBind) { Interval = interval });
			}
			return timer;
		}

		protected virtual void UpdateBinding(object sender, EventArgs e) =>
			be?.UpdateTarget();

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			internalBinding.Mode = BindingMode.OneWay;
			internalBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;

			var result = internalBinding.ProvideValue(serviceProvider);
			be = result as BindingExpression;

			return result;
		}
	}
}
