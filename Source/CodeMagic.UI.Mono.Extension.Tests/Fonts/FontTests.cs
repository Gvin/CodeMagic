using CodeMagic.UI.Mono.Extension.Fonts;
using NUnit.Framework;

namespace CodeMagic.UI.Mono.Extension.Tests.Fonts
{
    [TestFixture]
    [Parallelizable]
    public class FontTests
    {
        [TestCase(0, 1, 1)]
        [TestCase(1, 10, 1)]
        [TestCase(2, 19, 1)]
        [TestCase(3, 1, 10)]
        [TestCase(4, 10, 10)]
        [TestCase(6, 1, 19)]
        public void GetGlyphRectTest(int glyph, int expectedX, int expectedY)
        {
            const int cellWidth = 8;
            const int cellHeight = 8;

            var font = new Font(null, cellWidth, cellHeight, 3, 1, 0);
            var glyphRect = font.GetGlyphRect(glyph);

            Assert.AreEqual(expectedX, glyphRect.X, 0, "X mismatch");
            Assert.AreEqual(expectedY, glyphRect.Y, 0, "Y mismatch");
            Assert.AreEqual(cellWidth, glyphRect.Width, 0, "Width mismatch");
            Assert.AreEqual(cellHeight, glyphRect.Height, 0, "Height mismatch");
        }
    }
}