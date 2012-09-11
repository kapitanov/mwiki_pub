using MediaWikiPublisher.Converter.Model;

namespace MediaWikiPublisher.Converter.Publishing
{
    public class EpubPublisher
    {
        public void Publish(WikiContent content, string targetPath)
        {
            var processor = new EpubPublisherProcessor(content, new CssStyleManager(), targetPath);
            processor.Enqueue(new CategoryTaskQueueItem(content.GetCategory(string.Empty)));

            processor.ProcessTasks();
            processor.Publish();
        }
    }
}
