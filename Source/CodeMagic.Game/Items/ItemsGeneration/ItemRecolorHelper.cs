using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.ItemsGeneration
{
    internal static class ItemRecolorHelper
    {
        private static readonly Dictionary<ItemMaterial, ColorPalette> Palette = new Dictionary<ItemMaterial, ColorPalette>
        {
            {ItemMaterial.Wood, new ColorPalette(Color.FromArgb(128, 64, 0), Color.FromArgb(64, 32, 32), Color.Yellow)},
            {ItemMaterial.Leather, new ColorPalette(Color.FromArgb(128, 64, 0), Color.FromArgb(64, 32, 32), Color.LightGray)},
            {ItemMaterial.Iron, new ColorPalette(Color.FromArgb(201, 192, 167), Color.FromArgb(138, 130, 109), Color.Aqua)},
            {ItemMaterial.Steel, new ColorPalette(Color.FromArgb(196, 194, 187), Color.FromArgb(145, 143, 137), Color.Red)},
            {ItemMaterial.Silver, new ColorPalette(Color.FromArgb(224, 224, 224), Color.FromArgb(171, 171, 171), Color.Violet)},
            {ItemMaterial.ElvesMetal, new ColorPalette(Color.FromArgb(217, 255, 179), Color.FromArgb(102, 204, 0), Color.Red)},
            {ItemMaterial.DwarfsMetal, new ColorPalette(Color.FromArgb(255, 255, 179), Color.FromArgb(204, 204, 0), Color.LightSkyBlue)},
            {ItemMaterial.Mythril, new ColorPalette(Color.FromArgb(194, 194, 163), Color.FromArgb(153, 153, 102), Color.Lime)}
        };

        private static readonly Color ReplaceColor1 = Color.FromArgb(255, 0, 0);
        private static readonly Color ReplaceColor2 = Color.FromArgb(0, 255, 0);
        private static readonly Color ReplaceColor3 = Color.FromArgb(0, 0, 255);

        private static readonly ColorPalette[] SpellBookColors = new[]
        {
            new ColorPalette(Color.FromArgb(0, 128, 255), Color.FromArgb(0, 0, 255), Color.White),
            new ColorPalette(Color.FromArgb(255, 128, 0), Color.FromArgb(128, 0, 0), Color.White),
            new ColorPalette(Color.FromArgb(0, 170, 85), Color.FromArgb(0, 102, 51), Color.White),
            new ColorPalette(Color.FromArgb(150, 45, 255), Color.FromArgb(75, 0, 151), Color.White),
        };

        public static SymbolsImage RecolorImage(SymbolsImage sourceImage, Color mainColor)
        {
            return SymbolsImage.Recolor(sourceImage, new Dictionary<Color, Color>
            {
                {ReplaceColor1, mainColor}
            });
        }

        public static SymbolsImage RecolorSpellBookImage(SymbolsImage sourceImage, out Color mainColor)
        {
            var palette = RandomHelper.GetRandomElement(SpellBookColors);
            mainColor = palette.Color1;
            return RecolorImage(sourceImage, palette);
        }

        public static SymbolsImage RecolorSpellBookGroundImage(SymbolsImage sourceImage, Color mainColor)
        {
            var palette = SpellBookColors.First(pal => pal.Color1 == mainColor);
            return RecolorImage(sourceImage, palette);
        }

        public static SymbolsImage RecolorItemImage(SymbolsImage sourceImage, ItemMaterial material)
        {
            var palette = Palette[material];
            return RecolorImage(sourceImage, palette);
        }

        private static SymbolsImage RecolorImage(SymbolsImage sourceImage, ColorPalette palette)
        {
            var recolorPalette = new Dictionary<Color, Color>
            {
                {ReplaceColor1, palette.Color1},
                {ReplaceColor2, palette.Color2},
                {ReplaceColor3, palette.Color3}
            };
            return SymbolsImage.Recolor(sourceImage, recolorPalette);
        }

        private class ColorPalette
        {
            public ColorPalette(Color color1, Color color2, Color color3)
            {
                Color1 = color1;
                Color2 = color2;
                Color3 = color3;
            }

            public Color Color1 { get; }

            public Color Color2 { get; }

            public Color Color3 { get; }
        }
    }
}