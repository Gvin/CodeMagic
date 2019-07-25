using System;
using CodeMagic.UI.Sad.Common;
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
        private DrawingSurface gameLabel;
        private Button startGameButton;
        private Button exitButton;

        public MainMenuView() 
            : base(Program.Width, Program.Height)
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            var xPosition = GetLabelPosition();

            gameLabel = new DrawingSurface(16, 2)
            {
                OnDraw = DrawGameLabel,
                Position = new Point(xPosition, 4),
                Theme = new DrawingSurfaceTheme
                {
                    Colors = new Colors { Appearance_ControlNormal = new Cell(DefaultForeground, DefaultBackground) }
                },
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

            exitButton = new Button(20, 3)
            {
                Position = new Point(xPosition - 2, 13),
                Text = "Ex1t",
                Theme = menuButtonsTheme,
                CanFocus = false
            };
            exitButton.Click += exitButton_Click;
            Add(exitButton);
        }

        private void DrawGameLabel(DrawingSurface surface)
        {
            surface.Surface.Print(0, 0, "<- C0de Mag1c ->", Color.BlueViolet);

            surface.Surface.Print(4, +1,
                new ColoredGlyph(Glyphs.GlyphBoxSingleVertical, Color.Red,
                    DefaultBackground));
            surface.Surface.Print(6, 1,
                new ColoredGlyph('\'', Color.Red,
                    DefaultBackground));
            surface.Surface.Print(10, 1,
                new ColoredGlyph(Glyphs.GlyphBoxSingleVertical, Color.Red,
                    DefaultBackground));
            surface.Surface.Print(12, 1,
                new ColoredGlyph('`', Color.Red,
                    DefaultBackground));
        }

        private void exitButton_Click(object sender, EventArgs args)
        {
            Game.Instance.Exit();
        }

        private void startGameButton_Click(object sender, EventArgs args)
        {
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