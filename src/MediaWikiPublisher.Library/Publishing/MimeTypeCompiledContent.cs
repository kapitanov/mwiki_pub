using System;
using System.IO;

namespace MediaWikiPublisher.Converter.Publishing
{
    public sealed class MimeTypeCompiledContent : CompiledContent
    {
        private static readonly Uri _Uri = new Uri(ContainerPathes.MimetypePath, UriKind.RelativeOrAbsolute);

        #region Overrides of CompiledContent

        protected override Uri RelativeUri { get { return _Uri; } }

        public override string MimeType { get { return MimeTypes.PlainText; } }

        protected override void WriteTo(Stream stream)
        {
            stream.Write(MimeTypes.EpubZip);
        }

        #endregion
    }
}
