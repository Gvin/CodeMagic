using System;
using CodeMagic.UI.Mono.Controls;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Mono.Fonts;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Views
{
    public class MainMenuView : BaseWindow, IMainMenuView
    {
        private readonly FramedButton continueGameButton;

        public MainMenuView() 
            : base(FontTarget.Interface)
        {
            const int buttonWidth = 20;
            var xPosition = GetLabelPosition();
            var buttonX = xPosition - 2;

            Controls.Add(new GameLogoControl(xPosition, 4));

            var startGameButton = new FramedButton(new Rectangle(buttonX, 9, buttonWidth, 3))
            {
                Text = "Start Game"
            };
            startGameButton.Click += (sender, args) => StartGame?.Invoke(this, EventArgs.Empty);
            Controls.Add(startGameButton);

            continueGameButton = new FramedButton(new Rectangle(buttonX, 13, buttonWidth, 3))
            {
                Text = "C0nt1nue Game"
            };
            continueGameButton.Click += (sender, args) => ContinueGame?.Invoke(this, EventArgs.Empty);
            Controls.Add(continueGameButton);

            var spellsLibraryButton = new FramedButton(new Rectangle(buttonX, 17, buttonWidth, 3))
            {
                Text = "Spells L1brary"
            };
            spellsLibraryButton.Click += (sender, args) => ShowSpellLibrary?.Invoke(this, EventArgs.Empty);
            Controls.Add(spellsLibraryButton);

            var settingsButton = new FramedButton(new Rectangle(buttonX, 21, buttonWidth, 3))
            {
                Text = "Sett1ngs"
            };
            settingsButton.Click += (sender, args) => ShowSettings?.Invoke(this, EventArgs.Empty);
            Controls.Add(settingsButton);

            var exitButton = new FramedButton(new Rectangle(buttonX, 25, buttonWidth, 3))
            {
                Text = "Ex1t"
            };
            exitButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Controls.Add(exitButton);
        }

        public event EventHandler StartGame;
        public event EventHandler ContinueGame;
        public event EventHandler ShowSpellLibrary;
        public event EventHandler ShowSettings;
        public event EventHandler Exit;

        public void SetContinueOptionState(bool canContinue)
        {
            continueGameButton.Enabled = canContinue;
        }

        private int GetLabelPosition()
        {
            return (int)Math.Floor(Width / 2d) - 8;
        }
    }
}