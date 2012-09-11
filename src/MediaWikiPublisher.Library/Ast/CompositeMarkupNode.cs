using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaWikiPublisher.Converter.Ast
{
    public abstract class CompositeMarkupNode : MarkupNode
    {
        private readonly IList<MarkupNode> children;

        protected CompositeMarkupNode(IEnumerable<MarkupNode> children)
        {
            if (children == null)
            {
                throw new ArgumentNullException("children");
            }

            this.children = children.ToList();
        }

        public ICollection<MarkupNode> Children { get { return children; } }
        
        public override bool Equals(MarkupNode other)
        {
            var otherNode = (CompositeMarkupNode) other;
            return children.SequenceEqual(otherNode.children);
        }

        protected override int GetHashCodeOverride()
        {
            return base.GetHashCodeOverride() ^ children.GetHashCode();
        }
    }
}
