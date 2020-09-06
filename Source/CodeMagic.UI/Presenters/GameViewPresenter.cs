using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.UI.Presenters
{
    public interface IGameView : IView
    {
        event EventHandler OpenInGameMenu;
        event EventHandler OpenSpellBook;
        event EventHandler OpenInventory;
        event EventHandler OpenPlayerStats;
        event EventHandler OpenGroundView;
        event EventHandler OpenCheats;

        bool SpellBookEnabled { set; }
        bool OpenGroundEnabled { set; }

        GameCore<Player> Game { set; }

        void Initialize();
    }

    public class GameViewPresenter : IPresenter
    {
        private readonly IGameView view;
        private readonly IApplicationController controller;
        private GameCore<Player> game;

        public GameViewPresenter(
            IGameView view, 
            IApplicationController controller)
        {
            this.view = view;
            this.controller = controller;

            this.view.OpenInGameMenu += View_OpenInGameMenu;
            this.view.OpenPlayerStats += View_OpenPlayerStats;
            this.view.OpenInventory += View_OpenInventory;
            this.view.OpenSpellBook += View_OpenSpellBook;
            this.view.OpenGroundView += View_OpenGroundView;
            this.view.OpenCheats += View_OpenCheats;
        }

        private void Game_MapUpdated(object sender, EventArgs e)
        {
            view.SpellBookEnabled = GetSpellBookEnabled();
            view.OpenGroundEnabled = GetOpenGroundEnabled();
        }

        private bool GetSpellBookEnabled()
        {
            return game.Player.Equipment.SpellBook != null;
        }

        private bool GetOpenGroundEnabled()
        {
            return game.Map.GetCell(game.PlayerPosition).Objects.OfType<IItem>().Any();
        }

        private void View_OpenCheats(object sender, EventArgs e)
        {
            controller.CreatePresenter<CheatsPresenter>().Run();
        }

        private void View_OpenGroundView(object sender, EventArgs e)
        {
            var cell = game.Map.GetCell(game.PlayerPosition);
            var itemsOnFloor = cell.Objects.OfType<IItem>().ToArray();
            if (itemsOnFloor.Length == 0)
                return;

            var floorInventory = new Inventory(itemsOnFloor);
            floorInventory.ItemRemoved += (s, a) => game.Map.RemoveObject(game.PlayerPosition, a.Item);

            controller.CreatePresenter<CustomInventoryPresenter>().Run(game, "Items on Floor", floorInventory);
        }

        private void View_OpenSpellBook(object sender, EventArgs e)
        {
            if (game.Player.Equipment.SpellBook == null)
                return;

            controller.CreatePresenter<SpellBookPresenter>().Run(game);
        }

        private void View_OpenInventory(object sender, EventArgs e)
        {
            controller.CreatePresenter<PlayerInventoryPresenter>().Run(game);
        }

        private void View_OpenPlayerStats(object sender, EventArgs e)
        {
            controller.CreatePresenter<PlayerStatsPresenter>().Run(game.Player);
        }

        private void View_OpenInGameMenu(object sender, EventArgs e)
        {
            CloseView();

            controller.CreatePresenter<InGameMenuPresenter>().Run(game);
        }

        private void Player_LeveledUp(object sender, EventArgs e)
        {
            controller.CreatePresenter<LevelUpPresenter>().Run(game.Player);
        }

        private void Player_Died(object sender, EventArgs e)
        {
            CloseView();

            controller.CreatePresenter<PlayerDeathPresenter>().Run();
        }

        public void Run(GameCore<Player> currentGame)
        {
            game = currentGame;
            view.Game = game;

            game.Player.Died += Player_Died;
            game.Player.LeveledUp += Player_LeveledUp;

            game.MapUpdated += Game_MapUpdated;

            view.Initialize();

            view.SpellBookEnabled = GetSpellBookEnabled();
            view.OpenGroundEnabled = GetOpenGroundEnabled();

            view.Show();
        }

        private void CloseView()
        {
            game.Player.Died -= Player_Died;
            game.Player.LeveledUp -= Player_LeveledUp;

            game.MapUpdated -= Game_MapUpdated;

            view.Close();
        }
    }
}