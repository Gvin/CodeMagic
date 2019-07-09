using System;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.UI.Console.Drawing;
using CodeMagic.UI.Console.Drawing.DrawingProcessors;

namespace CodeMagic.UI.Console.Views
{
    public class GameView : View
    {
        private readonly GameDrawer gameDrawer;
        private readonly GameCore game;

        public GameView(GameCore game)
        {
            this.game = game;

            var drawer = new ConsoleDrawer();
            var factory = new DrawingProcessorsFactory();
            var floorColorFactory = new FloorColorFactory();
            gameDrawer = new GameDrawer(factory, drawer, floorColorFactory);
        }

        public override void DrawStatic()
        {
            base.DrawStatic();
            gameDrawer.DrawStaticElements();
        }

        public override void DrawDynamic()
        {
            base.DrawDynamic();
            gameDrawer.DrawGame(game);
        }

        public override void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.C:
                    OpenSpellBook();
                    break;
                case ConsoleKey.F1:
                    game.Player.Health += 10;
                    break;
                default:
                    ActivateKeyAssignedPlayerAction(keyInfo.Key);
                    break;
            }  
        }

        private void ActivateKeyAssignedPlayerAction(ConsoleKey key)
        {
            var action = GetPlayerAction(key);
            if (action != null)
            {
                game.PerformPlayerAction(action);
            }
        }

        private void OpenSpellBook()
        {
            if (game.Player.Equipment.SpellBook == null)
                return;

            var spellBookView = new SpellBookView(game);
            spellBookView.Show();
        }

        private IPlayerAction GetPlayerAction(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    return new MovePlayerAction(Direction.Up);
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    return new MovePlayerAction(Direction.Down);
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    return new MovePlayerAction(Direction.Left);
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    return new MovePlayerAction(Direction.Right);
                case ConsoleKey.Spacebar:
                    return new EmptyPlayerAction();
                case ConsoleKey.F:
                    return new MeleAttackPlayerAction();
            }

            return null;
        }
    }
}