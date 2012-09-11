using System.Text.RegularExpressions;
using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter.Parsing
{
    public sealed class HyperlinkElementParser : IInlineElementParser
    {
        private static readonly Regex HyperinkRegex = new Regex(@"^(?<uri>[^\|$]+)(|\|(?<text>[^$]*))$", RegexOptions.Compiled);
        private static readonly Regex ImageLinkRegex = new Regex(@"^(Image|File)\:(?<uri>[^\n|]+)[^$]*$", RegexOptions.Compiled);

        //private readonly IInlineElementParser[] parsers =
        //    new IInlineElementParser[]
        //        {
        //            new HyperlinkElementParser(),
        //            new BoldAndItalicTextElementParser(),
        //            new BoldTextElementParser(),
        //            new ItalicTextElementParser(),
        //            new PlainTextElementParser()
        //        };

        public string RegularExpression { get { return @"\[\[(?<text>[^\]]+)\]\]"; } }

        public MarkupNode CreateMarkupNode(Match match)
        {
            return CreateMarkupNodeInternal(match.Groups["text"].Value);
        }

        private MarkupNode CreateMarkupNodeInternal(string text)
        {
            var match = ImageLinkRegex.Match(text);
            if(match.Success)
            {
                return new ImageMarkupNode(match.Groups["uri"].Value);
            }

            return CreateHyperlinkElement(text);
        }

        private MarkupNode CreateHyperlinkElement(string text)
        {
            string uri = null;
            string caption = null;

            var match = HyperinkRegex.Match(text);
            if (match.Success)
            {
                uri = match.Groups["uri"].Value;
                caption = match.Groups["text"].Value;
            }

            uri = uri ?? text;
            if(string.IsNullOrWhiteSpace(caption))
            {
                caption = uri;
            }

            return new HyperlinkMarkupNode(
                    uri,
                    TextRunsMarkupElementParser.Parse(caption, TextRunsMarkupElementParserMode.ParseTextOnly));
        }
    }
}
