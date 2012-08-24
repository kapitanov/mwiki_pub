using System.Collections.Generic;
using System.Linq;

namespace MediaWikiPublisher.Model
{
    public class WikiContent
    {
        private readonly string title;
        private readonly List<WikiCategoryContent> categories;
        private readonly Dictionary<string, Dictionary<string, WikiPageCategory>> pages;

        public WikiContent(string title, IEnumerable<WikiCategoryContent> categories)
        {
            this.title = title;
            this.categories = categories.ToList();
        }

        public string Title
        {
            get { return title; }
        }

        public List<WikiCategoryContent> Categories
        {
            get { return categories; }
        }
    }
}
