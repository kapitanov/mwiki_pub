using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MediaWikiPublisher.Converter.Publishing
{
    public sealed class NavigatorCompiledContent : CompiledContent
    {
        private static readonly Uri _Uri = new Uri(ContainerPathes.TocPath, UriKind.RelativeOrAbsolute);

        private readonly IEpubMetadata metadata;

        public NavigatorCompiledContent(IEpubMetadata metadata)
        {
            this.metadata = metadata;
        }

        #region Overrides of CompiledContent

        protected override Uri RelativeUri { get { return _Uri; } }

        public override string MimeType { get { return MimeTypes.Xml; } }

        private XElement CreateMetaElement(string name, string content)
        {
            return new XElement(
                XmlTags.Meta,
                new XAttribute(XmlAttributes.Name, name),
                new XAttribute(XmlAttributes.Content, content));
        }

        private XElement CreateHeadElement()
        {
            return new XElement(
                XmlTags.Head,
                CreateMetaElement("dtb:uid", "http://www.hxa.name/articles/content/epup-guide_hxa7241_2007_1.epub"),
                CreateMetaElement("dtb:depth", "1"),
                CreateMetaElement("dtb:totalPageCount", "0"),
                CreateMetaElement("dtb:maxPageNumber", "0"));
        }

        private XElement CreateDocTitleElement()
        {
            return new XElement(
                XmlTags.DocTitle,
                new XElement(
                    XmlTags.Text, metadata.Title));
        }

        private XElement CreateNavMapElement()
        {
            return new XElement(
                XmlTags.NavMap,
                metadata.Pages.Select(
                    (page, index) =>
                    new XElement(
                        XmlTags.NavPoint,
                        new XAttribute(XmlAttributes.Id, string.Format("navPoint-{0}", index + 1)),
                        new XAttribute(XmlAttributes.PlayOrder, index + 1),
                        new XElement(XmlTags.NavLabel,
                                     new XElement(XmlTags.Text, page.Title)),
                        new XElement(XmlTags.Content,
                                     new XAttribute(XmlAttributes.Src, page.Uri.GetPackagePath()))
                        )
                    )
                );
        }

        protected override void WriteTo(Stream stream)
        {
            var xml = new XDocument(
                new XElement(XmlTags.Ncx,
                             new XAttribute(XmlAttributes.Version, "2005-1"),
                             CreateHeadElement(),
                             CreateDocTitleElement(),
                             CreateNavMapElement()
                    )
                );

            stream.Write(xml.ToString());
        }

        private static class XmlTags
        {
            private const string _Namespace = "{http://www.daisy.org/z3986/2005/ncx/}";
            public const string Ncx = _Namespace + "ncx";
            public const string Head = _Namespace + "head";
            public const string Meta = _Namespace + "meta";
            public const string DocTitle = _Namespace + "docTitle";
            public const string Text = _Namespace + "text";
            public const string NavMap = _Namespace + "navMap";
            public const string NavPoint = _Namespace + "navPoint";
            public const string NavLabel = _Namespace + "navLabel";
            public const string Content = _Namespace + "content";
        }

        private static class XmlAttributes
        {
            public const string Version = "version";
            public const string Name = "name";
            public const string Content = "content";
            public const string Id = "id";
            public const string PlayOrder = "playOrder";
            public const string Src = "src";
        }

        #endregion
    }
}
