using System;

namespace MediaWikiPublisher.Converter.Parsing
{
    public sealed class TextSegment
    {
        private readonly string text;

        public TextSegment(string text)
        {
            if(text == null)
            {
                throw new ArgumentNullException("text");
            }

            this.text = text;
        }

        public string Text { get { return text; } }
    }
}
