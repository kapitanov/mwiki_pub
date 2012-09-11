using System.Text.RegularExpressions;
using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter.Parsing
{
    public sealed class ItalicTextElementParser : IInlineElementParser
    {
        public string RegularExpression { get { return @"\'\'(?<text>[^\0\'$]{1,})\'\'"; } }

        public MarkupNode CreateMarkupNode(Match match)
        {
            var text = match.Groups["text"].Value;
            return new TextRunMarkupNode(text, TextStyle.Italic);
        }
    }
}
