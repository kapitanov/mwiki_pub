using System.Collections.Generic;
using System.Linq;

namespace MediaWikiPublisher.Converter.Model
{
    public class WikiCategoryContent : WikiContentBase
    {
        private readonly string name;
        private readonly List<WikiPageContent> pages;
        private readonly Dictionary<string, WikiPageContent> pagesByName;

        public WikiCategoryContent(string name, IEnumerable<WikiPageContent> pages)
        {
            this.name = name;
            this.pages = pages.ToList();
            pagesByName = this.pages.ToDictionary(_ => _.Title);
        }

        public string Name { get { return name; } }

        public IList<WikiPageContent> Pages { get { return pages; } }

        public WikiPageContent GetPage(string title)
        {
            WikiPageContent page;
            if(pagesByName.TryGetValue(title, out page))
            {
                return page;
            }
            return null;
        }
    }
}
