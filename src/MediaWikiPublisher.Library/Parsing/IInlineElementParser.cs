using System.Text.RegularExpressions;
using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter.Parsing
{
    public interface IInlineElementParser
    {
        string RegularExpression { get; }

        MarkupNode CreateMarkupNode(Match match);
    }
}
