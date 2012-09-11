using System;
using System.Xml.Linq;
using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter.Compilation
{
    public sealed class HtmlCompilerWorker : IMarkupNodeVisitor
    {
        private readonly IHtmlResourceManager resourceManager;
        private readonly ICssStyleManager styleManager;
        private readonly XDocument document = new XDocument();
        private XContainer container;

        public HtmlCompilerWorker(IHtmlResourceManager resourceManager, ICssStyleManager styleManager)
        {
            this.resourceManager = resourceManager;
            this.styleManager = styleManager;
        }

        public XDocument Compile(MarkupNode node)
        {
            container = document;
            node.Accept(this);
            return document;
        }

        void IMarkupNodeVisitor.Visit(DocumentMarkupNode node)
        {
            container.Add(new XDocumentType("html", null, null, null));
            using (NestedContainer(HtmlTags.Html))
            {
                using (NestedContainer(HtmlTags.Head))
                {
                    container.AddElement(HtmlTags.Title).Add(node.Title);
                    container.AddElement(HtmlTags.Meta)
                        .AddAttribute(HtmlAttributes.HttpEquiv, "Content-Type")
                        .AddAttribute(HtmlAttributes.Content, "text/html; charset=utf-8");
                }

                using (NestedContainer(HtmlTags.Body))
                {
                    ProcessCompositeNode(node);
                }
            }
        }

        void IMarkupNodeVisitor.Visit(HeaderMarkupNode node)
        {
            if (styleManager.IsSectionable(node.Level + 1))
            {
                using (NestedContainer(HtmlTags.Div)
                    .Attribute(HtmlAttributes.Class, styleManager.GetSectionClass(node.Level + 1)))
                {
                    ProcessHeader(node);
                }
            }
            else
            {
                ProcessHeader(node);
            }
        }

        void IMarkupNodeVisitor.Visit(ParagraphMarkupNode node)
        {
            using (NestedContainer(HtmlTags.Paragraph))
            {
                ProcessCompositeNode(node);
            }
        }

        void IMarkupNodeVisitor.Visit(HyperlinkMarkupNode node)
        {
            var uri = resourceManager.RequestDocumentUri(node.Uri);
            using (NestedContainer(HtmlTags.Anchor)
                .Attribute(HtmlAttributes.HRef, uri))
            {
                ProcessCompositeNode(node);
            }
        }

        void IMarkupNodeVisitor.Visit(ImageMarkupNode node)
        {
            var uri = resourceManager.RequestImageUri(node.ImageUri);
            container.AddElement(HtmlTags.Image).AddAttribute(HtmlAttributes.Src, uri);
        }

        void IMarkupNodeVisitor.Visit(TextRunMarkupNode node)
        {
            var bold = node.Style.HasFlag(TextStyle.Bold);
            var italic = node.Style.HasFlag(TextStyle.Italic);
            if (bold || italic)
            {
                var cssClass = string.Empty;
                if (bold)
                {
                    cssClass += styleManager.InlineBoldClass;
                }
                if (italic)
                {
                    if(cssClass.Length > 0)
                    {
                        cssClass += " ";
                    }
                    cssClass += styleManager.InlineItalicClass;
                }

                using (NestedContainer(HtmlTags.Span)
                    .Attribute(HtmlAttributes.Class, cssClass))
                {
                    container.Add(node.Text);
                }
            }
            else
            {
                container.Add(node.Text);
            }
        }

        void IMarkupNodeVisitor.Visit(ListMarkupNode node)
        {
            string listTag = null;
            switch (node.Style)
            {
                case ListStyle.Numbered:
                    listTag = HtmlTags.OrderedList;
                    break;
                case ListStyle.Bulleted:
                    listTag = HtmlTags.UnorderedList;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            using (NestedContainer(listTag))
            {
                foreach (var child in node.Children)
                {
                    using (NestedContainer(HtmlTags.ListItem))
                    {
                        child.Accept(this);
                    }
                }
            }
        }

        void IMarkupNodeVisitor.Visit(IndentMarkupNode node)
        {
            using (NestedContainer(HtmlTags.Div)
                .Attribute(HtmlAttributes.Class, styleManager.GetIndentClass(node.Level)))
            {
                container.Add(node.Text);
            }
        }

        private void ProcessHeader(HeaderMarkupNode node)
        {
            container.AddElement(string.Format(HtmlTags.HeaderFormat, node.Level)).Add(node.Title);
            ProcessCompositeNode(node);
        }

        private void ProcessCompositeNode(CompositeMarkupNode node)
        {
            foreach (var child in node.Children)
            {
                child.Accept(this);
            }
        }

        private NestedContainerToken NestedContainer(string tag, params object[] content)
        {
            return new NestedContainerToken(this, new XElement(tag, content));
        }

        private struct NestedContainerToken : IDisposable
        {
            private readonly HtmlCompilerWorker worker;
            private readonly XContainer container;

            public NestedContainerToken(HtmlCompilerWorker worker, XContainer container)
                : this()
            {
                this.worker = worker;
                this.container = worker.container;
                worker.container.Add(container);
                worker.container = container;
            }

            public NestedContainerToken Attribute(XName name, object value)
            {
                worker.container.Add(new XAttribute(name, value));
                return this;
            }

            public void Dispose()
            {
                worker.container = container;
            }
        }
    }
}
