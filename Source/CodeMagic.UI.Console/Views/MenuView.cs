using System;
using System.Drawing;
using CodeMagic.UI.Console.Controls;
using CodeMagic.UI.Console.Drawing.Writing;

namespace CodeMagic.UI.Console.Views
{
    public class MenuView : View
    {
        private StaticTextBlockControl logoTextBox;
        private VerticalMenuControl<int> menu;

        public MenuView()
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            var leftPos = GetLeftPosition();

            logoTextBox = new StaticTextBlockControl
            {
                X = leftPos,
                Y = 3,
                Lines = new []
                {
                    "     <- Code Magic ->",
                    "        |  \'   | `"
                },
                TextColor = new []
                {
                    Color.BlueViolet,
                    Color.Red
                }
            };

            Controls.Add(logoTextBox);

            menu = new VerticalMenuControl<int>
            {
                X = leftPos,
                Y = 10,
                SelectedFrameColor = Color.Yellow,
                TextColor = Color.Green
            };
            menu.Items.Add(new MenuItem<int>("       Start Game       ", 0));
            menu.Items.Add(new MenuItem<int>("          Exit          ", 1));
            menu.SelectedItemIndex = 0;

            Controls.Add(menu);
        }

        private int GetLeftPosition()
        {
            return (int)Math.Floor(Writer.ScreenWidth / 2d) - 12;
        }

        public override void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            base.ProcessKey(keyInfo);

            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    ViewsManager.Current.Exit();
                    break;
                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                    ProcessSelectedOption();
                    break;
            }

        }

        private void ProcessSelectedOption()
        {
            switch (menu.SelectedItem.Data)
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