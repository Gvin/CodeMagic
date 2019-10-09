using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Drawing;
using Microsoft.Xna.Framework;
using SadConsole;
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

        private readonly IGameCore game;

        public GameAreaControl(IGameCore game) 
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

            if (visibleArea == null)
                return;

            for (int y = 0; y < visibleArea.Height; y++)
            {
                for (int x = 0; x < visibleArea.Width; x++)
                {
                    var cell = visibleArea.GetCell(x, y);
                    DrawCell(x, y, cell);
                }
            }
        }

        private void DrawCell(int mapX, int mapY, IAreaMapCell cell)
        {
            var image = CellImageHelper.GetCellImage(cell);

            var realX = mapX * Program.MapCellImageSize;
            var realY = mapY * Program.MapCellImageSize;

            Surface.DrawImage(realX, realY, image, DefaultForegroundColor, DefaultBackgroundColor);

            DrawDebugData(realX, realY, cell);
        }

        #region Debug Data Drawing

        private void DrawDebugData(int realX, int realY, IAreaMapCell cell)
        {
            if (Properties.Settings.Default.DebugDrawTemperature)
                DrawTemperature(realX, realY, cell);
            if (Properties.Settings.Default.DebugDrawLightLevel)
                DrawLightLevel(realX, realY + 1, cell);
            if (Properties.Settings.Default.DebugDrawMagicEnergy)
                DrawMagicEnergy(realX, realY + 1, cell);
        }

        private void DrawLightLevel(int x, int y, IAreaMapCell cell)
        {
            if (cell == null)
                return;

            var value = (int)cell.LightLevel;
            Surface.Print(x, y, new ColoredString(value.ToString(), Color.Yellow, Color.Black));
        }

        private void DrawTemperature(int x, int y, IAreaMapCell cell)
        {
            if (cell == null)
                return;

            var value = cell.Temperature / 10;
            Surface.Print(x, y, new ColoredString(value.ToString(), Color.Red, Color.Black));
        }

        private void DrawMagicEnergy(int x, int y, IAreaMapCell cell)
        {
            if (cell == null)
                return;

            Surface.Print(x, y, new ColoredString(cell.MagicEnergyLevel.ToString(), Color.Blue, Color.Black));
            Surface.Print(x, y + 1, new ColoredString(cell.MagicDisturbanceLevel.ToString(), Color.Blue, Color.Black));
        }

        #endregion
    }
}