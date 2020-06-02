using System;
using System.Threading.Tasks;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.GameProcess;
using CodeMagic.UI.Sad.Saving;
using Microsoft.Xna.Framework.Input;
using SadConsole.Input;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class InGameMenuView : View
    {
        private GameLogoControl gameLabel;
        private StandardButton continueGameButton;
        private StandardButton startGameButton;
        private StandardButton exitToMenuButton;
        private StandardButton exitButton;

        private readonly GameCore<Player> currentGame;

        public InGameMenuView(GameCore<Player> currentGame)
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

            continueGameButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 9),
                Text = "C0nt1nue Game"
            };
            continueGameButton.Click += continueGameButton_Click;
            Add(continueGameButton);

            startGameButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 13),
                Text = "Start New Game"
            };
            startGameButton.Click += startGameButton_Click;
            Add(startGameButton);

            exitToMenuButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 17),
                Text = "Ex1t t0 Menu"
            };
            exitToMenuButton.Click += exitToMenuButton_Click;
            Add(exitToMenuButton);

            exitButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 21),
                Text = "Ex1t"
            };
            exitButton.Click += exitButton_Click;
            Add(exitButton);
        }

        private void exitToMenuButton_Click(object sender, EventArgs e)
        {
            Close();

            new MainMenuView().Show();
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
            new SaveManager().SaveGame();
            SadConsole.Game.Instance.Exit();
        }

        private void startGameButton_Click(object sender, EventArgs args)
        {
            currentGame.Dispose();

            Close();

            var generatingView = new WaitMessageView("Starting new game...", () =>
            {
                var game = GameManager.Current.StartGame();
                var gameView = new GameView(game);
                gameView.Show();
            });
            generatingView.Show();
        }

        private int GetLabelPosition()
        {
            return (int)Math.Floor(Width / 2d) - 8;
        }
    }
}