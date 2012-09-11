using NLog;

namespace MediaWikiPublisher.Converter.Publishing
{
    public abstract class TaskQueueItem
    {
        private readonly Logger log;

        protected TaskQueueItem()
        {
            log = LogManager.GetLogger(GetType().Name);
        }

        protected Logger Log { get { return log; } }

        public abstract void Process(EpubPublisherProcessor processor);
    }
}
