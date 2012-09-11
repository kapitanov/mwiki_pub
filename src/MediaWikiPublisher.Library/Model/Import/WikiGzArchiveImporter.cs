using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using NLog;

namespace MediaWikiPublisher.Converter.Model.Import
{
    public class WikiGzArchiveImporter : IWikiImporter
    {
        private static readonly Logger _Log = LogManager.GetLogger(typeof(WikiGzArchiveImporter).Name);

        public string FormatName { get { return "MediaWiki GZ Archive"; } }

        public string FormatExtension { get { return ".gz"; } }

        public WikiContent Import(string filename, Stream stream)
        {
            _Log.Debug("Loading from {0}", FormatExtension);
            filename = Path.GetFileNameWithoutExtension(filename);
            using (var gzip = new GZipInputStream(stream))
            {
                return WikiImporter.ImportFrom(filename, gzip, this);
            }
        }
    }
}
