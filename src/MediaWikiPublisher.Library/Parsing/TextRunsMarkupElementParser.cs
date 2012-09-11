using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter.Parsing
{
    public static class TextRunsMarkupElementParser
    {
        private static readonly IInlineElementParser[] AllParsers;
        private static readonly IInlineElementParser[] TextOnlyParsers;

        static TextRunsMarkupElementParser()
        {
            TextOnlyParsers = new IInlineElementParser[]
                {
                    new BoldAndItalicTextElementParser(),
                    new BoldTextElementParser(),
                    new ItalicTextElementParser(),
                    new PlainTextElementParser()
                };
            AllParsers = new IInlineElementParser[] { new HyperlinkElementParser() }
                .Concat(TextOnlyParsers)
                .ToArray();
        }

        public static MarkupNode[] Parse(string text, TextRunsMarkupElementParserMode mode = TextRunsMarkupElementParserMode.ParseAll)
        {
            return new TextRunsMarkupElementParserWorker(mode).Parse(text);
        }

        private struct TextRunsMarkupElementParserWorker
        {
            private readonly TextRunsMarkupElementParserMode mode;

            public TextRunsMarkupElementParserWorker(TextRunsMarkupElementParserMode mode)
                : this()
            {
                this.mode = mode;
            }

            public MarkupNode[] Parse(string text)
            {
                return ParseInternal(text).ToArray();
            }

            private IEnumerable<MarkupNode> ParseInternal(string text)
            {
                var parsedElements = new List<ParsedInline>();

                Parsers.Aggregate(
                    text,
                    (current, parser) => Regex.Replace(
                        current,
                        parser.RegularExpression,
                        match =>
                        {
                            parsedElements.Add(new ParsedInline(match.Index, parser.CreateMarkupNode(match)));
                            return new string('\0', match.Length);
                        }));



                return parsedElements.Where(_ => _.Node != null).OrderBy(_ => _.Index).Select(_ => _.Node);
            }

            private IEnumerable<IInlineElementParser> Parsers
            {
                get
                {
                    switch (mode)
                    {
                        case TextRunsMarkupElementParserMode.ParseAll:
                            return AllParsers;

                        case TextRunsMarkupElementParserMode.ParseTextOnly:
                            return TextOnlyParsers;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }

        private struct ParsedInline
        {
            private readonly int index;
            private readonly MarkupNode node;

            public ParsedInline(int index, MarkupNode node)
                : this()
            {
                this.index = index;
                this.node = node;
            }

            public int Index { get { return index; } }

            public MarkupNode Node { get { return node; } }
        }
    }
}
