using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Drawing;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;
using Point = CodeMagic.Core.Game.Point;

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

            for (int y = 0; y < visibleArea.Height; y++)
            {
                for (int x = 0; x < visibleArea.Width; x++)
                {
                    var realPosition = new Point(visibleArea.Position.X + x, visibleArea.Position.Y + y);
                    DrawCell(x, y, realPosition, visibleArea.GetCell(x, y) != null);
                }
            }
        }

        private void DrawCell(int mapX, int mapY, Point realPosition, bool isVisible)
        {
            var image = CellImageHelper.GetCellImage(game.Map, realPosition, isVisible);

            var realX = mapX * Program.MapCellImageSize;
            var realY = mapY * Program.MapCellImageSize;

            Surface.DrawImage(realX, realY, image, DefaultForegroundColor, DefaultBackgroundColor);

            DrawDebugData(realX, realY, game.Map.TryGetCell(realPosition));
        }

        #region Debug Data Drawing

        private void DrawDebugData(int realX, int realY, AreaMapCell cell)
        {
            if (Properties.Settings.Default.DebugDrawTemperature)
                DrawTemperature(realX, realY, cell);
            if (Properties.Settings.Default.DebugDrawLightLevel)
                DrawLightLevel(realX, realY + 1, cell);
        }

        private void DrawLightLevel(int x, int y, AreaMapCell cell)
        {
            if (cell == null)
                return;

            var value = (int)cell.LightLevel;
            Surface.Print(x, y, new ColoredString(value.ToString(), Color.Yellow, Color.Black));
        }

        private void DrawTemperature(int x, int y, AreaMapCell cell)
        {
            if (cell == null)
                return;

            var value = cell.Environment.Temperature / 10;
            Surface.Print(x, y, new ColoredString(value.ToString(), Color.Red, Color.Black));
        }

        #endregion
    }
}