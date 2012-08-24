using System;
using System.IO;

using Ionic.Zip;

namespace MediaWikiPublisher.Model
{
    public class WikiArchiveImporter : IWikiImporter
    {
        #region Implementation of IWikiImporter

        public string FormatName { get { return "MediaWiki Archive"; } }

        public string FormatExtension { get { return ".zip"; } }

        public WikiContent Import(Stream stream)
        {
            var zip = ZipFile.Read(stream);
            foreach (var file in zip)
            {
                using(var fileStream = new MemoryStream())
                {
                    file.Extract(fileStream);
                    fileStream.Position = 0;

                    return WikiImporter.ImportFrom(Path.GetExtension(file.FileName), fileStream, this);
                }
            }

            throw new ApplicationException("Unable to load data from archive");
        }

        #endregion
    }
}
