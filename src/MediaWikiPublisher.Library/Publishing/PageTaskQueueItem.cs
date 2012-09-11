using System;
using MediaWikiPublisher.Converter.Model;

namespace MediaWikiPublisher.Converter.Publishing
{
    public sealed class PageTaskQueueItem : TaskQueueItem
    {
        private readonly string category;
        private readonly WikiPageContent page;

        public PageTaskQueueItem(string category, WikiPageContent page)
        {
            this.category = category;
            this.page = page;
        }

        public override void Process(EpubPublisherProcessor processor)
        {
            Uri uri;

            var key = string.Format("{0}:{1}", category, page.Title);
            if(processor.ResourceManager.TryRequestDocumentUri(key, out uri))
            {
                return;
            }

            Log.Debug("Processing wiki page \"{0}\"", page.Title);

            var markupNode = processor.MarkupParser.Parse(page.Title, page.Text);
            var htmlDocument = processor.HtmlCompiler.Compile(markupNode);

            var content = new PageCompiledContent(uri.ToString(), htmlDocument.ToString(), page.Title);
            processor.Packager.Add(content);
        }
    }
}
