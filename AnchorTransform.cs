using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Alignment = System.Drawing.ContentAlignment;

namespace AeonHacs.Wpf
{
    [MarkupExtensionReturnType(typeof(TranslateTransform))]
    public class AnchorTransform : MarkupExtension
    {
        protected static Size Infinity = new Size(double.PositiveInfinity, double.PositiveInfinity);

        [ConstructorArgument("anchor")]
        public Alignment Anchor { get; set; }
        protected TranslateTransform Transform { get; set; } = new TranslateTransform();

        protected FrameworkElement TargetObject { get; set; }

        protected VerticalAlignment VerticalAlignment { get; set; }
        protected HorizontalAlignment HorizontalAlignment { get; set; }

        protected double Height { get; set; }
        protected double Width { get; set; }

        public AnchorTransform() : this(Alignment.TopLeft) { }

        public AnchorTransform(Alignment anchor)
        {
            Anchor = anchor;
        }

        ~AnchorTransform()
        {
            if (TargetObject != null)
                TargetObject.LayoutUpdated -= TargetObject_LayoutUpdated;
        }

        private void TargetObject_LayoutUpdated(object sender, EventArgs e)
        {
            bool sizeChanged = false;
            if (Width != TargetObject.ActualWidth)
            {
                Width = TargetObject.ActualWidth;
                sizeChanged = true;
            }
            if (Height != TargetObject.ActualHeight)
            {
                Height = TargetObject.ActualHeight;
                sizeChanged = true;
            }

            if (UpdateAlignment() || sizeChanged)
                UpdateTransform();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!(serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget pvt))
                return default;

            TargetObject = pvt.TargetObject as FrameworkElement;
            if (TargetObject == null)
                throw new InvalidOperationException($"Unable to identify TargetObject.");

            TargetObject.LayoutUpdated += TargetObject_LayoutUpdated;

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

            UpdateAlignment();
            UpdateTransform();
            return Transform;
        }

        protected bool UpdateAlignment()
        {
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment verticalAlignment = VerticalAlignment.Top;

            if (TargetObject.Parent is Canvas)
            {
                horizontalAlignment = (!double.IsNaN(Canvas.GetLeft(TargetObject)) || double.IsNaN(Canvas.GetRight(TargetObject)))
                    ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                verticalAlignment = (!double.IsNaN(Canvas.GetTop(TargetObject)) || double.IsNaN(Canvas.GetBottom(TargetObject)))
                    ? VerticalAlignment.Top : VerticalAlignment.Bottom;
            }
            else
            {
                horizontalAlignment = TargetObject.HorizontalAlignment;
                verticalAlignment = TargetObject.VerticalAlignment;
            }

            bool alignmentChanged = false;
            if (HorizontalAlignment != horizontalAlignment)
            {
                HorizontalAlignment = horizontalAlignment;
                alignmentChanged = true;
            }
            if (VerticalAlignment != verticalAlignment)
            {
                VerticalAlignment = verticalAlignment;
                alignmentChanged = true;
            }
            return alignmentChanged;
        }

        protected void UpdateTransform()
        {
            if (Anchor == 0)
                Anchor = Alignment.TopLeft;

            double X, Y;

            switch (Anchor)
            {
                default:
                case Alignment.TopLeft:
                case Alignment.MiddleLeft:
                case Alignment.BottomLeft:
                    if (HorizontalAlignment == HorizontalAlignment.Left)
                        X = 0;
                    else if (HorizontalAlignment == HorizontalAlignment.Right)
                        X = -Width;
                    else
                        X = -Width / 2;
                    break;
                case Alignment.TopCenter:
                case Alignment.MiddleCenter:
                case Alignment.BottomCenter:
                    if (HorizontalAlignment == HorizontalAlignment.Left)
                        X = -Width / 2;
                    else if (HorizontalAlignment == HorizontalAlignment.Right)
                        X = Width / 2;
                    else
                        X = 0;
                    break;
                case Alignment.TopRight:
                case Alignment.MiddleRight:
                case Alignment.BottomRight:
                    if (HorizontalAlignment == HorizontalAlignment.Left)
                        X = -Width;
                    else if (HorizontalAlignment == HorizontalAlignment.Right)
                        X = 0;
                    else
                        X = Width / 2;
                    break;
            }

            switch (Anchor)
            {
                default:
                case Alignment.TopLeft:
                case Alignment.TopCenter:
                case Alignment.TopRight:
                    if (VerticalAlignment == VerticalAlignment.Top)
                        Y = 0;
                    else if (VerticalAlignment == VerticalAlignment.Bottom)
                        Y = -Height;
                    else
                        Y = -Height / 2;
                    break;
                case Alignment.MiddleLeft:
                case Alignment.MiddleCenter:
                case Alignment.MiddleRight:
                    if (VerticalAlignment == VerticalAlignment.Top)
                        Y = -Height / 2;
                    else if (VerticalAlignment == VerticalAlignment.Bottom)
                        Y = Height / 2;
                    else
                        Y = 0;
                    break;
                case Alignment.BottomLeft:
                case Alignment.BottomCenter:
                case Alignment.BottomRight:
                    if (VerticalAlignment == VerticalAlignment.Top)
                        Y = -Height;
                    else if (VerticalAlignment == VerticalAlignment.Bottom)
                        Y = 0;
                    else
                        Y = Height / 2;
                    break;
            }

            Transform.X = X;
            Transform.Y = Y;
        }
    }
}