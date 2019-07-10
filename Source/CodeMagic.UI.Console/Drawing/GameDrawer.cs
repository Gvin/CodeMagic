using System;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Area.Liquids;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Core.Statuses;
using CodeMagic.UI.Console.Drawing.DrawingProcessors;
using CodeMagic.UI.Console.Drawing.JournalTextProviding;
using Writer = Colorful.Console;

namespace CodeMagic.UI.Console.Drawing
{
    public class GameDrawer
    {
        private const int GameScreenTopShift = 1;
        private const int GameScreenLeftShift = 1;
        private const int PlayerDataRightShift = 45;
        private const int JournalBottomShift = 8;
        private const int MaxJournalMessagesCount = 5;

        private static readonly SymbolsImage EmptyImage = new SymbolsImage();

        private readonly IDrawingProcessorsFactory processorsFactory;
        private readonly FloorColorFactory floorColorFactory;
        private readonly JournalTextProviderFactory journalTextProviderFactory;

        public GameDrawer(IDrawingProcessorsFactory processorsFactory, FloorColorFactory floorColorFactory)
        {
            this.processorsFactory = processorsFactory;
            this.floorColorFactory = floorColorFactory;

            journalTextProviderFactory = new JournalTextProviderFactory();
        }

        public void DrawStaticElements()
        {
            Writer.CursorTop = Writer.WindowHeight - JournalBottomShift;
            Writer.BackgroundColor = Color.Black;

            // Journal line
            Writer.CursorLeft = 0;
            Writer.Write(LineTypes.DoubleVerticalAndSingleRight, Color.Gray);
            DrawingHelper.DrawHorizontalLine(Writer.CursorTop, 1, Writer.WindowWidth - 2, false, Color.Gray);
            Writer.Write(LineTypes.DoubleVerticalAndSingleLeft, Color.Gray);

            // Player stats box
            var leftPosition = Writer.WindowWidth - PlayerDataRightShift;
            DrawingHelper.WriteAt(LineTypes.DoubleHorizontalAndSingleDown, leftPosition - 1, 0, Color.Gray);
            DrawingHelper.DrawVerticalLine(leftPosition - 1, 1, Writer.WindowHeight - JournalBottomShift - 1, false, Color.Gray);
            DrawingHelper.WriteAt(LineTypes.SingleHorizontalAndUp, leftPosition - 1, Writer.WindowHeight - JournalBottomShift, Color.Gray);

            Writer.CursorTop = GameScreenTopShift;
            Writer.CursorLeft = leftPosition;
            Writer.BackgroundColor = Color.Black;

            Writer.Write("Player Stats", Color.White);
            DrawingHelper.DrawHorizontalLine(Writer.CursorTop + 1, leftPosition, Writer.WindowWidth - 2, false, Color.Gray);
            Writer.Write(LineTypes.DoubleVerticalAndSingleLeft, Color.Gray);
            DrawingHelper.WriteAt(LineTypes.SingleVerticalAndRight, leftPosition - 1, Writer.CursorTop - 1, Color.Gray);
        }

