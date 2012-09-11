using System;

namespace MediaWikiPublisher.Converter.Ast
{
    [Flags]
    public enum TextStyle
    {
        Normal = 0x00,
        Bold = 0x01,
        Italic = 0x02
    }
}
