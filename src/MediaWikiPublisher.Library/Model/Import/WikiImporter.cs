using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace MediaWikiPublisher.Converter.Model.Import
{
    public static class WikiImporter
    {
        private static readonly Logger _Log = LogManager.GetLogger(typeof (WikiImporter).Name);
        private static readonly Dictionary<string, IWikiImporter> _Importers = new Dictionary<string, IWikiImporter>();

        static WikiImporter()
        {
            Register<WikiXmlImporter>();
            Register<WikiZipArchiveImporter>();
            Register<WikiGzArchiveImporter>();
        }

        private static void Register<TWikiImporter>() where TWikiImporter : IWikiImporter, new()
        {
            var importer = new TWikiImporter();
            _Importers[importer.FormatExtension.ToLower()] = importer;
        }

        public static string Filter
        {
            get
            {
                var builder = _Importers.Values
                    .Aggregate(
                        new StringBuilder(),
                        (sb, importer) => sb.AppendFormat("{0}|*{1}|", importer.FormatName, importer.FormatExtension));
                return builder.ToString(0, builder.Length - 1);
            }
        }

        public static Task<WikiContent> ImportAsync(string path)
        {
            return Task.Factory.StartNew(() => Import(path));
        }

        public static WikiContent Import(string path)
        {
            try
            {
                using (var stream = File.OpenRead(path))
                {
                    return ImportFrom(Path.GetFileName(path), stream, null);
                }
            }
            catch (Exception e)
            {
                _Log.ErrorException("Import() failed", e);
                throw new ApplicationException(string.Format("\"{0}\" - {1}", path, e.Message), e);
            }
        }

        public static WikiContent ImportFrom(string filename, Stream stream, IWikiImporter caller)
        {
            _Log.Debug("ImportFrom(\"{0}\")", filename);

            IWikiImporter importer;
            var fileExtension = Path.GetExtension(filename);
            if (!_Importers.TryGetValue(fileExtension.ToLower(), out importer))
            {
                _Log.Error("ImportFrom(\"{0}\"): format {1} is unknown", filename, fileExtension);
                throw new ApplicationException(string.Format("Format {0} is unknown", fileExtension));
            }

            if (caller != null && caller == importer)
            {
                var callerName = caller.GetType().Name;
                _Log.Error("ImportFrom(\"{0}\") called from {1} caused recursion", filename, callerName);
                throw new ApplicationException(string.Format("ImportFrom() called from {0} caused recursion", callerName));
            }
            
            return importer.Import(filename, stream);
        }
    }
}
