using System.Collections.Generic;
using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter.Parsing
{
    public interface IMarkupElementParserResult
    {
        MarkupNode Complete(IWikiMarkupParserContext context);
    }
}
