using System;
using System.Collections.Generic;

namespace MediaWikiPublisher.Converter.Ast
{
    public sealed class HeaderMarkupNode : CompositeMarkupNode
    {
        private readonly string title;
        private readonly int level;

        public HeaderMarkupNode(string title, int level, IEnumerable<MarkupNode> children) 
            : base(children)
        {
            if(string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            if(level < 0 || level > 6)
            {
                throw new ArgumentOutOfRangeException("level");
            }

            this.title = title;
            this.level = level;
        }

        public string Title { get { return title; } }

        public int Level { get { return level; } }

        public override void Visit(IMarkupNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(MarkupNode other)
        {
            var otherNode = (HeaderMarkupNode)other;
            return title == otherNode.title &&
                   level == otherNode.level && 
                   base.Equals(other);
        }

        protected override int GetHashCodeOverride()
        {
            return base.GetHashCodeOverride() ^ title.GetHashCode() ^ level.GetHashCode();
        }
    }
}
