using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MediaWikiPublisher.Converter.Compilation;

namespace MediaWikiPublisher.Converter.Publishing
{
    public sealed class EpubResourceManager : IHtmlResourceManager
    {
        private readonly EpubPublisherProcessor processor;

        private int lastDocumentId;
        private readonly Dictionary<string, Uri> documents = new Dictionary<string, Uri>();
        private int lastImageId;
        private readonly Dictionary<string, Uri> images = new Dictionary<string, Uri>();

        private static readonly Regex _PageNameRegex = new Regex(@"^((?<category>[^\:]*)\:|)(?<page>[^$]+)$", RegexOptions.Compiled);

        public EpubResourceManager(EpubPublisherProcessor processor)
        {
            this.processor = processor;
        }

        public bool TryRequestDocumentUri(string key, out Uri uri)
        {
            return TryRequestUri(
                key,
                ref lastDocumentId,
                documents,
                DocumentUriFactory,
                out uri);
        }

        public bool TryRequestImageUri(string key, out Uri uri)
        {
            return TryRequestUri(
                key,
                ref lastImageId,
                images,
                ImageUriFactory,
                out uri);
        }

        #region Implementation of IHtmlResourceManager

        public Uri RequestDocumentUri(string key)
        {
            return RequestUri(
                key,
                ref lastDocumentId,
                documents,
                DocumentUriFactory);
        }

        

        public Uri RequestImageUri(string key)
        {
            return RequestUri(
                key,
                ref lastImageId,
                images,
                ImageUriFactory);
        }

        #endregion

        private static Uri RequestUri(
           string key,
           ref int lastId,
           IDictionary<string, Uri> uris,
           Func<string, int, string> factory)
        {
            Uri uri;
            if (!uris.TryGetValue(key, out uri))
            {
                var id = lastId;
                lastId++;

                var uriString = factory(key, id);
                uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
                uris[key] = uri;
            }

            return uri;
        }

        private static bool TryRequestUri(
           string key,
            ref int lastId,
           IDictionary<string, Uri> uris,
           Func<string, int, string> factory,
           out Uri uri)
        {
            if (!uris.TryGetValue(key, out uri))
            {
                var id = lastId;
                lastId++;

                var uriString = factory(key, id);
                uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
                uris[key] = uri;
                return false;
            }

            return true;
        }

        private string DocumentUriFactory(string k, int id)
        {
            var match = _PageNameRegex.Match(k);
            var categoryName = string.Empty;
            var pageName = k;

            if (match.Success)
            {
                categoryName = match.Groups["category"].Value;
                pageName = match.Groups["page"].Value;
            }


            var category = processor.Content.GetCategory(categoryName);
            if (category != null)
            {
                var page = category.GetPage(pageName);
                if (page != null)
                {
                    processor.Enqueue(new PageTaskQueueItem(category.Name, page));
                    return string.Format(ContainerPathes.PagePathFormat, id);
                }
            }

            return k;
        }

        private string ImageUriFactory(string k, int id)
        {
            processor.Enqueue(new ImageTaskQueueItem(k));
            return string.Format(ContainerPathes.ImagePathFormat, id, Path.GetExtension(k));
        }
    }
}
