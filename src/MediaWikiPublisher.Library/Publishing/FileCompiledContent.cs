using System;
using System.IO;

namespace MediaWikiPublisher.Converter.Publishing
{
    public abstract class FileCompiledContent : CompiledContent
    {
        private readonly string id;
        private readonly Uri uri;
        private readonly string mimeType;

        protected FileCompiledContent(string uri, string mimeType)
        {
            id = Path.GetFileNameWithoutExtension(uri);
            this.uri = new Uri(uri, UriKind.RelativeOrAbsolute);
            this.mimeType = mimeType;
        }

        protected override Uri RelativeUri { get { return uri; } }

        public override string MimeType { get { return mimeType; } }

        public string Id { get { return id; } }
    }
}
