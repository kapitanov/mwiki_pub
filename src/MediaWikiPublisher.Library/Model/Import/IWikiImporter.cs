using System.IO;

namespace MediaWikiPublisher.Converter.Model.Import
{
    public interface IWikiImporter
    {
        string FormatName { get; }
        string FormatExtension { get; }

        WikiContent Import(string filename, Stream stream);
    }
}
