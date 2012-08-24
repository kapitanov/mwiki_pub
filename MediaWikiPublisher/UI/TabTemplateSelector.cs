using System.Windows;
using System.Windows.Controls;

namespace MediaWikiPublisher.UI
{
    public class TabTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate StartPageTemplate { get; set; }
        public DataTemplate DocumentTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is DocumentTabViewModel)
            {
                return DocumentTemplate;
            }

            if (item is StartPageTabViewModel)
            {
                return StartPageTemplate;
            }


            return DefaultTemplate;
        }
    }
}
