using System.Collections.Generic;

namespace CodeMagic.UI.Sad.Common
{
    public static class Glyphs
    {
        private static readonly Dictionary<char, int> GlyphsMapping;

        static Glyphs()
        {
            GlyphsMapping = new Dictionary<char, int>
            {
                {'│', GlyphBoxSingleVertical},
                {'─', GlyphBoxSingleHorizontal},
                {'┌', GlyphBoxSingleDownRight},
                {'┐', GlyphBoxSingleDownLeft},
                {'└', GlyphBoxSingleUpRight},
                {'┘', GlyphBoxSingleUpLeft},

                {'═', GlyphBoxDoubleHorizontal},
                {'║', GlyphBoxDoubleVertical},

                {'█', GlyphBlockFull},
                {'▓', GlyphBlockFilledThick},
                {'░', GlyphBlockFilledRare},

                {'☺', GlyphSmileTransparent},
                {'☻', GlyphSmileFilled},
                {'←', GlyphArrowLeft},
                {'↑', GlyphArrowUp},
                {'→', GlyphArrowRight},
                {'↓', GlyphArrowDown},
                {'▲', GlyphTriangleArrowUp},
                {'►', GlyphTriangleArrowRight},
                {'▼', GlyphTriangleArrowDown},
                {'◄', GlyphTriangleArrowLeft},
                {'☼', GlyphSun},
                {'•', GlyphBullet},

                {'ĭ', GlyphTorchVertical}
            };
        }

        public static int GetGlyph(char symbol)
        {
            if (GlyphsMapping.ContainsKey(symbol))
                return GlyphsMapping[symbol];

            return symbol;
        }

        #region Symbols

        /// <summary>
        /// ☺
        /// </summary>
        public const int GlyphSmileTransparent = 1;

        /// <summary>
        /// ☻
        /// </summary>
        public const int GlyphSmileFilled = 2;

        public const int GlyphHeart = 3;

        public const int GlyphDiamonds = 4;

        public const int GlyphClubs = 5;

        public const int GlyphSpades = 6;

        public const int GlyphBullet = 7;

        public const int GlyphInverseBullet = 8;

        public const int GlyphCircle = 9;

        public const int GlyphInverseCircle = 10;

        public const int GlyphMaleSign = 11;

        public const int GlyphFemaleSign = 12;

        public const int GlyphNoteSingle = 13;

        public const int GlyphNoteDouble = 14;

        /// <summary>
        /// ☼
        /// </summary>
        public const int GlyphSun = 15;

        /// <summary>
        /// ►
        /// </summary>
        public const int GlyphTriangleArrowRight = 16;

        /// <summary>
        /// ◄
        /// </summary>
        public const int GlyphTriangleArrowLeft = 17;

        public const int GlyphArrowsUpDown = 18;

        public const int GlyphDoubleExclamation = 19;

        public const int GlyphNewLine = 20;

        public const int GlyphParagraph = 21;

        public const int GlyphUnderlineBold = 22;

        public const int GlyphArrowsUpDownToEnd = 23;

        /// <summary>
        /// ↑
        /// </summary>
        public const int GlyphArrowUp = 24;

        /// <summary>
        /// ↓
        /// </summary>
        public const int GlyphArrowDown = 25;

        /// <summary>
        /// →
        /// </summary>
        public const int GlyphArrowRight = 26;

        /// <summary>
        /// ←
        /// </summary>
        public const int GlyphArrowLeft = 27;

        public const int GlyphArrowsLeftRight = 29;

        /// <summary>
        /// ▲
        /// </summary>
        public const int GlyphTriangleArrowUp = 30;

        /// <summary>
        /// ▼
        /// </summary>
        public const int GlyphTriangleArrowDown = 31;

        public const int GlyphHouse = 127;

        public const int GlyphDoubleArrowLeft = 174;

        public const int GlyphDoubleArrowRight = 175;

        /// <summary>
        /// ░
        /// </summary>
        public const int GlyphBlockFilledRare = 176;

