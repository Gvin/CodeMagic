using System;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Mono.Fonts;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Views
{
    public class PlayerDeathView : BaseWindow, IPlayerDeathView
    {
        public PlayerDeathView() : base(FontTarget.Interface)
        {
            Controls.Add(new Label(Width / 2 - 7, 12)
            {
                Text = "Y0u have d1ed!",
                ForeColor = Color.Red
            });

            var buttonsX = Width / 2 - 10;

            var startNewGameButton = new FramedButton(new Rectangle(buttonsX, 16, 20, 3))
            {
                Text = "Start New Game"
            };
            startNewGameButton.Click += (sender, args) => StartNewGame?.Invoke(this, EventArgs.Empty);
            Controls.Add(startNewGameButton);

            var backToMenuButton = new FramedButton(new Rectangle(buttonsX, 20, 20, 3))
            {
                Text = "Back t0 Menu"
            };
            backToMenuButton.Click += (sender, args) => ExitToMenu?.Invoke(this, EventArgs.Empty);
            Controls.Add(backToMenuButton);

            var exitGameButton = new FramedButton(new Rectangle(buttonsX, 24, 20, 3))
            {
                Text = "Ex1t Game"
            };
            exitGameButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Controls.Add(exitGameButton);
        }

        public event EventHandler StartNewGame;
        public event EventHandler ExitToMenu;
        public event EventHandler Exit;

        protected override void ProcessKeyPressed(Keys key)
        {
            base.ProcessKeyPressed(key);

            switch (key)
            {
                case Keys.Escape:
                    ExitToMenu?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }
    }
}