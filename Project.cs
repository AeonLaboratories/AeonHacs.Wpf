using System.ComponentModel;

namespace System.Windows
{
    public static class Project
    {
        private static readonly DependencyObject designerObject = new DependencyObject();

        public static bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(designerObject);
    }
}
