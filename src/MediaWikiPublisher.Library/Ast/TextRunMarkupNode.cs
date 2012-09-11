using System.Linq;

namespace MediaWikiPublisher.Converter.Ast
{
    public sealed class TextRunMarkupNode : TextMarkupNode
    {
        private readonly TextStyle style;

        public TextRunMarkupNode(string text, TextStyle style = TextStyle.Normal) 
            : base(text)
        {
            this.style = style;
        }

        public TextStyle Style { get { return style; } }

        public override void Visit(IMarkupNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(MarkupNode other)
        {
            var otherNode = (TextRunMarkupNode)other;
            return style == otherNode.style &&
                   base.Equals(other);
        }

        protected override int GetHashCodeOverride()
        {
            return base.GetHashCodeOverride() ^ style.GetHashCode();
        }
    }
}
