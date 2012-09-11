using System.Collections.Generic;

namespace MediaWikiPublisher.Converter.Ast
{
    public sealed class ParagraphMarkupNode : CompositeMarkupNode
    {
        public ParagraphMarkupNode(IEnumerable<MarkupNode> children)
            : base(children)
        {
        }

        public override void Visit(IMarkupNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
