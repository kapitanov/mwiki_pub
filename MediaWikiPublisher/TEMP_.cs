/*

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using EPubSharp;

using WikiPlex;
using WikiPlex.Common;
using WikiPlex.Compilation;
using WikiPlex.Compilation.Macros;
using WikiPlex.Formatting.Renderers;

namespace MediaWiki
{
    public class Page
    {
        private readonly string id;
        private readonly string category;
        private readonly string title;
        private readonly string text;

        public Page(string id, string category, string title, string text)
        {
            this.id = id;
            this.category = category;
            this.title = title;
            this.text = text;
        }

        public string Id { get { return id; } }

        public string Category { get { return category; } }

        public string Title { get { return title; } }

        public string Text { get { return text; } }
    }

    public class WikiContent
    {
        private readonly string title;
        private readonly List<string> categories;
        private readonly Dictionary<string, Dictionary<string, Page>> pages;

        public WikiContent(string title, List<string> categories, Dictionary<string, Dictionary<string, Page>> pages)
        {
            this.title = title;
            this.categories = categories;
            this.pages = pages;
        }

        public string Title
        {
            get { return title; }
        }

        public List<string> Categories
        {
            get { return categories; }
        }

        public Dictionary<string, Dictionary<string, Page>> Pages
        {
            get { return pages; }
        }
    }

    public class MediaWikiParser
    {
        public WikiContent Parse(string path)
        {
            var xml = XDocument.Load(path).Element(Xml.MediaWiki);
            var title = LoadTitle(xml);
            var categories = LoadCategories(xml);
            var pages = LoadPages(xml, categories);
            return new WikiContent(title, categories, pages);
        }

        private string LoadTitle(XElement xml)
        {
            return xml
                .Element(Xml.SiteInfo)
                .Element(Xml.SiteName)
                .Value;
        }

        private List<string> LoadCategories(XElement xml)
        {
            return xml
                .Element(Xml.SiteInfo)
                .Element(Xml.Namespaces)
                .Elements(Xml.Namespace)
                .Select(_ => _.Value)
                .ToList();
        }

        private Dictionary<string, Dictionary<string, Page>> LoadPages(XElement xml, List<string> categories)
        {
            return (from p in LoadPagesInternal(xml, categories)
                    group p by p.Category
                        into cat
                        orderby cat.Key
                        select cat)
                .ToDictionary(
                    _ => _.Key,
                    _ => _.ToDictionary(__ => __.Title));
        }

        private IEnumerable<Page> LoadPagesInternal(XElement xml, List<string> categories)
        {
            return xml
                .Elements(Xml.Page)
                .Select(x => LoadPage(x, categories));
        }

        private Page LoadPage(XElement xml, List<string> categories)
        {
            var titleRaw = xml.Element(Xml.Title).Value;

            string category, title;
            var m = Regex.Match(titleRaw, @"^(?<category>[^\:]{1,})\:(?<title>[^$]{1,})$");
            if (!m.Success)
            {
                category = string.Empty;
                title = titleRaw;
            }
            else
            {
                category = m.Groups["category"].Value;
                if (categories.Contains(category))
                {
                    title = m.Groups["title"].Value;
                }
                else
                {
                    category = string.Empty;
                    title = titleRaw;
                }
            }

            var revision = xml.Element(Xml.Revision);
            var id = revision.Element(Xml.Id).Value;
            var text = revision.Element(Xml.Text).Value;

            return new Page(id, category, title, text);
        }

        private static class Xml
        {
            private const string _Namespace = "{http://www.mediawiki.org/xml/export-0.4/}";
            public const string MediaWiki = _Namespace + "mediawiki";
            public const string SiteInfo = _Namespace + "siteinfo";
            public const string SiteName = _Namespace + "sitename";
            public const string Namespaces = _Namespace + "namespaces";
            public const string Namespace = _Namespace + "namespace";
            public const string Page = _Namespace + "page";
            public const string Title = _Namespace + "title";
            public const string Revision = _Namespace + "revision";
            public const string Id = _Namespace + "id";
            public const string Text = _Namespace + "text";
        }
    }

    static class DocumentPackager
    {
        public static void CreateDocument(string Filename, Metadata BookMetadata, List<ContentComponent> ContentFiles)
        {
            new Document().CreateDocument(Filename, BookMetadata, ContentFiles);
        }
    }

    class InternalLinkMacro : IMacro
    {
        /// <summary>
        /// Gets the id of the macro.
        /// 
        /// </summary>
        public string Id
        {
            get
            {
                return "InternalLink";
            }
        }

        /// <summary>
        /// Gets the list of rules for the macro.
        /// 
        /// </summary>
        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                {
                    new MacroRule(@"(?i)(\[\[)((?>[^]|]+))(]])", new Dictionary<int, string>
                    {
                        {
                            1,
                            "Text to Remove"
                        },
                        {
                            2,
                            "Internal Hyperlink With Text Tag"
                        },
                        {
                            3,
                            "Text to Remove"
                        }
                    }),
                };
            }
        }
    }

    internal class InternalLinkRenderer : Renderer
    {
        private const string ExternalLinkFormat = "<a href=\"{0}\" class=\"internalLink\">{1}<span class=\"internalLinkIcon\"></span></a>";
        private const string LinkFormat = "<a href=\"{0}\">{1}</a>";

        /// <summary>
        /// Gets the collection of scope names for this <see cref="T:WikiPlex.Formatting.Renderers.IRenderer"/>.
        /// 
        /// </summary>
        protected override ICollection<string> ScopeNames
        {
            get
            {
                return (ICollection<string>)new string[]
        {
          "Internal Hyperlink With Text Tag"
        };
            }
        }

        /// <summary>
        /// Gets the invalid macro error text.
        /// 
        /// </summary>
        protected override string InvalidMacroError
        {
            get
            {
                return "Cannot resolve link macro, invalid number of parameters.";
            }
        }

        /// <summary>
        /// Will expand the input into the appropriate content based on scope.
        /// 
        /// </summary>
        /// <param name="scopeName">The scope name.</param><param name="input">The input to be expanded.</param><param name="htmlEncode">Function that will html encode the output.</param><param name="attributeEncode">Function that will html attribute encode the output.</param>
        /// <returns>
        /// The expanded content.
        /// </returns>
        protected override string PerformExpand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            input = input.Trim();
            return ExpandLinkWithText(input, attributeEncode, htmlEncode);
            if (scopeName == "Hyperlink With No Text")
                return ExpandLinkNoText(input, attributeEncode, htmlEncode);
            if (scopeName == "Hyperlink With Text Tag")
                return ExpandLinkWithText(input, attributeEncode, htmlEncode);
            if (scopeName == "Hyperlink As Mailto, No Text")
                return string.Format("<a href=\"{0}\" class=\"externalLink\">{1}<span class=\"externalLinkIcon\"></span></a>", (object)attributeEncode("mailto:" + input), (object)htmlEncode(input));
            if (scopeName == "Anchor Tag")
                return string.Format("<a name=\"{0}\"></a>", (object)attributeEncode(input));
            if (scopeName == "Link To Anchor")
                return string.Format("<a href=\"{0}\">{1}</a>", (object)attributeEncode("#" + input), (object)htmlEncode(input));
            else
                return (string)null;
        }

        private static string ExpandLinkWithText(string input, Func<string, string> attributeEncode, Func<string, string> htmlEncode)
        {
            TextPart textPart = Utility.ExtractTextParts(input);
            string str = textPart.Text;
            if (!str.StartsWith("http", StringComparison.OrdinalIgnoreCase) && !str.StartsWith("mailto", StringComparison.OrdinalIgnoreCase))
                str = "http://" + str;
            return string.Format("<a href=\"{0}\" class=\"externalLink\">{1}<span class=\"externalLinkIcon\"></span></a>", (object)attributeEncode(str), (object)htmlEncode(textPart.FriendlyText));
        }

        private static string ExpandLinkNoText(string input, Func<string, string> attributeEncode, Func<string, string> htmlEncode)
        {
            string str = input;
            if (!str.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                str = "http://" + str;
            return string.Format("<a href=\"{0}\" class=\"externalLink\">{1}<span class=\"externalLinkIcon\"></span></a>", (object)attributeEncode(str), (object)htmlEncode(input));
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var parser = new MediaWikiParser();
            var xmlFilename = @"C:\Temp\_delta\pages_current.xml";
            var wiki = parser.Parse(xmlFilename);

            var pages = wiki.Pages[""].Values;
            var wikiEngine = new WikiEngine();
            Macros.Register<InternalLinkMacro>();
            Renderers.Register<InternalLinkRenderer>();

            var contentComponents = new List<ContentComponent>();

            const string header = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.1//EN"" ""http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
  <head>
    <link href=""css/main.css"" type=""text/css"" rel=""Stylesheet"" />
    <title>About</title>
  </head>
  <body class=""epub"">
";

            const string footer = @"  </body>
</html>";

            foreach (var page in pages)
            {
                var html = wikiEngine.Render(page.Text);

                var content = new ContentComponent
                {
                    MediaMimeType = "text/html",
                    Title = page.Title,
                    ItemId = string.Format("page_{0}", page.Id),
                    Content = html,
                    UriString = string.Format("/content/page_{0}.html", page.Id)
                };

                contentComponents.Add(content);
            }

            var epubFilename = Path.Combine(Path.GetDirectoryName(xmlFilename), Path.GetFileNameWithoutExtension(xmlFilename) + ".epub");
            var metadata = new Metadata { Title = wiki.Title };
            DocumentPackager.CreateDocument(epubFilename, metadata, contentComponents);
        }
    }
}
*/