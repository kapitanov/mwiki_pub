namespace MediaWikiPublisher.Converter.Model
{
    public class WikiPageContent : WikiContentBase
    {
        private readonly string id;
        private readonly string category;
        private readonly string title;
        private readonly string text;

        public WikiPageContent(string id, string category, string title, string text)
        {
            this.id = id;
            this.category = category;
            this.title = title;
            this.text = text;
        }

        public string Id { get { return id; } }

        public string Category { get { return category; } }

        public string Title { get { return title; } }

        public string Text { get { return text; } }
    }
}
