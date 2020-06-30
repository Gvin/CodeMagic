using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.GameProcess;
using Point = SadRogue.Primitives.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class MainMenuView : GameViewBase
    {
        private GameLogoControl gameLabel;
        private StandardButton startGameButton;
        private StandardButton continueGameButton;
        private StandardButton spellsLibraryButton;
        private StandardButton exitButton;
        private StandardButton settingsButton;

        public MainMenuView()
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
            ControlHostComponent.Add(gameLabel);

            startGameButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 9),
                Text = "Start Game"
            };
            startGameButton.Click += startGameButton_Click;
            ControlHostComponent.Add(startGameButton);

            continueGameButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 13),
                Text = "C0nt1nue Game"
            };
            if (CurrentGame.Game == null)
            {
                continueGameButton.IsEnabled = false;
            }
            else
            {
                continueGameButton.Click += continueGameButton_Click;
            }
            ControlHostComponent.Add(continueGameButton);

            spellsLibraryButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 17),
                Text = "Spells L1brary"
            };
            spellsLibraryButton.Click += (sender, args) => OpenSpellsLibrary();
            ControlHostComponent.Add(spellsLibraryButton);

            settingsButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 21),
                Text = "Sett1ngs"
            };
            settingsButton.Click += (sender, args) => OpenSettingsDialog();
            ControlHostComponent.Add(settingsButton);

            exitButton = new StandardButton(20)
            {
                Position = new Point(xPosition - 2, 25),
                Text = "Ex1t"
            };
            exitButton.Click += exitButton_Click;
            ControlHostComponent.Add(exitButton);
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
            Program.Exit();
        }

        private void continueGameButton_Click(object sender, EventArgs args)
        {
            if (CurrentGame.Game == null)
                return;

            Hide();

            var game = (GameCore<Player>) CurrentGame.Game;
            var gameView = new GameView(game);
            gameView.Show();
        }

        private void startGameButton_Click(object sender, EventArgs args)
        {
            Hide();

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