using System.Windows;
using System.Windows.Controls;

using MediaWikiPublisher.Model;

namespace MediaWikiPublisher.UI
{
    public class LayoutItemStyleSelector : StyleSelector
    {
        public Style DefaultStyle { get; set; }
        public Style DocumentStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is WikiContent)
            {
                return DocumentStyle;
            }

            return DefaultStyle;
        }
    }
}
