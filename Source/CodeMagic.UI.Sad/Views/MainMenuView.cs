using System;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class MainMenuView : GameViewBase, IMainMenuView
    {
        private GameLogoControl gameLabel;
        private StandardButton startGameButton;
        private StandardButton continueGameButton;
        private StandardButton spellsLibraryButton;
        private StandardButton exitButton;
        private StandardButton settingsButton;

        public event EventHandler StartGame;
        public event EventHandler ContinueGame;
        public event EventHandler ShowSpellLibrary;
        public event EventHandler ShowSettings;
        public event EventHandler Exit;

        public MainMenuView()
        {
            InitializeControls();
        }

        public void SetContinueOptionState(bool canContinue)
        {
            continueGameButton.IsEnabled = canContinue;
        }

        private void InitializeControls()
        {
            var xPosition = GetLabelPosition();

            gameLabel = new GameLogoControl
            {
                Position = new Point(xPosition, 4)
            };
            Add(gameLabel);

            startGameButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 9),
                Text = "Start Game"
            };
            startGameButton.Click += (sender, args) => StartGame?.Invoke(this, EventArgs.Empty);
            Add(startGameButton);

            continueGameButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 13),
                Text = "C0nt1nue Game"
            };
            continueGameButton.Click += (sender, args) => ContinueGame?.Invoke(this, EventArgs.Empty);
            Add(continueGameButton);

            spellsLibraryButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 17),
                Text = "Spells L1brary"
            };
            spellsLibraryButton.Click += (sender, args) => ShowSpellLibrary?.Invoke(this, EventArgs.Empty);
            Add(spellsLibraryButton);

            settingsButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 21),
                Text = "Sett1ngs"
            };
            settingsButton.Click += (sender, args) => ShowSettings?.Invoke(this, EventArgs.Empty);
            Add(settingsButton);

            exitButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 25),
                Text = "Ex1t"
            };
            exitButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Add(exitButton);
        }

        private int GetLabelPosition()
        {
            return (int)Math.Floor(Width / 2d) - 8;
        }

        public void Close()
        {
            Close(DialogResult.None);
        }
    }
}