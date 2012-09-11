using System.Xml.Linq;

namespace MediaWikiPublisher.Converter.Compilation
{
    public static class XmlLinqExtensions
    {
        public static XElement AddElement(this XContainer parent, XName name)
        {
            var child = new XElement(name);
            parent.Add(child);
            return child;
        }

        public static XContainer AddAttribute(this XContainer parent, XName name, object value)
        {
            parent.Add(new XAttribute(name, value));
            return parent;
        }

        public static XContainer AddText(this XContainer parent, string text)
        {
            parent.Add(text);
            return parent;
        }
    }
}
