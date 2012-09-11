using System;
using System.IO;
using System.IO.Packaging;

namespace MediaWikiPublisher.Converter.Publishing
{
    public abstract class CompiledContent
    {
        private Uri uri;

        public Uri Uri { get { return uri ?? (uri = PackUriHelper.CreatePartUri(RelativeUri)); } }

        protected abstract Uri RelativeUri { get; }
        public abstract string MimeType { get; }
        
        public void WriteTo(Package package)
        {
            var part = package.CreatePart(Uri, MimeType);
            WriteTo(part.GetStream());
        }

        protected abstract void WriteTo(Stream stream);
    }
}
