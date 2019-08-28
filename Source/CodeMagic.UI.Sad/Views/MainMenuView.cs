using System;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.GameProcess;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;
using Game = SadConsole.Game;

namespace CodeMagic.UI.Sad.Views
{
    public class MainMenuView : View
    {
        private GameLogoControl gameLabel;
        private Button startGameButton;
        private Button spellsLibraryButton;
        private Button exitButton;

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

            var menuButtonsTheme = new ButtonLinesTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(DefaultForeground, DefaultBackground)
                }
            };

            startGameButton = new Button(20, 3)
            {
                Position = new Point(xPosition - 2, 9),
                Text = "Start Game",
                Theme = menuButtonsTheme,
                CanFocus = false
            };
            startGameButton.Click += startGameButton_Click;
            Add(startGameButton);

            spellsLibraryButton = new Button(20,3)
            {
                Position = new Point(xPosition - 2, 13),
                Text = "Spells L1brary",
                Theme = menuButtonsTheme,
                CanFocus = false
            };
            spellsLibraryButton.Click += (sender, args) => OpenSpellsLibrary();
            Add(spellsLibraryButton);

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