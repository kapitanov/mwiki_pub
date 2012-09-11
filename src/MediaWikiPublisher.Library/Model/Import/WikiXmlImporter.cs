using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NLog;

namespace MediaWikiPublisher.Converter.Model.Import
{
    public class WikiXmlImporter : IWikiImporter
    {
        private static readonly Logger _Log = LogManager.GetLogger(typeof(WikiXmlImporter).Name);

        #region Implementation of IWikiImporter

        public string FormatName { get { return "MediaWiki XML"; } }

        public string FormatExtension { get { return ".xml"; } }

        public WikiContent Import(string filename, Stream stream)
        {
            _Log.Debug("Loading from {0}", FormatExtension);

            var xml = XDocument.Load(stream)
                .Element(Xml.MediaWiki);
            var title = LoadTitle(xml);
            var categories = LoadCategories(xml);
            var pages = LoadPages(xml, categories);
            return new WikiContent(title, pages);
        }

        #endregion

        #region Implementation

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

        private IEnumerable<WikiCategoryContent> LoadPages(XElement xml, List<string> categories)
        {
            return (from p in LoadPagesInternal(xml, categories)
                    group p by p.Category
                    into cat
                    orderby cat.Key
                    select new WikiCategoryContent(cat.Key, from p in cat orderby p.Title ascending select p));
        }

        private IEnumerable<WikiPageContent> LoadPagesInternal(XElement xml, List<string> categories)
        {
            return xml
                .Elements(Xml.Page)
                .Select(x => LoadPage(x, categories));
        }

        private WikiPageContent LoadPage(XElement xml, List<string> categories)
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

            return new WikiPageContent(id, category, title, text);
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

        #endregion
    }
}
