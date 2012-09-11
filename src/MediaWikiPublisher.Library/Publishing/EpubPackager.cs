using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using MediaWikiPublisher.Converter.Compilation;

namespace MediaWikiPublisher.Converter.Publishing
{
    public sealed class EpubPackager : IEpubMetadata
    {
        private readonly string title;
        private readonly IHtmlResourceManager resourceManager;
        private readonly ICssStyleManager styleManager;

        private readonly List<PageCompiledContent> pages = new List<PageCompiledContent>();
        private readonly List<FileCompiledContent> images = new List<FileCompiledContent>();
        private readonly List<CompiledContent> otherItems = new List<CompiledContent>();
        private PageCompiledContent startPage;

        public EpubPackager(string title, IHtmlResourceManager resourceManager, ICssStyleManager styleManager)
        {
            this.title = title;
            this.resourceManager = resourceManager;
            this.styleManager = styleManager;
        }

        public void Add(PageCompiledContent item)
        {
            pages.Add(item);
        }
        
        public void BuildPackage(string targetPath)
        {
            startPage = new ContentPageCompiledContent(this, resourceManager, styleManager);
            pages.Insert(0, startPage);

            otherItems.Add(new MimeTypeCompiledContent());
            otherItems.Add(new ContainerCompiledContent());
            otherItems.Add(new MetadataCompiledContent(this));
            otherItems.Add(new NavigatorCompiledContent(this));

            using (var package = Package.Open(targetPath, FileMode.Create))
            {
                foreach (var item in otherItems
                    .Concat(pages)
                    .Concat(images))
                {
                    item.WriteTo(package);
                }
            }
        }

        #region Implementation of IEpubMetadata

        string IEpubMetadata.Title { get { return title; } }

        FileCompiledContent IEpubMetadata.StartPage { get { return startPage; } }

        IEnumerable<PageCompiledContent> IEpubMetadata.Pages { get { return pages; } }

        IEnumerable<FileCompiledContent> IEpubMetadata.Images { get { return images; } }

        #endregion
    }
}
