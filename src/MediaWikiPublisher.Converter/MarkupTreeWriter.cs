using System.Collections.Generic;
using System.Text;

namespace MediaWikiPublisher.Converter
{
    internal class MarkupTreeWriter
    {
        private readonly StringBuilder builder = new StringBuilder();
        private readonly Stack<string> tags = new Stack<string>();

        private string currentIndentationString;
        private int currentIndentation;
        private bool attributesWritten = true;

        private int CurrentIndentation
        {
            get { return currentIndentation; }
            set
            {
                currentIndentation = value;

                var sb = new StringBuilder();
                for (int i = 0; i < currentIndentation + 1; i++)
                {
                    sb.Append(". ");
                }
                currentIndentationString = sb.ToString();
            }
        }

        public void BeginTag(string id)
        {
            if (!attributesWritten)
            {
                builder.Append(">\n");
                attributesWritten = true;
            }

            builder.AppendFormat("{0}<{1}", currentIndentationString, id);
            tags.Push(id);
            CurrentIndentation++;
            attributesWritten = false;
        }

        public void WriteAttribute(string name, string value)
        {
            builder.AppendFormat(" {0}=\"{1}\"", name, value);
        }

        public void WriteText(string text)
        {
            if (!attributesWritten)
            {
                builder.Append(">\n");
                attributesWritten = true;
            }
            builder.AppendFormat("{0}{1}\n", currentIndentationString, text);
        }

        public void EndTag()
        {
            CurrentIndentation--;
            if (!attributesWritten)
            {
                builder.Append(">\n");
                attributesWritten = true;
            }
            builder.AppendFormat("{0}</{1}>\n", currentIndentationString, tags.Pop());
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }
}
