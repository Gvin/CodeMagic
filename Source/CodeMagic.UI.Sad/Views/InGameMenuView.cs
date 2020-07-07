using System;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework.Input;
using SadConsole.Input;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class InGameMenuView : GameViewBase, IInGameMenuView
    {
        private GameLogoControl gameLabel;
        private StandardButton continueGameButton;
        private StandardButton startGameButton;
        private StandardButton exitToMenuButton;
        private StandardButton exitButton;

        public InGameMenuView()
        {
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
            continueGameButton.Click += (sender, args) => ContinueGame?.Invoke(this, EventArgs.Empty);
            Add(continueGameButton);

            startGameButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 13),
                Text = "Start New Game"
            };
            startGameButton.Click += (sender, args) => StartNewGame?.Invoke(this, EventArgs.Empty);
            Add(startGameButton);

            exitToMenuButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 17),
                Text = "Ex1t t0 Menu"
            };
            exitToMenuButton.Click += (sender, args) => ExitToMenu?.Invoke(this, EventArgs.Empty);
            Add(exitToMenuButton);

            exitButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 21),
                Text = "Ex1t"
            };
            exitButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Add(exitButton);
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            if (key.Key == Keys.Escape)
            {
                ContinueGame?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return base.ProcessKeyPressed(key);
        }

        private int GetLabelPosition()
        {
            return (int)Math.Floor(Width / 2d) - 8;
        }

        public void Close()
        {
            Close(DialogResult.None);
        }

        public event EventHandler ContinueGame;
        public event EventHandler StartNewGame;
        public event EventHandler Exit;
        public event EventHandler ExitToMenu;
    }
}