        private void DrawPlayerData(IPlayer player)
        {
            var leftPosition = Writer.WindowWidth - PlayerDataRightShift;

            Writer.CursorTop = GameScreenTopShift;
            Writer.CursorLeft = leftPosition;
            Writer.BackgroundColor = Color.Black;

            Writer.CursorTop += 2;
            Writer.CursorLeft = leftPosition;
            Writer.Write("HP:   ", Color.White);
            Writer.Write($"{player.Health} / {player.MaxHealth}   ", Color.Red);

            Writer.CursorTop++;
            Writer.CursorLeft = leftPosition;
            Writer.Write("Mana: ", Color.White);
            Writer.Write($"{player.Mana} / {player.MaxMana}     ", Color.Blue);

            Writer.CursorTop += 2;
            Writer.CursorLeft = leftPosition;
            Writer.WriteLine("Weapon:", Color.White);
            Writer.CursorLeft = leftPosition;
            if (player.Equipment.Weapon == null)
            {
                Writer.WriteLine("[Nothing]", Color.Gray);
            }
            else
            {
                Writer.WriteLine($"[{player.Equipment.Weapon.Name}]", ItemDrawingHelper.GetItemNameColor(player.Equipment.Weapon));
            }

            Writer.CursorTop++;

            Writer.CursorLeft = leftPosition;
            Writer.WriteLine($"Damage: {player.Equipment.MinDamage} - {player.Equipment.MaxDamage}", Color.White);
            Writer.CursorLeft = leftPosition;
            Writer.WriteLine($"Protection: {player.Equipment.Protection}", Color.White);

            Writer.CursorTop += 2;
            Writer.CursorLeft = leftPosition;
            Writer.WriteLine("Actions:", Color.White);
            Writer.CursorLeft = leftPosition;
            Writer.WriteLine("[F] - Mele Attack", Color.White);
            Writer.CursorLeft = leftPosition;
            Writer.WriteLine("[C] - Spell Book", Color.White);
        }

        public void DrawGame(GameCore game)
        {
            Writer.BackgroundColor = Color.Black;

            var visibleArea = game.GetVisibleArea();

            for (var y = 0; y < visibleArea.Height; y++)
            {
                for (var x = 0; x < visibleArea.Width; x++)
                {
                    var visibleCell = visibleArea.GetCell(x, y);
                    DrawCell(visibleCell, x, y);
                }
            }

            DrawJournal(game.Journal);
            DrawPlayerData(game.Player);
        }

        private void DrawJournal(Journal journal)
        {
            Writer.CursorTop = Writer.WindowHeight - JournalBottomShift + 1;
            Writer.BackgroundColor = Color.Black;

            var messages = journal.GetLastMessages(MaxJournalMessagesCount);
            foreach (var message in messages)
            {
                var text = $"[{message.Turn}] " + journalTextProviderFactory.GetMessageText(message.Message);
                text = text.Length > Writer.WindowWidth - 1 ? text.Substring(0, Writer.WindowWidth - 1) : text;
                Writer.CursorLeft = GameScreenLeftShift;
                while (text.Length < Writer.WindowWidth - 2)
                    text += " ";
                Writer.WriteLine(text, Color.DarkGray);
            }
        }

        private void DrawCell(AreaMapCell cell, int x, int y)
        {
            var image = GetCellImage(cell) ?? EmptyImage;
            var background = floorColorFactory.GetFloorColor(cell);

            var realX = x * SymbolsImage.Size + GameScreenLeftShift;
            var realY = y * SymbolsImage.Size + GameScreenTopShift;
            DrawingHelper.DrawImageAt(realX, realY, image, background);

//            DrawPressure(cell, realX, realY);
//            DrawTemperature(cell, realX, realY);
//            DrawWaterLevel(cell, realX, realY);
        }

        private void DrawPressure(AreaMapCell cell, int x, int y)
        {
            if (cell == null)
                return;

            Writer.CursorTop = y;
            Writer.CursorLeft = x;
            Writer.BackgroundColor = Color.Black;
            Writer.Write((int)Math.Round(cell.Environment.Pressure / 10d), Color.Violet);
        }

        private void DrawTemperature(AreaMapCell cell, int x, int y)
        {
            if (cell == null)
                return;

            Writer.CursorTop = y + 1;
            Writer.CursorLeft = x;
            Writer.BackgroundColor = Color.Black;
            Writer.Write((int)Math.Round(cell.Environment.Temperature / 10d), Color.Red);
        }

        private void DrawWaterLevel(AreaMapCell cell, int x, int y)
        {
            if (cell == null)
                return;

            Writer.CursorTop = y + 2;
            Writer.CursorLeft = x;
            Writer.BackgroundColor = Color.Black;
            Writer.Write(cell.Liquids.GetLiquidVolume<WaterLiquid>(), Color.DeepSkyBlue);
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