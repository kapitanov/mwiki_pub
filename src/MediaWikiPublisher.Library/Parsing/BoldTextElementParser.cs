using System.Text.RegularExpressions;
using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter.Parsing
{
    public sealed class BoldTextElementParser : IInlineElementParser
    {
        public string RegularExpression { get { return @"\'\'\'(?<text>[^\0\'$]{1,})\'\'\'"; } }

        public MarkupNode CreateMarkupNode(Match match)
        {
            return new TextRunMarkupNode(match.Groups["text"].Value, TextStyle.Bold);
        }
    }
}