        public const int GlyphBoxFilledMedium = 177;

        /// <summary>
        /// ▓
        /// </summary>
        public const int GlyphBlockFilledThick = 178;

        /// <summary>
        /// │
        /// </summary>
        public const int GlyphBoxSingleVertical = 179;

        /// <summary>
        /// ┤
        /// </summary>
        public const int GlyphBoxSingleVerticalLeft = 180;

        public const int GlyphBoxSingleVerticalDoubleLeft = 181;

        public const int GlyphBoxDoubleVerticalSingleLeft = 182;

        public const int GlyphBoxDoubleDownSingleLeft = 183;

        public const int GlyphBoxSingleDownDoubleLeft = 184;

        public const int GlyphBoxDoubleVerticalLeft = 185;

        public const int GlyphBoxDoubleVertical = 186;

        public const int GlyphBoxDoubleDownLeft = 187;

        public const int GlyphBoxDoubleUpLeft = 188;

        public const int GlyphBoxDoubleUpSingleLeft = 189;

        public const int GlyphBoxSingleUpDoubleLeft = 190;

        /// <summary>
        /// ┐
        /// </summary>
        public const int GlyphBoxSingleDownLeft = 191;

        /// <summary>
        /// └
        /// </summary>
        public const int GlyphBoxSingleUpRight = 192;

        /// <summary>
        /// ┴
        /// </summary>
        public const int GlyphBoxSingleHorizontalUp = 193;

        /// <summary>
        /// ┬
        /// </summary>
        public const int GlyphBoxSingleHorizontalDown = 194;

        /// <summary>
        /// ├
        /// </summary>
        public const int GlyphBoxSingleVerticalRight = 195;

        /// <summary>
        /// ─
        /// </summary>
        public const int GlyphBoxSingleHorizontal = 196;

        /// <summary>
        /// ┼
        /// </summary>
        public const int GlyphBoxSingleHorizontalVertical = 197;

        /// <summary>
        /// ╞
        /// </summary>
        public const int GlyphBoxSingleVerticalDoubleRight = 198;

        /// <summary>
        /// ╟
        /// </summary>
        public const int GlyphBoxDoubleVerticalSingleRight = 199;

        /// <summary>
        /// ╚
        /// </summary>
        public const int GlyphBoxDoubleUpRight = 200;

        /// <summary>
        /// ╔
        /// </summary>
        public const int GlyphBoxDoubleDownRight = 201;

        /// <summary>
        /// ╩
        /// </summary>
        public const int GlyphBoxDoubleHorizontalUp = 202;

        /// <summary>
        /// ╦
        /// </summary>
        public const int GlyphBoxDoubleHorizontalDown = 203;

        /// <summary>
        /// ╠
        /// </summary>
        public const int GlyphBoxDoubleVerticalRight = 204;

        /// <summary>
        /// ═
        /// </summary>
        public const int GlyphBoxDoubleHorizontal = 205;

        /// <summary>
        /// ╬
        /// </summary>
        public const int GlyphBoxDoubleHorizontalVertical = 206;

        /// <summary>
        /// ╧
        /// </summary>
        public const int GlyphBoxDoubleHorizontalSingleUp = 207;

        /// <summary>
        /// 
        /// </summary>
        public const int GlyphBoxDoubleHorizontalSingleDown = 209;

        /// <summary>
        /// ╨
        /// </summary>
        public const int GlyphBoxSingleHorizontalDoubleUp = 208;

        /// <summary>
        /// ┘
        /// </summary>
        public const int GlyphBoxSingleUpLeft = 217;

        /// <summary>
        /// ┌
        /// </summary>
        public const int GlyphBoxSingleDownRight = 218;

        /// <summary>
        /// █
        /// </summary>
        public const int GlyphBlockFull = 219;

        public const int GlyphTorchVertical = 141;

        public const int GlyphTorchHorizontal = 170;

        #endregion
    }
}