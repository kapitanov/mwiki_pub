namespace MediaWikiPublisher.Converter.Ast
{
    public interface IMarkupNodeVisitor
    {
        void Visit(DocumentMarkupNode node);
        void Visit(HeaderMarkupNode node);
        void Visit(ParagraphMarkupNode node);
        void Visit(HyperlinkMarkupNode node);
        void Visit(ImageMarkupNode node);
        void Visit(TextRunMarkupNode node);
        void Visit(ListMarkupNode node);
        void Visit(IndentMarkupNode node);
    }
}
