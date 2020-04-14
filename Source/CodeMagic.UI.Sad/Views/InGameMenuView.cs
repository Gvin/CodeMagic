using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.GameProcess;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class InGameMenuView : View
    {
        private GameLogoControl gameLabel;
        private Button continueGameButton;
        private Button startGameButton;
        private Button exitButton;

        private readonly CurrentGame.GameCore<Player> currentGame;

        public InGameMenuView(CurrentGame.GameCore<Player> currentGame) 
            : base(Program.Width, Program.Height)
        {
            this.currentGame = currentGame;

            InitializeControls();
        }

        private void InitializeControls()
        {
            var xPosition = GetLabelPosition();

            gameLabel = new GameLogoControl
            {
                Position = new Point(xPosition, 4)
            };
            Add(gameLabel);

            var menuButtonsTheme = new ButtonLinesTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(DefaultForeground, DefaultBackground)
                }
            };

            continueGameButton = new Button(20, 3)
            {
                Position = new Point(xPosition - 2, 9),
                Text = "C0nt1nue Game",
                Theme = menuButtonsTheme,
                CanFocus = false
            };
            continueGameButton.Click += continueGameButton_Click;
            Add(continueGameButton);

            startGameButton = new Button(20, 3)
            {
                Position = new Point(xPosition - 2, 13),
                Text = "Start New Game",
                Theme = menuButtonsTheme,
                CanFocus = false
            };
            startGameButton.Click += startGameButton_Click;
            Add(startGameButton);

            exitButton = new Button(20, 3)
            {
                Position = new Point(xPosition - 2, 17),
                Text = "Ex1t",
                Theme = menuButtonsTheme,
                CanFocus = false
            };
            exitButton.Click += exitButton_Click;
            Add(exitButton);
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            if (key.Key == Keys.Escape)
            {
                ContinueCurrentGame();
                return true;
            }
            return base.ProcessKeyPressed(key);
        }

        private void continueGameButton_Click(object sender, EventArgs e)
        {
            ContinueCurrentGame();
        }

        private void ContinueCurrentGame()
        {
            Close();

            new GameView(currentGame).Show();
        }

        private void exitButton_Click(object sender, EventArgs args)
        {
            SadConsole.Game.Instance.Exit();
        }

        private void startGameButton_Click(object sender, EventArgs args)
        {
            currentGame.Dispose();

            Close();

            var game = new GameManager().StartGame();
            var gameView = new GameView(game);
            gameView.Show();
        }

        private int GetLabelPosition()
        {
            return (int)Math.Floor(Width / 2d) - 8;
        }
    }
}