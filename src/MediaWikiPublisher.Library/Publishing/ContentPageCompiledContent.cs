using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaWikiPublisher.Converter.Ast;
using MediaWikiPublisher.Converter.Compilation;

namespace MediaWikiPublisher.Converter.Publishing
{
    public sealed class ContentPageCompiledContent : PageCompiledContent
    {
        public ContentPageCompiledContent(IEpubMetadata metadata, IHtmlResourceManager resourceManager, ICssStyleManager styleManager)
            : base(ContainerPathes.ContentPagePath, GeneratePage(metadata, resourceManager, styleManager), "Content")
        { }

        private static string GeneratePage(IEpubMetadata metadata, IHtmlResourceManager resourceManager, ICssStyleManager styleManager)
        {
            var document = new DocumentMarkupNode(
                "Content",
                ListOf(
                    new HeaderMarkupNode(
                        metadata.Title,
                        1,
                        ListOf(
                            new ListMarkupNode(
                                ListStyle.Numbered,
                                from page in metadata.Pages
                                select new HyperlinkMarkupNode(
                                    page.Uri.GetPackagePath(),
                                    ListOf(new TextRunMarkupNode(page.Title))
                                    )
                                )
                            )
                        )
                    )
                );

            var compiler = new HtmlCompiler(resourceManager, styleManager);
            var html = compiler.Compile(document);

            return html.ToString();
        }

        private static IEnumerable<MarkupNode> ListOf(params MarkupNode[] nodes)
        {
            return nodes;
        }
    }
}
