using System.Xml.Linq;
using MediaWikiPublisher.Converter.Ast;

namespace MediaWikiPublisher.Converter.Compilation
{
    public sealed class HtmlCompiler
    {
        private readonly IHtmlResourceManager resourceManager;
        private readonly ICssStyleManager styleManager;

        public HtmlCompiler(IHtmlResourceManager resourceManager, ICssStyleManager styleManager)
        {
            this.resourceManager = resourceManager;
            this.styleManager = styleManager;
        }

        public XDocument Compile(MarkupNode root)
        {
            var worker = new HtmlCompilerWorker(resourceManager, styleManager);
            return worker.Compile(root);
        }
    }
}
