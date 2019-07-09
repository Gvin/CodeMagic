using System.ComponentModel;
using System.Drawing;
using CodeMagic.Core.Area;

namespace CodeMagic.UI.Console.Drawing
{
    public class FloorColorFactory
    {
        public Color GetFloorColor(FloorTypes type)
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