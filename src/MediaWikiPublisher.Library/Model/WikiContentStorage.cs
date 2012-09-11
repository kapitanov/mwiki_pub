using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MediaWikiPublisher.Converter.Model
{
   public static class WikiContentStorage
    {
       public static string Filter { get { return "MediaWiki document|*.mwiki"; } }

       public static Task<WikiContent> OpenAsync(string path)
       {
           return Task.Factory.StartNew(() => Open(path));
       }

       private static WikiContent Open(string path)
       {
           var xml = XDocument.Load(path);
           var root = xml.Element(Xml.Wiki);

           var title = root.Element(Xml.Info).Element(Xml.Name).Value;

           var categories = xml
               .Element(Xml.Categories)
               .Elements(Xml.Category)
               .Select(element =>
                           {
                               var categoryName = element.Element(Xml.Name).Value;
                               return new WikiCategoryContent(
                                   categoryName,
                                   element
                                       .Element(Xml.Pages)
                                       .Elements(Xml.Page)
                                       .Select(pageElement =>
                                                   {
                                                       var revisionElement = pageElement
                                                           .Element(Xml.Revisions)
                                                           .Element(Xml.Revision);
                                                       return new WikiPageContent(
                                                           pageElement.Element(Xml.Title).Value,
                                                           categoryName,
                                                           revisionElement.Attribute(Xml.Id).Value,
                                                           revisionElement.Value);
                                                   })
                                   );
                           });

           return new WikiContent(title, categories);
       }

       public static Task SaveAsync(string path, WikiContent content)
       {
           return Task.Factory.StartNew(() => Save(path, content));
       }
       
       private static void Save(string path, WikiContent content)
       {
           var xml = new XDocument(
               new XElement(
                   Xml.Wiki,
                   new XElement(Xml.Info, new XElement(Xml.Name, content.Title)),
                   new XElement(
                       Xml.Categories,
                       from category in content.Categories
                       select new XElement(
                           Xml.Category,
                           new XElement(Xml.Name, category.Name),
                           new XElement(Xml.Pages,
                                        from page in category.Pages
                                        select new XElement(
                                            Xml.Page,
                                            new XElement(Xml.Title, page.Title),
                                            new XElement(
                                                Xml.Revisions,
                                                new XElement(
                                                    Xml.Revision,
                                                    new XAttribute(Xml.Id, page.Id),
                                                    new XText(page.Text)
                                                    )
                                                )
                                            )
                               )
                           )
                       )
                   )
               );
           xml.Save(path);
       }

       private static class Xml
       {
           private const string _Namespace = "{mediawiki}";

           public const string Wiki = _Namespace + "wiki";
           public const string Info = _Namespace + "info";
           public const string Title = _Namespace + "title";
           public const string Categories = _Namespace + "categories";
           public const string Category = _Namespace + "category";
           public const string Pages = _Namespace + "pages";
           public const string Page = _Namespace + "page";
           public const string Name = _Namespace + "name";
           public const string Revisions = _Namespace + "revisions";
           public const string Revision = _Namespace + "revision";
           public const string Id = "id";
       }
    }
}
