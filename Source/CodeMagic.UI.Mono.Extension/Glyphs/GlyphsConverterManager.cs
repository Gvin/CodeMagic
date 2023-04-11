namespace CodeMagic.UI.Mono.Extension.Glyphs;

public static class GlyphsConverterManager
{
    private static IGlyphsConverter _converter;

    public static void Initialize(IGlyphsConverter glyphsConverter)
    {
        _converter = glyphsConverter;
    }

    public static int ConvertGlyph(int glyph)
    {
        return _converter.ConvertGlyph(glyph);
    }

    public static int ConvertGlyph(char glyph)
    {
        return _converter.ConvertGlyph(glyph);
    }
}
