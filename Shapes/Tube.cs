using AeonHacs.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AeonHacs.Wpf.Shapes;

public class Tube : TubingBase
{
    public static readonly DependencyProperty OrientationProperty = LayoutProperties.OrientationProperty.AddOwner(typeof(Tube));

    public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(
               nameof(Length), typeof(double), typeof(Tube), new FrameworkPropertyMetadata(2.0,
                   FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure |
                   FrameworkPropertyMetadataOptions.AffectsRender));

    public RelativeDirection Orientation { get => (RelativeDirection)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    public double Length { get => (double)GetValue(LengthProperty); set => SetValue(LengthProperty, value); }

    protected override Geometry DefineGeometry()
    {
        switch (Orientation)
        {
            case RelativeDirection.Left:
            case RelativeDirection.Right:
                return new RectangleGeometry(new Rect(0.5, 0.5, Length - 1, TubingSize));
            case RelativeDirection.Up:
            case RelativeDirection.Down:
                return new RectangleGeometry(new Rect(0.5, 0.5, TubingSize, Length - 1));
            default:
                return Geometry.Empty;
        }
    }
}
