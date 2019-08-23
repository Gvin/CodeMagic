namespace CodeMagic.UI.Sad.Common
{
    public static class StringExtension
    {
        public static string ConvertGlyphs(this string text)
        {
            var result = string.Empty;
            foreach (var symbol in text)
            {
                result += (char)Glyphs.GetGlyph(symbol);
            }

            return result;
        }
    }
}