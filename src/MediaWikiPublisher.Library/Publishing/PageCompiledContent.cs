using System;
using System.IO;

namespace MediaWikiPublisher.Converter.Publishing
{
    public class PageCompiledContent : FileCompiledContent
    {
        private readonly string content;
        private readonly string title;

        public PageCompiledContent(string path, string content, string title)
            : base(path, MimeTypes.XHtml)
        {
            this.content = content;
            this.title = title;
        }

        public string Title { get { return title; } }

        protected override void WriteTo(Stream stream)
        {
            stream.Write(content);
        }
    }
}
