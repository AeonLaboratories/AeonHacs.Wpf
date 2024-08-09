using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AeonHacs.Wpf.Views;

public class MC : Section
{
    static MC()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MC), new FrameworkPropertyMetadata(typeof(MC)));
    }
}
