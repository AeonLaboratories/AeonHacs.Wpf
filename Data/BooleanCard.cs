using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AeonHacs.Wpf.Data;
public class BooleanCard : PropertyCard
{
    static BooleanCard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(BooleanCard), new FrameworkPropertyMetadata(typeof(BooleanCard)));
    }
}
