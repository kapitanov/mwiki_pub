using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaWikiPublisher.Converter.Compilation;

namespace MediaWikiPublisher.Converter
{
    public class HtmlResourceManagerStub : IHtmlResourceManager
    {
        #region Implementation of IHtmlResourceManager

        public Uri RequestDocumentUri(string key)
        {
            return new Uri("pack://documents/" + key);
        }

        public Uri RequestImageUri(string key)
        {
            return new Uri("pack://images/" + key);
        }

        #endregion
    }
}
