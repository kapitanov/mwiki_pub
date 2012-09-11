using System.Collections.Generic;
using System.Linq;

namespace MediaWikiPublisher.Converter.Model
{
    public class WikiContent
    {
        private readonly string title;
        private readonly List<WikiCategoryContent> categories;
        private readonly Dictionary<string, WikiCategoryContent> categoriesByName;

        public WikiContent(string title, IEnumerable<WikiCategoryContent> categories)
        {
            this.title = title;
            this.categories = categories.ToList();
            categoriesByName = this.categories.ToDictionary(_=>_.Name);
        }

        public string Title { get { return title; } }

        public List<WikiCategoryContent> Categories { get { return categories; } }

        public WikiCategoryContent GetCategory(string name)
        {
            WikiCategoryContent category;
            if(categoriesByName.TryGetValue(name, out category))
            {
                return category;
            }

            return null;
        }
    }
}
