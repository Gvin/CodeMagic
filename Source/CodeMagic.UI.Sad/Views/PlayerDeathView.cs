using System;
using CodeMagic.UI.Sad.GameProcess;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Views
{
    public class PlayerDeathView : View
    {
        private Button startNewGameButton;
        private Button backToMenuButton;
        private Button exitGameButton;

        public PlayerDeathView() 
            : base(Program.Width, Program.Height)
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            var buttonsTheme = new ButtonLinesTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(DefaultForeground, DefaultBackground)
                }
            };

            var buttonsX = Width / 2 - 10;

            startNewGameButton = new Button(20, 3)
            {
                Position = new Point(buttonsX, 16),
                Theme = buttonsTheme,
                CanFocus = false,
                Text = "Start New Game"
            };
            startNewGameButton.Click += startNewGameButton_Click;
            Add(startNewGameButton);

            backToMenuButton = new Button(20, 3)
            {
                Position = new Point(buttonsX, 20),
                Theme = buttonsTheme,
                CanFocus = false,
                Text = "Back t0 Menu"
            };
            backToMenuButton.Click += backToMenuButton_Click;
            Add(backToMenuButton);

            exitGameButton = new Button(20, 3)
            {
                Position = new Point(buttonsX, 24),
                Theme = buttonsTheme,
                CanFocus = false,
                Text = "Ex1t Game"
            };
            exitGameButton.Click += exitGameButton_Click;
            Add(exitGameButton);
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            var labelX = Width / 2 - 7;
            Print(labelX, 12, new ColoredString("You have died!", Color.Red, DefaultBackground));
        }

        private void startNewGameButton_Click(object sender, EventArgs e)
        {
            Close();

            var game = new GameManager().StartGame();
            var gameView = new GameView(game);
            gameView.Show();
        }

        private void backToMenuButton_Click(object sender, EventArgs e)
        {
            Close();

            new MainMenuView().Show();
        }

        private void exitGameButton_Click(object sender, EventArgs e)
        {
            SadConsole.Game.Instance.Exit();
        }
    }
}