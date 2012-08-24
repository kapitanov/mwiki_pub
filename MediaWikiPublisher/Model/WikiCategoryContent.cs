using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MediaWikiPublisher.Model
{
    public class WikiCategoryContent : WikiContentBase
   {
       private readonly string name;
       private readonly List<WikiPageCategory> pages;

       public WikiCategoryContent(string name, IEnumerable<WikiPageCategory> pages)
       {
           this.name = name;
           this.pages = pages.ToList();
       }

       public string Name { get { return name; } }

       public IList<WikiPageCategory> Pages { get { return pages; } }

   }
}
