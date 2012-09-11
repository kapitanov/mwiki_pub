namespace MediaWikiPublisher.Converter.Compilation
{
    public interface ICssStyleManager
    {
        bool IsSectionable(int level);
        string GetSectionClass(int level);
        string InlineBoldClass { get; }
        string InlineItalicClass { get; }
        string GetIndentClass(int level);
    }
}
