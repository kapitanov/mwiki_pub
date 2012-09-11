using System.IO;
using System.Text;

namespace MediaWikiPublisher.Converter.Publishing
{
    public static class StreamExtentions
    {
        private static readonly Encoding _Encoding = Encoding.ASCII;

        public static void Write(this Stream stream, string text)
        {
            var buffer = _Encoding.GetBytes(text);
            stream.Write(buffer);
        }

        public static void Write(this Stream stream, byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}
