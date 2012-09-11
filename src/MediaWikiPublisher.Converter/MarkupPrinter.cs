using System.Text;
using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter
{
    public sealed class MarkupPrinter : IMarkupNodeVisitor
    {
        private readonly StringBuilder writer = new StringBuilder();

        public string Print(MarkupNode node)
        {
            node.Accept(this);
            return writer.ToString();
        }

        void IMarkupNodeVisitor.Visit(DocumentMarkupNode node)
        {
            foreach (var child in node.Children)
            {
                child.Accept(this);
            }
        }

        void IMarkupNodeVisitor.Visit(HeaderMarkupNode node)
        {
            for (var i = 0; i <= node.Level; i++)
            {
                writer.Append('=');
            }

            writer.Append(node.Title);

            for (var i = 0; i <= node.Level; i++)
            {
                writer.Append('=');
            }

            writer.AppendLine();

            foreach (var child in node.Children)
            {
                child.Accept(this);
            }
        }

        void IMarkupNodeVisitor.Visit(ParagraphMarkupNode node)
        {
            foreach (var child in node.Children)
            {
                child.Accept(this);
            }
            writer.AppendLine();
        }

        void IMarkupNodeVisitor.Visit(HyperlinkMarkupNode node)
        {
            writer.AppendFormat("[[{0}|", node.Uri);
            foreach (var child in node.Children)
            {
                child.Accept(this);
            }
            writer.Append("]]");
        }

        void IMarkupNodeVisitor.Visit(ImageMarkupNode node)
        {
            writer.AppendFormat("[[Image:{0}]]", node.ImageUri);
        }

        void IMarkupNodeVisitor.Visit(TextRunMarkupNode node)
        {
            var italic = node.Style.HasFlag(TextStyle.Italic);
            var bold = node.Style.HasFlag(TextStyle.Bold);

            if(italic)
            {
                writer.Append("''");    
            }
            if (bold)
            {
                writer.Append("'''");
            }
            writer.Append(node.Text);
            if (italic)
            {
                writer.Append("''");
            }
            if (bold)
            {
                writer.Append("'''");
            }
        }

        void IMarkupNodeVisitor.Visit(ListMarkupNode node)
        {
            var ch = node.Style == ListStyle.Bulleted ? "*" : "#";
            foreach (var child in node.Children)
            {
                writer.Append(ch);
                child.Accept(this);
                writer.AppendLine();
            }
        }

        void IMarkupNodeVisitor.Visit(IndentMarkupNode node)
        {
            for (var i = 0; i < node.Level; i++)
            {
                writer.Append(':');
            }
            writer.Append(node.Text);
        }
    }
}
