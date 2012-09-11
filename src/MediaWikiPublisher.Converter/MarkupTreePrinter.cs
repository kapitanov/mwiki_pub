using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter
{
   public sealed class MarkupTreePrinter : IMarkupNodeVisitor
   {
       private readonly MarkupTreeWriter writer = new MarkupTreeWriter();

       public string Print(MarkupNode node)
       {
           node.Accept(this);
           return writer.ToString();
       }

       void IMarkupNodeVisitor.Visit(DocumentMarkupNode node)
       {
           writer.BeginTag("document");
           foreach (var child in node.Children)
           {
               child.Accept(this);
           }
           writer.EndTag();
       }

       void IMarkupNodeVisitor.Visit(HeaderMarkupNode node)
       {
           writer.BeginTag("header");
           writer.WriteAttribute("title", node.Title);
           writer.WriteAttribute("level", node.Level.ToString());
           foreach (var child in node.Children)
           {
               child.Accept(this);
           }
           writer.EndTag();
       }

       void IMarkupNodeVisitor.Visit(ParagraphMarkupNode node)
       {
           writer.BeginTag("paragraph");
           foreach (var child in node.Children)
           {
               child.Accept(this);
           }
           writer.EndTag();
       }

       void IMarkupNodeVisitor.Visit(HyperlinkMarkupNode node)
       {
           writer.BeginTag("hyperlink");
           writer.WriteAttribute("uri", node.Uri);
           foreach (var child in node.Children)
           {
               child.Accept(this);
           }
           writer.EndTag();
       }

       void IMarkupNodeVisitor.Visit(ImageMarkupNode node)
       {
           writer.BeginTag("image");
           writer.WriteAttribute("uri", node.ImageUri.ToString());
           writer.EndTag();
       }

       void IMarkupNodeVisitor.Visit(TextRunMarkupNode node)
       {
           writer.BeginTag("text");
           writer.WriteAttribute("style", node.Style.ToString());
           writer.WriteText(node.Text);
           writer.EndTag();
       }

       void IMarkupNodeVisitor.Visit(ListMarkupNode node)
       {
           writer.BeginTag("list");
           writer.WriteAttribute("style", node.Style.ToString());
           foreach (var child in node.Children)
           {
               child.Accept(this);
           }
           writer.EndTag();
       }

       void IMarkupNodeVisitor.Visit(IndentMarkupNode node)
       {
           writer.BeginTag("indent");
           writer.WriteAttribute("level", node.Level.ToString());
           writer.WriteText(node.Text);
           writer.EndTag();
       }
    }
}
