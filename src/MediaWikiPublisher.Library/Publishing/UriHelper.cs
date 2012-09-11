using System;

namespace MediaWikiPublisher.Converter.Publishing
{
    public static class UriHelper
    {
        public static string GetPackagePath(this Uri uri)
        {
            var path = uri.ToString();
            if(path.StartsWith("/"))
            {
                return path.Substring(1);
            }

            return path;
        }
    }
}
