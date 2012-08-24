using System.IO;

namespace MediaWikiPublisher.Model
{
    public interface IWikiImporter
    {
        string FormatName { get; }
        string FormatExtension { get; }

        WikiContent Import(Stream stream);
    }
}
