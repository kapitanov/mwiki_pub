using MediaWikiPublisher.Converter.Model;

namespace MediaWikiPublisher.Converter.Publishing
{
    public sealed class CategoryTaskQueueItem : TaskQueueItem
    {
        private readonly WikiCategoryContent category;

        public CategoryTaskQueueItem(WikiCategoryContent category)
        {
            this.category =  category;
        }

        public override void Process(EpubPublisherProcessor processor)
        {
            Log.Debug("Processing entire wiki category \"{0}\"", category.Name);
            foreach (var page in category.Pages)
            {
                processor.Enqueue(new PageTaskQueueItem(category.Name, page));
            }
        }
    }
}
