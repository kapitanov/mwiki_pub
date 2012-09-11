using MediaWikiPublisher.Converter.Compilation;

namespace MediaWikiPublisher.Converter.Publishing
{
    public sealed class CssStyleManager : ICssStyleManager
    {
        #region Implementation of ICssStyleManager

        public bool IsSectionable(int level)
        {
            return true;
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
