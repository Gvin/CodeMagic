using System;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.GameProcess;
using SadConsole;
using SadRogue.Primitives;

namespace CodeMagic.UI.Sad.Views
{
    public class PlayerDeathView : GameViewBase
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
            startNewGameButton.Click += startNewGameButton_Click;
            ControlHostComponent.Add(startNewGameButton);

            backToMenuButton = new StandardButton(20)
            {
                Position = new Point(buttonsX, 20),
                Text = "Back t0 Menu"
            };
            backToMenuButton.Click += backToMenuButton_Click;
            ControlHostComponent.Add(backToMenuButton);

            exitGameButton = new StandardButton(20)
            {
                Position = new Point(buttonsX, 24),
                Text = "Ex1t Game"
            };
            exitGameButton.Click += exitGameButton_Click;
            ControlHostComponent.Add(exitGameButton);
        }

        protected override void DrawView(ICellSurface surface)
        {
            base.DrawView(surface);

            var labelX = Width / 2 - 7;
            surface.Print(labelX, 12, new ColoredString("You have died!", Color.Red, DefaultBackground));
        }

        private void startNewGameButton_Click(object sender, EventArgs e)
        {
            Hide();

            var game = GameManager.Current.StartGame();
            var gameView = new GameView(game);
            gameView.Show();
        }

        private void backToMenuButton_Click(object sender, EventArgs e)
        {
            Hide();

            new MainMenuView().Show();
        }

        private void exitGameButton_Click(object sender, EventArgs e)
        {
            Program.Exit();
        }
    }
}