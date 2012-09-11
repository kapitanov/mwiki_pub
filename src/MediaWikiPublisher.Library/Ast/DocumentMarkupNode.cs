using System.Collections.Generic;
using System.Linq;

namespace MediaWikiPublisher.Converter.Ast
{
    public sealed class DocumentMarkupNode : CompositeMarkupNode
    {
        private readonly string title;

        public DocumentMarkupNode(string title, IEnumerable<MarkupNode> children)
            : base(children)
        {
            this.title = title;
        }

        public string Title { get { return title; } }

        public override void Visit(IMarkupNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(MarkupNode other)
        {
            var otherNode = (DocumentMarkupNode)other;
            return title == otherNode.title && base.Equals(other);
        }

        protected override int GetHashCodeOverride()
        {
            return base.GetHashCodeOverride() ^ title.GetHashCode();
        }
    }
}
