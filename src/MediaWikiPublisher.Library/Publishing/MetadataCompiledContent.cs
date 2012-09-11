using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MediaWikiPublisher.Converter.Publishing
{
    public sealed class MetadataCompiledContent : CompiledContent
    {
        private static readonly Uri _Uri = new Uri(ContainerPathes.ContentPath, UriKind.RelativeOrAbsolute);

        private readonly IEpubMetadata metadata;

        public MetadataCompiledContent(IEpubMetadata metadata)
        {
            this.metadata = metadata;
        }

        #region Overrides of CompiledContent

        protected override Uri RelativeUri { get { return _Uri; } }

        public override string MimeType { get { return MimeTypes.Xml; } }

        private XElement CreateManifest()
        {
            return new XElement(XmlTags.Manifest,
                                new XElement(XmlTags.Item,
                                             new XAttribute(XmlAttributes.Id, "ncx"),
                                             new XAttribute(XmlAttributes.HRef, ContainerPathes.TocPath),
                                             new XAttribute(XmlAttributes.MediaType, MimeTypes.DtbNcx)),

                                from item in metadata.Pages.Concat(metadata.Images)
                                select new XElement(XmlTags.Item,
                                                    new XAttribute(XmlAttributes.Id, item.Id),
                                                    new XAttribute(XmlAttributes.HRef, item.Uri.GetPackagePath()),
                                                    new XAttribute(XmlAttributes.MediaType, item.MimeType)
                                    )
                );
        }

        private XElement CreateSpine()
        {
            return new XElement(XmlTags.Spine,
                                new XAttribute(XmlAttributes.Toc, "ncx"),
                                new XElement(XmlTags.ItemRef,
                                             new XAttribute(XmlAttributes.IdRef, metadata.StartPage.Id)),
                                from item in metadata.Pages.Concat(metadata.Images)
                                select new XElement(XmlTags.ItemRef,
                                                    new XAttribute(XmlAttributes.IdRef, item.Id)
                                    )
                );
        }

        private XElement CreateMetadata()
        {
            return new XElement(XmlTags.Metadata, 
                new XElement(XmlTags.Title, metadata.Title),
                new XElement(XmlTags.Identifier, 
                    new XAttribute(XmlAttributes.Id, "dcidid"),
                    Guid.NewGuid().ToString())
                );
        }

        private XElement CreateGuide()
        {
            return new XElement(XmlTags.Guide,
                                new XElement(XmlTags.Reference,
                                             new XAttribute(XmlAttributes.Title, "Cover"),
                                             new XAttribute(XmlAttributes.Type, "cover"),
                                             new XAttribute(XmlAttributes.HRef, metadata.StartPage.Uri.GetPackagePath())));
        }

        protected override void WriteTo(Stream stream)
        {
            var xml = new XDocument(
                new XElement(XmlTags.Package,
                             new XAttribute(XmlAttributes.UniqueIdentifier, "dcidid"),
                             new XAttribute(XmlAttributes.Version, "2.0"),
                             CreateMetadata(), 
                             CreateManifest(),
                             CreateSpine(),
                             CreateGuide()));

            stream.Write(xml.ToString());
        }

        private static class XmlTags
        {
            private const string _Namespace = "{http://www.idpf.org/2007/opf}";
            public const string Package = _Namespace + "package";
            public const string Manifest = _Namespace + "manifest";
            public const string Metadata = _Namespace + "metadata";
            public const string Item = _Namespace + "item";
            public const string Spine = _Namespace + "spine";
            public const string ItemRef = _Namespace + "itemref";
            public const string Guide = _Namespace + "guide";
            public const string Reference = _Namespace + "reference";

            private const string _NamespaceDc = "{http://purl.org/dc/elements/1.1/}";
            public const string Title = _NamespaceDc + "title";
            public const string Identifier = _NamespaceDc + "identifier";
        }

        private static class XmlAttributes
        {
            public const string Version = "version";
            public const string UniqueIdentifier = "unique-identifier";
            public const string Id = "id";
            public const string IdRef = "idref";
            public const string HRef = "href";
            public const string MediaType = "media-type";
            public const string Toc = "toc";
            public const string Title = "title";
            public const string Type = "type";
        }

        #endregion
    }
}
