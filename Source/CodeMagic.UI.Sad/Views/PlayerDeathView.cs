using System;
using CodeMagic.Game;
using CodeMagic.Game.GameProcess;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using SadConsole;

namespace CodeMagic.UI.Sad.Views
{
    public class PlayerDeathView : GameViewBase, IPlayerDeathView
    {
        private StandardButton startNewGameButton;
        private StandardButton backToMenuButton;
        private StandardButton exitGameButton;

        public PlayerDeathView()
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            var buttonsX = Width / 2 - 10;

            startNewGameButton = new StandardButton(20)
            {
                Position = new Point(buttonsX, 16),
                Text = "Start New Game"
            };
            startNewGameButton.Click += (sender, args) => StartNewGame?.Invoke(this, EventArgs.Empty);
            Add(startNewGameButton);

            backToMenuButton = new StandardButton(20)
            {
                Position = new Point(buttonsX, 20),
                Text = "Back t0 Menu"
            };
            backToMenuButton.Click += (sender, args) => ExitToMenu?.Invoke(this, EventArgs.Empty);
            Add(backToMenuButton);

            exitGameButton = new StandardButton(20)
            {
                Position = new Point(buttonsX, 24),
                Text = "Ex1t Game"
            };
            exitGameButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Add(exitGameButton);
        }

        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            var labelX = Width / 2 - 7;
            surface.Print(labelX, 12, new ColoredString("You have died!", Color.Red, DefaultBackground));
        }

        public void Close()
        {
            Close(DialogResult.None);
        }

        public event EventHandler StartNewGame;
        public event EventHandler ExitToMenu;
        public event EventHandler Exit;
    }
}