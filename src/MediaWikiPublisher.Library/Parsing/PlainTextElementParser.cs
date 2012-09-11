using System.Text.RegularExpressions;
using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter.Parsing
{
    public sealed class PlainTextElementParser : IInlineElementParser
    {
        public string RegularExpression { get { return @"(\0|^)(?<text>[^\0$]{1,})(\0|$)"; } }

        public MarkupNode CreateMarkupNode(Match match)
        {
            var text = match.Groups["text"].Value;
            return new TextRunMarkupNode(text);
        }
    }
}
