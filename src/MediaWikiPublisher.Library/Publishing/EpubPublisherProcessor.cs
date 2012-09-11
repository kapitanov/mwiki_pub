using System.Collections.Generic;
using MediaWikiPublisher.Converter.Compilation;
using MediaWikiPublisher.Converter.Model;
using MediaWikiPublisher.Converter.Parsing;

namespace MediaWikiPublisher.Converter.Publishing
{
    public class EpubPublisherProcessor
    {
        private readonly List<TaskQueueItem> tasks = new List<TaskQueueItem>();
        private readonly WikiMarkupParser markupParser = new WikiMarkupParser();
        private readonly EpubPackager packager;

        private readonly EpubResourceManager resourceManager;
        private readonly HtmlCompiler htmlCompiler;

        private readonly WikiContent content;
        private readonly string targetPath;

        public EpubPublisherProcessor(WikiContent content, ICssStyleManager styleManager, string targetPath)
        {
            this.content = content;
            this.targetPath = targetPath;

            resourceManager = new EpubResourceManager(this);
            htmlCompiler = new HtmlCompiler(resourceManager, styleManager);
            packager = new EpubPackager(content.Title, resourceManager, styleManager);
        }

        public WikiContent Content { get { return content; } }

        public EpubResourceManager ResourceManager { get { return resourceManager; } }

        public WikiMarkupParser MarkupParser { get { return markupParser; } }

        public HtmlCompiler HtmlCompiler { get { return htmlCompiler; } }

        public EpubPackager Packager { get { return packager; } }

        public void Enqueue(TaskQueueItem item)
        {
            tasks.Add(item);
        }

        public void ProcessTasks()
        {
            while (tasks.Count > 0)
            {
                var taskIndex = tasks.Count - 1;
                var task = tasks[taskIndex];

                task.Process(this);

                tasks.RemoveAt(taskIndex);
            }
        }

        public void Publish()
        {
            packager.BuildPackage(targetPath);
        }
    }
}
