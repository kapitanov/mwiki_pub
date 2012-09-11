using System;

namespace MediaWikiPublisher.Converter.Ast
{
    public sealed class IndentMarkupNode : TextMarkupNode
    {
        private readonly int level;

        public IndentMarkupNode(string text, int level = 1)
            : base(text)
        {
            if (level <= 0)
            {
                throw new ArgumentOutOfRangeException("level");
            }

            this.level = level;
        }

        public int Level { get { return level; } }

        public override void Visit(IMarkupNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(MarkupNode other)
        {
            var otherNode = (IndentMarkupNode)other;
            return level == otherNode.level &&
                   base.Equals(other);
        }

        protected override int GetHashCodeOverride()
        {
            return base.GetHashCodeOverride() ^ level.GetHashCode();
        }
    }
}
