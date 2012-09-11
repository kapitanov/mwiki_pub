using System;
using System.Collections.Generic;

namespace MediaWikiPublisher.Converter.Ast
{
    public sealed class HyperlinkMarkupNode : CompositeMarkupNode
    {
        private readonly string uri;

        public HyperlinkMarkupNode(string uri, IEnumerable<MarkupNode> children)
            : base(children)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            this.uri = uri;
        }

        public string Uri { get { return uri; } }

        public override void Visit(IMarkupNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(MarkupNode other)
        {
            var otherNode = (HyperlinkMarkupNode)other;
            return uri == otherNode.uri &&
                   base.Equals(other);
        }

        protected override int GetHashCodeOverride()
        {
            return base.GetHashCodeOverride() ^ uri.GetHashCode();
        }
    }
}
