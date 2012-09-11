using System.Collections.Generic;

namespace MediaWikiPublisher.Converter.Publishing
{
    public interface IEpubMetadata
    {
        string Title { get; }
        FileCompiledContent StartPage { get; }
        IEnumerable<PageCompiledContent> Pages { get; }
        IEnumerable<FileCompiledContent> Images { get; }
    }
}
