namespace MediaWikiPublisher.Converter.Compilation
{
    public static class HtmlTags
    {
        public const string Namespace = "{http://www.w3.org/1999/xhtml}";
        public const string NamespaceFormattable = "{{http://www.w3.org/1999/xhtml}}";
        public const string Html = Namespace + "html";
        public const string Head = Namespace + "head";
        public const string Title = Namespace + "title";
        public const string Meta = Namespace + "meta";
        public const string Body = Namespace + "body";
        public const string Div = Namespace + "div";
        public const string HeaderFormat = NamespaceFormattable + "h{0}";
        public const string Paragraph = Namespace + "p";
        public const string Anchor = Namespace + "a";
        public const string Image = Namespace + "img";
        public const string Span = Namespace + "span";
        public const string OrderedList = Namespace + "ol";
        public const string UnorderedList = Namespace + "ul";
        public const string ListItem = Namespace + "li";
    }
}
