using System.ComponentModel;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Area.Liquids;
using CodeMagic.Core.Objects.DecorativeObjects;

namespace CodeMagic.UI.Console.Drawing
{
    public class FloorFactory
    {
        private static readonly Color WaterColor = Color.CadetBlue;
        private static readonly Color IceColor = Color.DarkTurquoise;
        private static readonly Color DirtColor = Color.FromArgb(107, 83, 49);
        private static readonly Color WoodColor = Color.FromArgb(99, 63, 11);

        public Color GetFloorColor(AreaMapCell cell)
        {
            if (cell == null)
                return Color.Black;

            var iceObject = cell.Objects.OfType<IceObject>().FirstOrDefault();
            if (iceObject != null && iceObject.Volume >= IceObject.MinVolumeForEffect)
                return IceColor;

            var waterLevel = cell.Liquids.GetLiquidVolume<WaterLiquid>();
            if (waterLevel >= WaterLiquid.MinVolumeForEffect)
                return WaterColor;

            return GetStandardFloorColor(cell.FloorType);
        }

        private Color GetStandardFloorColor(FloorTypes type)
        {
            switch (type)
            {
                case FloorTypes.Dirt:
                    return DirtColor;
                case FloorTypes.Wood:
                    return WoodColor;
                case FloorTypes.Stone:
                    return Color.DarkGray;
                case FloorTypes.Grass:
                    return Color.DarkOliveGreen;
                case FloorTypes.Hole:
                    return Color.Black;
                default:
                    throw new InvalidEnumArgumentException($"Unknown floor type {type}");
            }
        }

        public SymbolsImage GetFloorImage(AreaMapCell cell)
        {
            var iceObject = cell.Objects.OfType<IceObject>().FirstOrDefault();
            if (iceObject != null && iceObject.Volume >= IceObject.MinVolumeForEffect)
                return GetIceFloorImage();

            var waterLevel = cell.Liquids.GetLiquidVolume<WaterLiquid>();
            if (waterLevel >= WaterLiquid.MinVolumeForEffect)
                return GetWaterFloorImage();

            return GetStandardFloorImage(cell.FloorType);
        }

        private SymbolsImage GetStandardFloorImage(FloorTypes type)
        {
            switch (type)
            {
                case FloorTypes.Wood:
                    return GetWoodFloorImage();
                case FloorTypes.Dirt:
                    return GetDirtFloorImage();
                case FloorTypes.Stone:
                    return GetStoneFloorImage();
                case FloorTypes.Grass:
                    return GetGrassFloorImage();
                default:
                    return new SymbolsImage();
            }
        }

        private SymbolsImage GetIceFloorImage()
        {
            var image = new SymbolsImage();

            image.SetDefaultBackColor(IceColor);
            image.SetDefaultColor(Color.White);

            image.SetSymbolMap(new[]
            {
                new[] {' ', '/', ' '},
                new[] {' ', ' ', ' '},
                new[] {'/', ' ', '/'}
            });

            return image;
        }

        private SymbolsImage GetWaterFloorImage()
        {
            var image = new SymbolsImage();

            image.SetDefaultBackColor(WaterColor);
            image.SetDefaultColor(Color.White);

            image.SetSymbolMap(new[]
            {
                new[] {' ', '\u2248', ' '},
                new[] {'\u2248', ' ', '\u2248'},
                new[] {' ', '\u2248', ' '}
            });

            return image;
        }

        private SymbolsImage GetGrassFloorImage()
        {
            var image = new SymbolsImage();

            image.SetDefaultBackColor(Color.DarkOliveGreen);
            image.SetDefaultColor(Color.DarkGreen);

            image.SetSymbolMap(new[]
            {
                new[] {'/', '/', '/'},
                new[] {'/', '/', '/'},
                new[] {'/', '/', '/'}
            });

            return image;
        }

        private SymbolsImage GetStoneFloorImage()
        {
            var image = new SymbolsImage();

            image.SetDefaultBackColor(Color.DarkGray);
            image.SetDefaultColor(Color.Gray);

            image.SetSymbolMap(new[]
            {
                new[] {' ', ' ', ','},
                new[] {'.', ' ', ' '},
                new[] {' ', ' ', '`'}
            });

            return image;
        }

        private SymbolsImage GetDirtFloorImage()
        {
            var image = new SymbolsImage();

            image.SetDefaultBackColor(DirtColor);
            image.SetDefaultColor(Color.Black);

            image.SetSymbolMap(new[]
            {
                new[] {' ', ' ', ','},
                new[] {'.', ' ', ' '},
                new[] {' ', ' ', '`'}
            });

            return image;
        }

        private SymbolsImage GetWoodFloorImage()
        {
            var image = new SymbolsImage();

            image.SetDefaultBackColor(WoodColor);
            image.SetDefaultColor(Color.FromArgb(61, 44, 20));

            image.SetSymbolMap(new[]
            {
                new[] {LineTypes.SingleVertical, LineTypes.SingleVerticalAndRight, LineTypes.SingleVerticalAndLeft},
                new[] {LineTypes.SingleVerticalAndRight, LineTypes.SingleVerticalAndLeft, LineTypes.SingleVertical},
                new[] {LineTypes.SingleVerticalAndLeft, LineTypes.SingleVertical, LineTypes.SingleVerticalAndRight }
            });

            return image;
        }
    }
}