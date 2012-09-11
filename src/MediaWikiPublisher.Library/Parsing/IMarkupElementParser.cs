using System.Collections.Generic;

namespace MediaWikiPublisher.Converter.Parsing
{
    public interface IMarkupElementParser
    {
        IMarkupElementParserResult Parse(IWikiMarkupParserContext context);
    }
}
