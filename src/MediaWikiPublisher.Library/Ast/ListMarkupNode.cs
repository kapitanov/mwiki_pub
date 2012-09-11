using System;
using System.Collections.Generic;

namespace MediaWikiPublisher.Converter.Ast
{
    public sealed class ListMarkupNode : CompositeMarkupNode
    {
        private readonly ListStyle style;
        private readonly int level;

        public ListMarkupNode(ListStyle style, int level, IEnumerable<MarkupNode> children)
            : base(children)
        {
            if (level <= 0)
            {
                throw new ArgumentOutOfRangeException("level");
            }

            this.style = style;
            this.level = level;
        }

        public ListMarkupNode(ListStyle style, IEnumerable<MarkupNode> children)
            : this(style, 1, children)
        { }

        public ListStyle Style { get { return style; } }

        public int Level { get { return level; } }

        public override void Visit(IMarkupNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(MarkupNode other)
        {
            var otherNode = (ListMarkupNode)other;
            return style == otherNode.style &&
                   level == otherNode.level &&
                   base.Equals(other);
        }

        protected override int GetHashCodeOverride()
        {
            return base.GetHashCodeOverride() ^ style.GetHashCode() ^ level.GetHashCode();
        }
    }
}
