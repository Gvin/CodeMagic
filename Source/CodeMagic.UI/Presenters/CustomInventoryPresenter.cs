using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.PlayerActions;

namespace CodeMagic.UI.Presenters
{
    public interface ICustomInventoryView : IView
    {
        event EventHandler PickUpOne;
        event EventHandler PickUpStack;
        event EventHandler PickUpAll;
        event EventHandler Exit;

        string InventoryName { set; }

        InventoryStack SelectedStack { get; }

        InventoryStack[] Stacks { set; }

        Player Player { set; }

        void Initialize();

        void RefreshItems(bool keepSelection);
    }

    public class CustomInventoryPresenter : IPresenter
    {
        private readonly ICustomInventoryView view;
        private GameCore<Player> game;
        private Inventory inventory;
        private bool actionPerformed;

        public CustomInventoryPresenter(ICustomInventoryView view)
        {
            this.view = view;
            actionPerformed = false;

            this.view.Exit += View_Exit;
            this.view.PickUpOne += View_PickUpOne;
            this.view.PickUpStack += View_PickUpStack;
            this.view.PickUpAll += View_PickUpAll;
        }

        private void View_PickUpAll(object sender, EventArgs e)
        {
            actionPerformed = true;
            foreach (var stack in inventory.Stacks)
            {
                var items = stack.Items.ToArray();
                foreach (var item in items)
                {
                    inventory.RemoveItem(item);
                    game.Player.Inventory.AddItem(item);
                }
            }
            CloseView();
        }

        private void View_PickUpStack(object sender, EventArgs e)
        {
            if (view.SelectedStack == null)
                return;

            actionPerformed = true;
            var items = view.SelectedStack.Items.ToArray();
            foreach (var item in items)
            {
                inventory.RemoveItem(item);
                game.Player.Inventory.AddItem(item);
            }

            if (inventory.ItemsCount > 0)
            {
                view.Stacks = inventory.Stacks;
                view.RefreshItems(false);
            }
            else
            {
                CloseView();
            }
        }

        private void View_PickUpOne(object sender, EventArgs e)
        {
            if (view.SelectedStack == null)
                return;

            actionPerformed = true;
            var item = view.SelectedStack.TopItem;
            inventory.RemoveItem(item);
            game.Player.Inventory.AddItem(item);

            if (inventory.ItemsCount > 0)
            {
                view.Stacks = inventory.Stacks;
                view.RefreshItems(true);
            }
            else
            {
                CloseView();
            }
        }

        private void CloseView()
        {
            if (actionPerformed)
            {
                game.PerformPlayerAction(new EmptyPlayerAction());
            }
            view.Close();
        }

        private void View_Exit(object sender, EventArgs e)
        {
            CloseView();
        }

        public void Run(GameCore<Player> currentGame, string inventoryName, Inventory customInventory)
        {
            game = currentGame;
            inventory = customInventory;
            view.InventoryName = inventoryName;
            view.Stacks = inventory.Stacks;
            view.Player = game.Player;

            view.Initialize();
            view.Show();
        }
    }
}