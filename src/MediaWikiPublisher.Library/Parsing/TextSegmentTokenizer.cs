using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaWikiPublisher.Converter.Parsing;

namespace MediaWikiPublisher.Converter.Parsing
{
   internal static class TextSegmentTokenizer
    {
       public static IEnumerable<TextSegment> SplitIntoSegments(this string markup)
       {
           var segment = new StringBuilder();

           foreach (var c in markup)
           {
               if (c == '\r')
               {

               }
               else if (c == '\n')
               {
                   if (segment.Length > 0)
                   {
                       yield return new TextSegment(segment.ToString());
                       segment.Clear();
                   }
               }
               else
               {
                   segment.Append(c);
               }
           }
           if (segment.Length > 0)
           {
               yield return new TextSegment(segment.ToString());
           }
       }
    }
}
