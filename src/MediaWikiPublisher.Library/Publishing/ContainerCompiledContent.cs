using System;
using System.IO;
using System.Xml.Linq;

namespace MediaWikiPublisher.Converter.Publishing
{
    public sealed class ContainerCompiledContent : CompiledContent
    {
        private static readonly Uri _Uri = new Uri(ContainerPathes.ContainerPath, UriKind.RelativeOrAbsolute);

        #region Overrides of CompiledContent

        protected override Uri RelativeUri { get { return _Uri; } }

        public override string MimeType { get { return MimeTypes.Xml; } }

        protected override void WriteTo(Stream stream)
        {
            var xml = new XDocument(
                new XElement(XmlTags.Container,
                             new XAttribute(XmlAttributes.Version, "1.0"),
                             new XElement(XmlTags.RootFiles,
                                          new XElement(XmlTags.RootFile,
                                                       new XAttribute(XmlAttributes.FullPath, ContainerPathes.ContentPath),
                                                       new XAttribute(XmlAttributes.MediaType, MimeTypes.Oebps))
                                 )
                    )
                );

            stream.Write(xml.ToString());
        }

        #endregion

        private static class XmlTags
        {
            private const string _Namespace = "{urn:oasis:names:tc:opendocument:xmlns:container}";
            public const string Container = _Namespace + "container";
            public const string RootFiles = _Namespace + "rootfiles";
            public const string RootFile = _Namespace + "rootfile";
        }

        private static class XmlAttributes
        {
            public const string Version = "version";
            public const string FullPath = "full-path";
            public const string MediaType = "media-type";
        }
    }
}
