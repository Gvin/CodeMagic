using System;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.GameProcess;
using Microsoft.Xna.Framework;
using Game = SadConsole.Game;

namespace CodeMagic.UI.Sad.Views
{
    public class MainMenuView : View
    {
        private GameLogoControl gameLabel;
        private StandardButton startGameButton;
        private StandardButton spellsLibraryButton;
        private StandardButton exitButton;
        private StandardButton settingsButton;

        public MainMenuView() 
            : base(Program.Width, Program.Height)
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

            startGameButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 9),
                Text = "Start Game"
            };
            startGameButton.Click += startGameButton_Click;
            Add(startGameButton);

            spellsLibraryButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 13),
                Text = "Spells L1brary"
            };
            spellsLibraryButton.Click += (sender, args) => OpenSpellsLibrary();
            Add(spellsLibraryButton);

            settingsButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 17),
                Text = "Sett1ngs"
            };
            settingsButton.Click += (sender, args) => OpenSettingsDialog();
            Add(settingsButton);

            exitButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 21),
                Text = "Ex1t"
            };
            exitButton.Click += exitButton_Click;
            Add(exitButton);
        }

        private void OpenSettingsDialog()
        {
            new SettingsView().Show();
        }

        private void OpenSpellsLibrary()
        {
            new MainSpellsLibraryView().Show();
        }

        private void exitButton_Click(object sender, EventArgs args)
        {
            Game.Instance.Exit();
        }

        private void startGameButton_Click(object sender, EventArgs args)
        {
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