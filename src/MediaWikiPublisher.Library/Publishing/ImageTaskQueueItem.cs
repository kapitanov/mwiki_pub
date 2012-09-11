using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaWikiPublisher.Converter.Publishing
{
    public sealed class ImageTaskQueueItem : TaskQueueItem
    {
        private readonly string name;

        public ImageTaskQueueItem(string name)
        {
            this.name = name;
        }

        public override void Process(EpubPublisherProcessor processor)
        {
            var filePath = processor.ResourceManager.RequestImageUri(name);
            /* TODO
             * 1. Compute image URI
             * 2. Download image
             * 3. Write image content into package
             * 
             * //public sealed class ImageCompiledContent : FileCompiledContent { }
    // public sealed class CompiledCssContent : FileCompiledContent { }
             **/
        }
    }
}
