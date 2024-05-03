using AeonHacs.Wpf.Controls;
using AeonHacs.Wpf.Converters;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Shapes;

public class UnionUT : Shape
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(UnionUT));

    public static readonly DependencyProperty Connection1Property = DependencyProperty.Register(
        nameof(Connection1), typeof(double), typeof(UnionUT), new FrameworkPropertyMetadata(6.0,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
            FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty Connection2Property = DependencyProperty.Register(
        nameof(Connection2), typeof(double), typeof(UnionUT), new FrameworkPropertyMetadata(6.0,
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
            FrameworkPropertyMetadataOptions.AffectsRender));

    static UnionUT()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(UnionUT), new FrameworkPropertyMetadata(typeof(UnionUT)));
    }

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    [TypeConverter(typeof(FittingSizeConverter))]
    public double Connection1 { get => (double)GetValue(Connection1Property); set => SetValue(Connection1Property, value); }

    [TypeConverter(typeof(FittingSizeConverter))]
    public double Connection2 { get => (double)GetValue(Connection2Property); set => SetValue(Connection2Property, value); }

    protected override Geometry DefiningGeometry => DefineGeometry();

    protected virtual Geometry DefineGeometry()
    {
        Point m1, m2, m3, m4, m5;
        Size s1, s2, s3, s4, s5;

        switch (Orientation)
        {
            case RelativeDirection.Left:
                s3.Width = 3;
                s3.Height = Math.Max(Connection1, Connection2);

                m1.X = 0.5;
                m1.Y = (s3.Height - Connection1) / 2 + 0.5;
                s1.Width = Connection1 - 1;
                s1.Height = Connection1;

                m2.X = m1.X + s1.Width;
                m2.Y = m1.Y + 1;
                s2.Width = 2;
                s2.Height = Connection1 - 2;

                m3.X = m2.X + s2.Width;
                m3.Y = 0.5;

                m4.X = m3.X + s3.Width;
                s4.Width = 2;
                s4.Height = Connection2 - 2;

                m5.X = m4.X + s4.Width;
                m5.Y = (s3.Height - Connection2) / 2 + 0.5;
                s5.Width = Connection2 - 1;
                s5.Height = Connection2;

                m4.Y = m5.Y + 1;
                break;
            default:
            case RelativeDirection.Up:
                s3.Width = Math.Max(Connection1, Connection2);
                s3.Height = 3;

                m1.X = (s3.Width - Connection1) / 2 + 0.5;
                m1.Y = 0.5;
                s1.Width = Connection1;
                s1.Height = Connection1 - 1;

                m2.X = m1.X + 1;
                m2.Y = m1.Y + s1.Height;
                s2.Width = Connection1 - 2;
                s2.Height = 2;

                m3.X = 0.5;
                m3.Y = m2.Y + s2.Height;

                m4.Y = m3.Y + s3.Height;
                s4.Width = Connection2 - 2;
                s4.Height = 2;

                m5.X = (s3.Width - Connection2) / 2 + 0.5;
                m5.Y = m4.Y + s4.Height;
                s5.Width = Connection2;
                s5.Height = Connection2 - 1;

                m4.X = m5.X + 1;
                break;
            case RelativeDirection.Down:
                s3.Width = Math.Max(Connection1, Connection2);
                s3.Height = 3;

                m1.X = (s3.Width - Connection2) / 2 + 0.5;
                m1.Y = 0.5;
                s1.Width = Connection2;
                s1.Height = Connection2 - 1;

                m2.X = m1.X + 1;
                m2.Y = m1.Y + s1.Height;
                s2.Width = Connection2 - 2;
                s2.Height = 2;

                m3.X = 0.5;
                m3.Y = m2.Y + s2.Height;

                m4.Y = m3.Y + s3.Height;
                s4.Width = Connection1 - 2;
                s4.Height = 2;

                m5.X = (s3.Width - Connection1) / 2 + 0.5;
                m5.Y = m4.Y + s4.Height;
                s5.Width = Connection1;
                s5.Height = Connection1 - 1;

                m4.X = m5.X + 1;
                break;
            case RelativeDirection.Right:
                s3.Width = 3;
                s3.Height = Math.Max(Connection1, Connection2);

                m1.X = 0.5;
                m1.Y = (s3.Height - Connection2) / 2 + 0.5;
                s1.Width = Connection2 - 1;
                s1.Height = Connection2;

                m2.X = m1.X + s1.Width;
                m2.Y = m1.Y + 1;
                s2.Width = 2;
                s2.Height = Connection2 - 2;

                m3.X = m2.X + s2.Width;
                m3.Y = 0.5;

                m4.X = m3.X + s3.Width;
                s4.Width = 2;
                s4.Height = Connection1 - 2;

                m5.X = m4.X + s4.Width;
                m5.Y = (s3.Height - Connection1) / 2 + 0.5;
                s5.Width = Connection1 - 1;
                s5.Height = Connection1;

                m4.Y = m5.Y + 1;
                break;
        }

        return Geometry.Parse($"M{m1} h{s1.Width} v{s1.Height} h{-s1.Width} z" +
            $"M{m2} h{s2.Width} v{s2.Height} h{-s2.Width} z" +
            $"M{m3} h{s3.Width} v{s3.Height} h{-s3.Width} z" +
            $"M{m4} h{s4.Width} v{s4.Height} h{-s4.Width} z" +
            $"M{m5} h{s5.Width} v{s5.Height} h{-s5.Width} z");
    }
}