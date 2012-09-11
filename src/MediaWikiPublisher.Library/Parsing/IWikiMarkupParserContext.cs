using System.Collections.Generic;
using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter.Parsing
{
    public interface IWikiMarkupParserContext
    {
        IEnumerator<TextSegment> Enumerator { get; }

        IEnumerable<MarkupNode> ConsumeUntil(int headerLevel);
    }
}
