using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Alignment = System.Drawing.ContentAlignment;

namespace HACS.WPF
{
	[MarkupExtensionReturnType(typeof(Thickness))]
	public class Anchor : MarkupExtension
	{
		protected static Size Infinity = new Size(double.PositiveInfinity, double.PositiveInfinity);

		[ConstructorArgument("alignment")]
		public Alignment Alignment { get; set; }

		protected FrameworkElement TargetObject { get; set; }

		protected PropertyChangeNotifier CanvasLeft { get; set; }
		protected PropertyChangeNotifier CanvasTop { get; set; }
		protected PropertyChangeNotifier CanvasRight { get; set; }
		protected PropertyChangeNotifier CanvasBottom { get; set; }

		protected double Height { get; set; }
		protected double Width { get; set; }

		public Anchor() : this(Alignment.TopLeft) { }

		public Anchor(Alignment alignment) => Alignment = alignment;

		~Anchor()
		{
			if (TargetObject != null)
				TargetObject.SizeChanged -= UpdateMargin;
			if (CanvasLeft != null)
				CanvasLeft.ValueChanged -= UpdateMargin;
			if (CanvasTop != null)
				CanvasTop.ValueChanged -= UpdateMargin;
			if (CanvasRight != null)
				CanvasRight.ValueChanged -= UpdateMargin;
			if (CanvasBottom != null)
				CanvasBottom.ValueChanged -= UpdateMargin;
		}

		private void TargetObject_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			Height = e.NewSize.Height;
			Width = e.NewSize.Width;

			UpdateMargin(sender, e);
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (!(serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget pvt))
				return default;

			TargetObject = (FrameworkElement)pvt.TargetObject;
			if (pvt.TargetProperty != FrameworkElement.MarginProperty)
				throw new InvalidOperationException($"{nameof(Anchor)} expects DependencyProperty Margin.");

			TargetObject.SizeChanged += TargetObject_SizeChanged;
			CanvasLeft = new PropertyChangeNotifier(TargetObject, Canvas.LeftProperty);
			CanvasLeft.ValueChanged += UpdateMargin;
			CanvasTop = new PropertyChangeNotifier(TargetObject, Canvas.TopProperty);
			CanvasTop.ValueChanged += UpdateMargin;
			CanvasRight = new PropertyChangeNotifier(TargetObject, Canvas.RightProperty);
			CanvasRight.ValueChanged += UpdateMargin;
			CanvasBottom = new PropertyChangeNotifier(TargetObject, Canvas.BottomProperty);
			CanvasBottom.ValueChanged += UpdateMargin;

			if (TargetObject.ActualHeight == 0)
			{
				TargetObject.Measure(Infinity);

				Height = TargetObject.DesiredSize.Height;
				Width = TargetObject.DesiredSize.Width;
			}
			else
			{
				Height = TargetObject.ActualHeight;
				Width = TargetObject.ActualWidth;
			}

			return GetMargin();
		}

		protected Thickness GetMargin()
		{
			bool workFromTop = !double.IsNaN(Canvas.GetTop(TargetObject)) ||
				double.IsNaN(Canvas.GetBottom(TargetObject));
			bool workFromLeft = !double.IsNaN(Canvas.GetLeft(TargetObject)) ||
				double.IsNaN(Canvas.GetRight(TargetObject));

			double leftMargin, topMargin;

			if (Alignment == 0)
				Alignment = Alignment.TopLeft;

			switch (Alignment)
			{
				default:
				case Alignment.TopLeft:
				case Alignment.MiddleLeft:
				case Alignment.BottomLeft:
					leftMargin = workFromLeft ? 0 : Width;
					break;
				case Alignment.TopCenter:
				case Alignment.MiddleCenter:
				case Alignment.BottomCenter:
					leftMargin = workFromLeft ? -Width / 2 : Width / 2;
					break;
				case Alignment.TopRight:
				case Alignment.MiddleRight:
				case Alignment.BottomRight:
					leftMargin = workFromLeft ? -Width : 0;
					break;
			}

			switch (Alignment)
			{
				default:
				case Alignment.TopLeft:
				case Alignment.TopCenter:
				case Alignment.TopRight:
					topMargin = workFromTop ? 0 : Height;
					break;
				case Alignment.MiddleLeft:
				case Alignment.MiddleCenter:
				case Alignment.MiddleRight:
					topMargin = workFromTop ? -Height / 2 : Height / 2;
					break;
				case Alignment.BottomLeft:
				case Alignment.BottomCenter:
				case Alignment.BottomRight:
					topMargin = workFromTop ? -Height : 0;
					break;
			}

			return new Thickness(leftMargin, topMargin, -leftMargin, -topMargin);
		}

		private void UpdateMargin(object sender, EventArgs e)
		{
			if (!TargetObject.Dispatcher.CheckAccess())
				TargetObject.Dispatcher.Invoke(() => UpdateMargin(sender, e));
			else
				TargetObject.SetCurrentValue(FrameworkElement.MarginProperty, GetMargin());
		}
	}
}