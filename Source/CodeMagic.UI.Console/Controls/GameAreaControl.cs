using System;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.LiquidObjects;
using CodeMagic.Core.Statuses;
using CodeMagic.UI.Console.Drawing;
using CodeMagic.UI.Console.Drawing.DrawingProcessors;
using CodeMagic.UI.Console.Drawing.Writing;

namespace CodeMagic.UI.Console.Controls
{
    public class GameAreaControl : ConsoleControl
    {
        private const bool DebugDrawTemperature = false;
        private const bool DebugDrawPressure = false;
        private const bool DebugDrawWaterLevel = false;

        private static readonly SymbolsImage EmptyImage = new SymbolsImage();

        private readonly GameCore game;
        private readonly IDrawingProcessorsFactory processorsFactory;
        private readonly FloorFactory floorFactory;

        public GameAreaControl(GameCore game)
        {
            this.game = game;

            processorsFactory = new DrawingProcessorsFactory();
            floorFactory = new FloorFactory();
        }

        protected override bool ProcessKey(ConsoleKeyInfo keyInfo)
        {
            base.ProcessKey(keyInfo);

            var action = GetPlayerAction(keyInfo.Key);
            if (action == null)
                return false;

            game.PerformPlayerAction(action);
            return true;
        }

        private IPlayerAction GetPlayerAction(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    return new MovePlayerAction(Direction.Up);
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    return new MovePlayerAction(Direction.Down);
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    return new MovePlayerAction(Direction.Left);
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    return new MovePlayerAction(Direction.Right);
                case ConsoleKey.Spacebar:
                    return new EmptyPlayerAction();
                case ConsoleKey.F:
                    return new MeleAttackPlayerAction();
            }

            return null;
        }

        protected override void DrawDynamic(IControlWriter writer)
        {
            base.DrawDynamic(writer);

            DrawGame(writer);
        }

        private void DrawGame(IControlWriter writer)
        {
            writer.BackColor = Color.Black;

            var visibleArea = game.GetVisibleArea();

            for (var y = 0; y < visibleArea.Height; y++)
            {
                for (var x = 0; x < visibleArea.Width; x++)
                {
                    var visibleCell = visibleArea.GetCell(x, y);
                    DrawCell(writer, visibleCell, x, y);
                }
            }
        }

        private void DrawCell(IControlWriter writer, AreaMapCell cell, int x, int y)
        {
            var image = GetCellImage(cell) ?? floorFactory.GetFloorImage(cell);
            var background = floorFactory.GetFloorColor(cell);

            var realX = x * SymbolsImage.Size;
            var realY = y * SymbolsImage.Size;
            writer.DrawImageAt(realX, realY, image, background);

            DrawPressure(writer, cell, realX, realY);
            DrawTemperature(writer, cell, realX, realY);
            DrawWaterLevel(writer, cell, realX, realY);
        }

        private void DrawPressure(IControlWriter writer, AreaMapCell cell, int x, int y)
        {
            if (!DebugDrawPressure)
                return;
            if (cell == null)
                return;

            writer.CursorY = y;
            writer.CursorX = x;
            writer.BackColor = Color.Black;
            writer.Write((int)Math.Round(cell.Environment.Pressure / 10d), Color.Violet);
        }

        private void DrawTemperature(IControlWriter writer, AreaMapCell cell, int x, int y)
        {
            if (!DebugDrawTemperature)
                return;
            if (cell == null)
                return;

            writer.CursorY = y + 1;
            writer.CursorX = x;
            writer.BackColor = Color.Black;
            writer.Write((int)Math.Round(cell.Environment.Temperature / 10d), Color.Red);
        }

        private void DrawWaterLevel(IControlWriter writer, AreaMapCell cell, int x, int y)
        {
            if (!DebugDrawWaterLevel)
                return;
            if (cell == null)
                return;

            writer.CursorY = y + 2;
            writer.CursorX = x;
            writer.BackColor = Color.Black;
            writer.Write(cell.Objects.GetLiquidVolume<WaterLiquidObject>(), Color.DeepSkyBlue);
        }

        private SymbolsImage GetCellImage(AreaMapCell cell)
        {
            if (cell == null)
                return EmptyImage;
            var bigObject = cell.Objects.FirstOrDefault(obj => obj.BlocksMovement);
            if (bigObject != null)
            {
                return GetObjectImage(bigObject);
            }

            return cell.Objects.Select(GetObjectImage).LastOrDefault(obj => obj != null);
        }

        private SymbolsImage GetObjectImage(object @object)
        {
            var processor = processorsFactory.GetProcessor(@object);
            var image = processor?.GetImage(@object);
            if (image == null)
                return null;

            if (@object is IDestroyableObject destroyable)
            {
                AddStatusImage(destroyable, image);
            }
            return image;
        }

        private void AddStatusImage(IDestroyableObject destroyable, SymbolsImage image)
        {
            if (destroyable.Statuses.Contains(OnFireObjectStatus.StatusType))
            {
                image.SetPixel(2, 2, '\u00C3', Color.Red, Color.Orange);
            }

            if (destroyable.Statuses.Contains(WetObjectStatus.StatusType))
            {
                image.SetPixel(2, 2, '\u2248', Color.White, Color.DeepSkyBlue);
            }
        }
    }
}