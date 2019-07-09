using System;
using System.Drawing;
using CodeMagic.UI.Console.Drawing;
using Writer = Colorful.Console;

namespace CodeMagic.UI.Console.Views
{
    public class MenuView : View
    {
        private static readonly Color SelectedFrameColor = Color.Yellow;

        private int selectedIndex = 0;

        public override void DrawStatic()
        {
            var leftPos = GetLeftPosition();
            Writer.CursorTop = 3;
            Writer.CursorLeft = leftPos;

            Writer.WriteLine("     <- Code Magic ->", Color.BlueViolet);
            Writer.CursorLeft = leftPos;
            Writer.Write("        |  \'   | `", Color.DarkRed);
        }

        private int GetLeftPosition()
        {
            return (int)Math.Floor(Writer.WindowWidth / 2d) - 12;
        }

        private void DrawOption(string text, int index, int leftPosition)
        {
            Writer.CursorLeft = leftPosition;
            
            if (index == selectedIndex)
            {
                Writer.Write(LineTypes.SingleDownRight, SelectedFrameColor);
                DrawingHelper.DrawHorizontalLine(Writer.CursorTop, leftPosition + 1, leftPosition + 24, false, SelectedFrameColor);
                Writer.Write(LineTypes.SingleDownLeft, SelectedFrameColor);
            }
            else
            {
                Writer.Write("                                                      ");
            }

            Writer.CursorLeft = leftPosition;
            Writer.CursorTop++;
            Writer.Write(index == selectedIndex ? LineTypes.SingleVertical : ' ', SelectedFrameColor);
            Writer.Write(text, Color.Green);
            Writer.Write(index == selectedIndex ? LineTypes.SingleVertical : ' ', SelectedFrameColor);
            Writer.CursorTop++;

            Writer.CursorLeft = leftPosition;
            if (index == selectedIndex)
            {
                Writer.Write(LineTypes.SingleUpRight, SelectedFrameColor);
                DrawingHelper.DrawHorizontalLine(Writer.CursorTop, leftPosition + 1, leftPosition + 24, false, SelectedFrameColor);
                Writer.Write(LineTypes.SingleUpLeft, SelectedFrameColor);
            }
            else
            {
                Writer.Write("                                                      ");
            }
        }

        public override void DrawDynamic()
        {
            var leftPos = GetLeftPosition();
            Writer.CursorTop = 10;
            DrawOption("       Start Game       ", 0, leftPos);
            Writer.CursorTop++;
            DrawOption("          Exit          ", 1, leftPos);
        }

        public override void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    selectedIndex--;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    selectedIndex++;
                    break;
                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                    ProcessSelectedOption();
                    break;
            }

            if (selectedIndex > 1)
                selectedIndex = 0;
            if (selectedIndex < 0)
                selectedIndex = 1;
        }

        private void ProcessSelectedOption()
        {
            switch (selectedIndex)
            {
                case 0:
                    ProcessStartGame();
                    break;
                case 1:
                    ProcessExit();
                    break;
            }
        }

        private void ProcessStartGame()
        {
            Close();

            var game = new GameManager().StartGame();
            var gameView = new GameView(game);
            gameView.Show();
        }

        private void ProcessExit()
        {
            ViewsManager.Current.Exit();
        }
    }
}