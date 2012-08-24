using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaWikiPublisher.Model;

namespace MediaWikiPublisher.UI
{
    public class DocumentTabViewModel: TabViewModel
    {
        private readonly WikiContent content;

        public DocumentTabViewModel(WikiContent content)
        {
            this.content = content;
        }

        #region Overrides of TabViewModel

        public override string Title
        {
            get { return content.Title; }
        }

        public WikiContent Content
        {
            get { return content; }
        }

        #endregion
    }
}
