using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaWikiPublisher.Model
{
    public static class WikiImporter
    {
        private static readonly Dictionary<string, IWikiImporter> Importers = new Dictionary<string, IWikiImporter>();

        static WikiImporter()
        {
            Register<WikiXmlImporter>();
            Register<WikiArchiveImporter>();
        }

        private static void Register<TWikiImporter>() where TWikiImporter : IWikiImporter, new()
        {
            var importer = new TWikiImporter();
            Importers[importer.FormatExtension.ToLower()] = importer;
        }

        public static string Filter
        {
            get
            {
                var builder = Importers.Values
                    .Aggregate(
                        new StringBuilder(),
                        (sb, importer) => sb.AppendFormat("{0}|*{1}|", importer.FormatName, importer.FormatExtension));
                return builder.ToString(0, builder.Length - 1);
            }
        }

        public static Task<WikiContent> ImportAsync(string path)
        {
            return Task.Factory.StartNew(arg =>
            {
                try
                {
                    var p = (string)arg;
                    using (var stream = File.OpenRead(p))
                    {
                        var ext = Path.GetExtension(p);
                        return ImportInternal(ext, stream, null);
                    }
                }
                catch (Exception e)
                {
                    throw new ApplicationException(string.Format("\"{0}\" - {1}", path, e.Message), e);
                }
            }, path);
        }

        public static WikiContent ImportFrom(string fileExtension, Stream stream, IWikiImporter caller)
        {
            return ImportInternal(fileExtension, stream, caller);
        }

        private static WikiContent ImportInternal(string fileExtension, Stream stream, IWikiImporter caller)
        {
            IWikiImporter importer;
            if (!Importers.TryGetValue(fileExtension.ToLower(), out importer))
            {
                throw new ApplicationException(string.Format("Format {0} is unknown", fileExtension));
            }

            if (caller != null && caller == importer)
            {
                throw new ApplicationException(
                    string.Format("ImportInternal() called from {0} caused recursion", caller.GetType().Name));
            }

            // TODO
            System.Threading.Thread.Sleep(2000);

            return importer.Import(stream);
        }
    }
}
