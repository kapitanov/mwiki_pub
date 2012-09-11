using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaWikiPublisher.Converter.Compilation
{
    public interface IHtmlResourceManager
    {
        Uri RequestDocumentUri(string key);
        Uri RequestImageUri(string key);
    }
}
