using System.ComponentModel;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.UI.Console.Drawing
{
    public class FloorFactory
    {
        private static readonly Color WaterColor = Color.CadetBlue;
        private static readonly Color WaterIceColor = Color.DarkTurquoise;

        private static readonly Color AcidColor = Color.GreenYellow;
        private static readonly Color AcidIceColor = Color.LightGreen;

        private static readonly Color DirtColor = Color.FromArgb(107, 83, 49);
        private static readonly Color WoodColor = Color.FromArgb(99, 63, 11);

        public Color GetFloorColor(AreaMapCell cell)
        {
            if (cell == null)
                return Color.Black;

            var waterIce = cell.Objects.OfType<WaterIceObject>().FirstOrDefault();
            if (waterIce != null && waterIce.Volume >= WaterIceObject.WaterIceMinVolumeForEffect)
                return WaterIceColor;

            var acidIce = cell.Objects.OfType<AcidIceObject>().FirstOrDefault();
            if (acidIce != null && acidIce.Volume >= AcidIceObject.AcidIceMinVolumeForEffect)
                return AcidIceColor;

            var waterLevel = cell.Objects.GetLiquidVolume<WaterLiquidObject>();
            if (waterLevel >= WaterLiquidObject.WaterMinVolumeForEffect)
                return WaterColor;

            var acidLevel = cell.Objects.GetLiquidVolume<AcidLiquidObject>();
            if (acidLevel >= AcidLiquidObject.AcidMinVolumeForEffect)
                return AcidColor;

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
            var waterIce = cell.Objects.OfType<WaterIceObject>().FirstOrDefault();
            if (waterIce != null && waterIce.Volume >= WaterIceObject.WaterIceMinVolumeForEffect)
                return GetWaterIceFloorImage();

            var acidIce = cell.Objects.OfType<AcidIceObject>().FirstOrDefault();
            if (acidIce != null && acidIce.Volume >= AcidIceObject.AcidIceMinVolumeForEffect)
                return GetAcidIceFloorImage();

            var waterLevel = cell.Objects.GetLiquidVolume<WaterLiquidObject>();
            if (waterLevel >= WaterLiquidObject.WaterMinVolumeForEffect)
                return GetWaterFloorImage();

            var acidLevel = cell.Objects.GetLiquidVolume<AcidLiquidObject>();
            if (acidLevel >= AcidLiquidObject.AcidMinVolumeForEffect)
                return GetAcidFloorImage();

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

        private SymbolsImage GetWaterIceFloorImage()
        {
            var image = new SymbolsImage();

            image.SetDefaultBackColor(WaterIceColor);
            image.SetDefaultColor(Color.White);

            image.SetSymbolMap(new[]
            {
                new[] {' ', '/', ' '},
                new[] {' ', ' ', ' '},
                new[] {'/', ' ', '/'}
            });

            return image;
        }

        private SymbolsImage GetAcidIceFloorImage()
        {
            var image = new SymbolsImage();

            image.SetDefaultBackColor(AcidIceColor);
            image.SetDefaultColor(Color.White);

            image.SetSymbolMap(new[]
            {
                new[] {' ', '/', ' '},
                new[] {' ', ' ', ' '},
                new[] {'/', ' ', '/'}
            });

            return image;
        }

        private SymbolsImage GetAcidFloorImage()
        {
            var image = new SymbolsImage();

            image.SetDefaultBackColor(AcidColor);
            image.SetDefaultColor(Color.White);

            image.SetSymbolMap(new[]
            {
                new[] {' ', '\u2248', ' '},
                new[] {'\u2248', ' ', '\u2248'},
                new[] {' ', '\u2248', ' '}
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
                new[] {LineTypes.SingleVertical, LineTypes.SingleVerticalRight, LineTypes.SingleVerticalLeft},
                new[] {LineTypes.SingleVerticalRight, LineTypes.SingleVerticalLeft, LineTypes.SingleVertical},
                new[] {LineTypes.SingleVerticalLeft, LineTypes.SingleVertical, LineTypes.SingleVerticalRight }
            });

            return image;
        }
    }
}