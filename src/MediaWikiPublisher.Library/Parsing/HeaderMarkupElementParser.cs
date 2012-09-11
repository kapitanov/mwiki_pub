using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter.Parsing
{
    public sealed class HeaderMarkupElementParser : IMarkupElementParser
    {
        private readonly int level;
        private readonly string key;

        public HeaderMarkupElementParser(int level)
        {
            this.level = level;
            key = new string('=', level + 1);
        }

        public IMarkupElementParserResult Parse(IWikiMarkupParserContext context)
        {
             var text = context.Enumerator.Current.Text;
             if (text.StartsWith(key) &&
                 text.EndsWith(key))
             {
                 return new HeaderMarkupElementParserResult(this, text);
             }

            return null;
        }

        private sealed class HeaderMarkupElementParserResult : IMarkupElementParserResult
        {
            private readonly HeaderMarkupElementParser parser;
            private readonly string text;
            
            public HeaderMarkupElementParserResult(HeaderMarkupElementParser parser, string text)
            {
                this.text = text;
                this.parser = parser;
            }

            public MarkupNode Complete(IWikiMarkupParserContext context)
            {
                return new HeaderMarkupNode(
                                       text.Substring(parser.key.Length, text.Length - 2 * parser.key.Length).Trim(),
                                       parser.level,
                                       context.ConsumeUntil(parser.level));
            }
        }
    }
}
