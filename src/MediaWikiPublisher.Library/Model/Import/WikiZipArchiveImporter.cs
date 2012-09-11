using System;
using System.IO;
using Ionic.Zip;
using NLog;

namespace MediaWikiPublisher.Converter.Model.Import
{
    public class WikiZipArchiveImporter : IWikiImporter
    {
        private static readonly Logger _Log = LogManager.GetLogger(typeof(WikiZipArchiveImporter).Name);

        public string FormatName { get { return "MediaWiki Zip Archive"; } }

        public string FormatExtension { get { return ".zip"; } }

        public WikiContent Import(string filename, Stream stream)
        {
            _Log.Debug("Loading from {0}", FormatExtension);
            var zip = ZipFile.Read(stream);
            foreach (var file in zip)
            {
                using (var fileStream = new MemoryStream())
                {
                    file.Extract(fileStream);
                    fileStream.Position = 0;

                    return WikiImporter.ImportFrom(file.FileName, fileStream, this);
                }
            }

            _Log.Error("File \"{0}\" is empty", filename);
            throw new ApplicationException("Unable to load data from archive");
        }
    }
}
