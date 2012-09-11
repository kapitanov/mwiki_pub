using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter.Parsing
{
    public sealed class ParagraphMarkupElementParser : IMarkupElementParser
    {
        public IMarkupElementParserResult Parse(IWikiMarkupParserContext context)
        {
            return new ParagraphMarkupElementParserResult();
        }

        private sealed class ParagraphMarkupElementParserResult : IMarkupElementParserResult
        {
            public MarkupNode Complete(IWikiMarkupParserContext context)
            {
                var text = context.Enumerator.Current.Text;
                return new ParagraphMarkupNode(TextRunsMarkupElementParser.Parse(text));
            }
        }
    }
}

