using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MediaWikiPublisher.Converter.Model
{
    public class Document :INotifyPropertyChanged
    {
        private readonly WikiContent content;

        public Document(WikiContent content)
        {
            this.content = content;
        }

        public string Title { get { return content.Title; } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
