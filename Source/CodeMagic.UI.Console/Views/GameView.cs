using System;
using System.Drawing;
using CodeMagic.Core.Game;
using CodeMagic.UI.Console.Controls;
using CodeMagic.UI.Console.Drawing;
using CodeMagic.UI.Console.Drawing.Writing;

namespace CodeMagic.UI.Console.Views
{
    public class GameView : View
    {
        private static readonly Color FrameColor = Color.Gray;

        private readonly GameCore game;

        private PlayerStatusPanel playerStatusPanel;
        private JournalPanel journalPanel;
        private GameAreaControl gameArea;

        public GameView(GameCore game)
        {
            this.game = game;

            Initialize();
        }

        private void Initialize()
        {
            playerStatusPanel = new PlayerStatusPanel(game.Player, FrameColor)
            {
                X = Writer.ScreenWidth - 47,
                Y = 0,
                Width = 46,
                Height = Writer.ScreenHeight - 8
            };
            Controls.Add(playerStatusPanel);

            journalPanel = new JournalPanel(game.Journal, FrameColor)
            {
                X = 0,
                Y = Writer.ScreenHeight - 8,
                Width = Writer.ScreenWidth,
                Height = 7
            };
            Controls.Add(journalPanel);

            gameArea = new GameAreaControl(game)
            {
                X = 1,
                Y = 1,
                Width = Writer.ScreenWidth - 46,
                Height = Writer.ScreenHeight - 8
            };
            Controls.Add(gameArea);
        }

        public override void DrawStatic()
        {
            base.DrawStatic();

            var x = playerStatusPanel.X;
            var y = journalPanel.Y;
            Writer.WriteAt(x, y, LineTypes.SingleHorizontalUp, FrameColor, Color.Black);
        }

        public override void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            base.ProcessKey(keyInfo);

            switch (keyInfo.Key)
            {
                case ConsoleKey.C:
                    OpenSpellBook();
                    break;
                case ConsoleKey.F2:
                    game.Player.Health += 10;
                    break;
                case ConsoleKey.F3:
                    game.Player.Mana += 100;
                    break;
                case ConsoleKey.Escape:
                    var menu = new MenuView();
                    menu.Show();
                    Close();
                    break;
            }
        }

        private void OpenSpellBook()
        {
            if (game.Player.Equipment.SpellBook == null)
                return;

            var spellBookView = new SpellBookView(game);
            spellBookView.Show();
        }
    }
}