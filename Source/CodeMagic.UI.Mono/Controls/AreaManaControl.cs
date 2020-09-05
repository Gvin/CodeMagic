using System;
using CodeMagic.Core.Area;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Controls
{
    public class AreaManaControl : Control
    {
        public AreaManaControl(Rectangle location) 
            : base(location)
        {
        }

        public IAreaMapCell Cell { get; set; }

        public override void Draw(ICellSurface surface)
        {
            if (Cell == null)
                return;

            var manaLevelPercent = (float)Cell.MagicEnergyLevel() / Cell.MaxMagicEnergyLevel();
            var disturbanceLevelPercent = (float)Cell.MagicDisturbanceLevel() / Cell.MaxMagicEnergyLevel();

            var manaLevelLength = (int)Math.Floor(Location.Width * manaLevelPercent);
            var disturbanceLevelLength = (int)Math.Ceiling(Location.Width * disturbanceLevelPercent);
            var leftLength = Location.Width - (manaLevelLength + disturbanceLevelLength);

            var shiftX = 0;

            for (int i = 0; i < disturbanceLevelLength; i++)
            {
                surface.Write(shiftX + i, 0, ".", Color.Black, Color.BlueViolet);
            }

            shiftX += disturbanceLevelLength;
            for (int i = 0; i < leftLength; i++)
            {
                surface.Write(shiftX + i, 0, " ", null, Color.DarkBlue);
            }

            shiftX += leftLength;
            for (int i = 0; i < manaLevelLength; i++)
            {
                surface.Write(shiftX + i, 0, ".", Color.Black, Color.DeepSkyBlue);
            }
        }
    }
}