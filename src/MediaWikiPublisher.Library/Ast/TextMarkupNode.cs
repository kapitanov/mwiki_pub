using System;
using System.Linq;

namespace MediaWikiPublisher.Converter.Ast
{
    public abstract class TextMarkupNode : MarkupNode
    {
        private readonly string text;

        protected TextMarkupNode(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            if (text.Any(_ => _ == '\0'))
            {
                throw new ArgumentException("text");
            }

            this.text = text;
        }

        public string Text { get { return text; } }
        
        public override bool Equals(MarkupNode other)
        {
            var otherNode = (TextMarkupNode)other;
            return text == otherNode.text &&
                   base.Equals(other);
        }

        protected override int GetHashCodeOverride()
        {
            return base.GetHashCodeOverride() ^ text.GetHashCode();
        }
    }
}
