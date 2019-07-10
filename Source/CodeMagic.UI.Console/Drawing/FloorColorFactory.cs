using System.ComponentModel;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Area.Liquids;
using CodeMagic.Core.Objects.DecorativeObjects;

namespace CodeMagic.UI.Console.Drawing
{
    public class FloorColorFactory
    {
        private static readonly Color WaterColor = Color.CadetBlue;

        private static readonly Color IceColor = Color.Aquamarine;

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
                    return Color.FromArgb(107, 83, 49);
                case FloorTypes.Wood:
                    return Color.SaddleBrown;
                case FloorTypes.Stone:
                    return Color.DarkGray;
                case FloorTypes.Grass:
                    return Color.DarkOliveGreen;
                case FloorTypes.BurnedWood:
                    return Color.FromArgb(46, 45, 43);
                case FloorTypes.Hole:
                    return Color.Black;
                default:
                    throw new InvalidEnumArgumentException($"Unknown floor type {type}");
            }
        }
    }
}