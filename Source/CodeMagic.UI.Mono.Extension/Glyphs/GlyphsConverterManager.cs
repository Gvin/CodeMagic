namespace CodeMagic.UI.Mono.Extension.Glyphs
{
    public static class GlyphsConverterManager
    {
        private static IGlyphsConverter converter;

        public static void Initialize(IGlyphsConverter glyphsConverter)
        {
            converter = glyphsConverter;
        }

        public static int ConvertGlyph(int glyph)
        {
            return converter.ConvertGlyph(glyph);
        }

        public static int ConvertGlyph(char glyph)
        {
            return converter.ConvertGlyph(glyph);
        }
    }
}