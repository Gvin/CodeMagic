﻿using System.Collections.Generic;
using System.Drawing;
using CodeMagic.Implementations.Items.Materials;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations
{
    public class MetalRecolorHelper
    {
        private static readonly Dictionary<MetalType, ColorPalette> Palette = new Dictionary<MetalType, ColorPalette>
        {
            {MetalType.Copper, new ColorPalette(Color.FromArgb(200, 125, 55), Color.FromArgb(160, 100, 44))},
            {MetalType.Bronze, new ColorPalette(Color.FromArgb(180, 107, 30), Color.FromArgb(123, 77, 30))},
            {MetalType.Iron, new ColorPalette(Color.FromArgb(201, 192, 167), Color.FromArgb(138, 130, 109))},
            {MetalType.Steel, new ColorPalette(Color.FromArgb(196, 194, 187), Color.FromArgb(145, 143, 137))},
            {MetalType.Silver, new ColorPalette(Color.FromArgb(224, 224, 224), Color.FromArgb(171, 171, 171))},
            {MetalType.ElvesMetal, new ColorPalette(Color.FromArgb(217, 255, 179), Color.FromArgb(102, 204, 0))},
            {MetalType.DwarfsMetal, new ColorPalette(Color.FromArgb(255, 255, 179), Color.FromArgb(204, 204, 0))},
            {MetalType.Mythril, new ColorPalette(Color.FromArgb(194, 194, 163), Color.FromArgb(153, 153, 102))},
            {MetalType.Adamant, new ColorPalette(Color.FromArgb(51, 51, 255), Color.FromArgb(0, 0, 179))}
        };

        private static readonly Color ReplaceColor1 = Color.FromArgb(255, 0, 0);
        private static readonly Color ReplaceColor2 = Color.FromArgb(0, 255, 0);

        public static SymbolsImage RecolorMetalImage(SymbolsImage sourceImage, MetalType metal)
        {
            var palete = Palette[metal];
            return RecolorImage(sourceImage, palete);
        }

        private static SymbolsImage RecolorImage(SymbolsImage sourceImage, ColorPalette palette)
        {
            var recolorPalette = new Dictionary<Color, Color>
            {
                {ReplaceColor1, palette.Color1},
                {ReplaceColor2, palette.Color2}
            };
            return SymbolsImage.Recolor(sourceImage, recolorPalette);
        }

        private class ColorPalette
        {
            public ColorPalette(Color color1, Color color2)
            {
                Color1 = color1;
                Color2 = color2;
            }

            public Color Color1 { get; }

            public Color Color2 { get; }
        }
    }
}