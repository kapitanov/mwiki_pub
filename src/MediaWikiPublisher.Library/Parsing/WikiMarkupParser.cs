using System;
using System.Collections.Generic;
using System.Linq;
using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter.Parsing
{
    public class WikiMarkupParser
    {
        private readonly IMarkupElementParser[] headerParsers =
            new IMarkupElementParser[]
                {
                    new HeaderMarkupElementParser(1),
                    new HeaderMarkupElementParser(2),
                    new HeaderMarkupElementParser(3),
                    new HeaderMarkupElementParser(4),
                    new HeaderMarkupElementParser(5),
                    new HeaderMarkupElementParser(6),
                };
        private readonly IMarkupElementParser paragraphParsers = new ParagraphMarkupElementParser();


        public MarkupNode Parse(string title, string text)
        {
            return new DocumentMarkupNode(
                title,
                new MarkupNode[]
                    {
                        new HeaderMarkupNode(title, 0, new MarkupNode[0]),
                    }
                    .Concat(ParseSegments(text.SplitIntoSegments())));
        }

        private IEnumerable<MarkupNode> ParseSegments(IEnumerable<TextSegment> segments)
        {
            using (var content = new MarkupNodeParserContext(this, segments))
            {
                return content.ParseSegments();
            }
        }

        private sealed class MarkupNodeParserContext : IWikiMarkupParserContext, IDisposable
        {
            private readonly WikiMarkupParser parser;
            private readonly IEnumerator<TextSegment> enumerator;

            public MarkupNodeParserContext(WikiMarkupParser parser, IEnumerable<TextSegment> segments)
            {
                this.parser = parser;
                enumerator = segments.GetEnumerator();
            }

            public IEnumerable<MarkupNode> ParseSegments()
            {
                do
                {
                    foreach (var node in ConsumeUntil(0))
                    {
                        yield return node;
                    }
                } while (enumerator.MoveNext());
            }

            IEnumerator<TextSegment> IWikiMarkupParserContext.Enumerator { get { return enumerator; } }

            IEnumerable<MarkupNode> IWikiMarkupParserContext.ConsumeUntil(int headerLevel)
            {
                return ConsumeUntil(headerLevel);
            }

            private IEnumerable<MarkupNode> ConsumeUntil(int headerLevel)
            {
                while (enumerator.MoveNext())
                {
                    IMarkupElementParserResult parserResult = null;
                    for (var i = parser.headerParsers.Length - 1; i >= 0; i--)
                    {
                        parserResult = parser.headerParsers[i].Parse(this);
                        if (parserResult != null)
                        {
                            if (i <= headerLevel - 1)
                            {
                                yield break;
                            }

                            yield return parserResult.Complete(this);

                            break;
                        }
                    }

                    if (parserResult == null)
                    {
                        var paragraph = ConsumeParagraph();
                        yield return paragraph;
                    }
                }
            }

            private MarkupNode ConsumeParagraph()
            {
                var parserResult = parser.paragraphParsers.Parse(this);
                if (parserResult != null)
                {
                    return parserResult.Complete(this);
                }

                return null;
            }

            public void Dispose()
            {
                enumerator.Dispose();
            }
        }
    }
}
