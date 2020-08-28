using System;
using CodeMagic.UI.Mono.Controls;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Mono.Fonts;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Views
{
    public class InGameMenuView : BaseWindow, IInGameMenuView
    {
        public InGameMenuView() : base(FontTarget.Interface)
        {
            var xPosition = GetLabelPosition();

            Controls.Add(new GameLogoControl(xPosition, 4));

            var continueGameButton = new FramedButton(new Rectangle(xPosition - 2, 9, 20, 3))
            {
                Text = "C0nt1nue Game"
            };
            continueGameButton.Click += (sender, args) => ContinueGame?.Invoke(this, EventArgs.Empty);
            Controls.Add(continueGameButton);

            var startGameButton = new FramedButton(new Rectangle(xPosition - 2, 13, 20, 3))
            {
                Text = "Start New Game"
            };
            startGameButton.Click += (sender, args) => StartNewGame?.Invoke(this, EventArgs.Empty);
            Controls.Add(startGameButton);

            var exitToMenuButton = new FramedButton(new Rectangle(xPosition - 2, 17, 20, 3))
            {
                Text = "Ex1t t0 Menu"
            };
            exitToMenuButton.Click += (sender, args) => ExitToMenu?.Invoke(this, EventArgs.Empty);
            Controls.Add(exitToMenuButton);

            var exitButton = new FramedButton(new Rectangle(xPosition - 2, 21, 20, 3))
            {
                Text = "Ex1t"
            };
            exitButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Controls.Add(exitButton);
        }

        public event EventHandler ContinueGame;
        public event EventHandler StartNewGame;
        public event EventHandler Exit;
        public event EventHandler ExitToMenu;

        private int GetLabelPosition()
        {
            return (int)Math.Floor(Width / 2d) - 8;
        }

        public override bool ProcessKeysPressed(Keys[] keys)
        {
            if (keys.Length == 1)
            {
                switch (keys[0])
                {
                    case Keys.Escape:
                        ContinueGame?.Invoke(this, EventArgs.Empty);
                        return true;
                }
            }

            return base.ProcessKeysPressed(keys);
        }
    }
}