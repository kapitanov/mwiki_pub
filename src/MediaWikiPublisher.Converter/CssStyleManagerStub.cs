using MediaWikiPublisher.Converter.Compilation;

namespace MediaWikiPublisher.Converter
{
    public class CssStyleManagerStub : ICssStyleManager
    {
        #region Implementation of ICssStyleManager

        public bool IsSectionable(int level)
        {
            return level <= 1;
        }

        public string GetSectionClass(int level)
        {
            return string.Format("section-{0}", level);
        }

        public string InlineBoldClass { get { return "inline-bold"; } }
        
        public string InlineItalicClass { get { return "inline-italic"; } }

        public string GetIndentClass(int level)
        {
            return string.Format("indent-{0}", level);
        }

        #endregion
    }
}
