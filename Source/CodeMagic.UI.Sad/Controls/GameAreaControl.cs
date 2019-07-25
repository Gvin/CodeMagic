using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Drawing;
using Microsoft.Xna.Framework;
using SadConsole.Controls;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Controls
{
    public class GameAreaControl : ControlBase
    {
        private static readonly Color DefaultForegroundColor = Color.White;
        private static readonly Color DefaultBackgroundColor = Color.Black;

        private const int ControlWidth = 30;
        private const int ControlHeight = 30;

        private readonly GameCore game;

        public GameAreaControl(GameCore game) 
            : base(ControlWidth, ControlHeight)
        {
            this.game = game;

            Theme = new DrawingSurfaceTheme();
            CanFocus = false;
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            var visibleArea = game.GetVisibleArea();

            for (int y = 0; y < visibleArea.Height; y++)
            {
                for (int x = 0; x < visibleArea.Width; x++)
                {
                    var cell = visibleArea.GetCell(x, y);
                    DrawCell(x, y, cell);
                }
            }
        }

        private void DrawCell(int mapX, int mapY, AreaMapCell cell)
        {
            var image = CellImageHelper.GetCellImage(cell);

            var realX = mapX * Program.MapCellImageSize;
            var realY = mapY * Program.MapCellImageSize;

            Surface.DrawImage(realX, realY, image, DefaultForegroundColor, DefaultBackgroundColor);
        }
    }
}