using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.PlayerActions;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Input;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class CustomInventoryView : InventoryViewBase
    {
        private readonly IGameCore game;
        private readonly Inventory inventory;
        private bool actionPerformed;

        private StandardButton pickUpStackButton;
        private StandardButton pickUpOneItemButton;
        private StandardButton pickUpAllButton;

        public CustomInventoryView(GameCore<Player> game, string inventoryName, Inventory inventory) 
            : base(inventoryName, game.Player)
        {
            this.game = game;
            this.inventory = inventory;
            actionPerformed = false;

            InitializeControls();

            RefreshItems(false);
        }

        protected override IEnumerable<InventoryStack> GetStacks()
        {
            return inventory.Stacks;
        }

        private void InitializeControls()
        {
            pickUpOneItemButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 40),
                Text = "[O] Pick Up One"
            };
            pickUpOneItemButton.Click += pickUpOneItemButton_Click;
            Add(pickUpOneItemButton);

            pickUpStackButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 43),
                Text = "[P] Pick Up"
            };
            pickUpStackButton.Click += pickUpStackButton_Click;
            Add(pickUpStackButton);

            pickUpAllButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 46),
                Text = "[A] Pick Up All"
            };
            pickUpAllButton.Click += pickUpAllButton_Click;
            Add(pickUpAllButton);
        }

        private void pickUpAllButton_Click(object sender, EventArgs e)
        {
            PickUpAll();
        }

        private void pickUpStackButton_Click(object sender, EventArgs e)
        {
            PickUpStack();
        }

        private void pickUpOneItemButton_Click(object sender, EventArgs e)
        {
            PickUpOneItem();
        }

        private void PickUpOneItem()
        {
            if (SelectedStack == null)
                return;

            actionPerformed = true;
            var item = SelectedStack.TopItem;
            inventory.RemoveItem(item);
            game.Player.Inventory.AddItem(item);

            if (inventory.ItemsCount > 0)
            {
                RefreshItems(true);
            }
            else
            {
                Close();
            }
        }

        private void PickUpStack()
        {
            if (SelectedStack == null)
                return;

            actionPerformed = true;
            var items = SelectedStack.Items.ToArray();
            foreach (var item in items)
            {
                inventory.RemoveItem(item);
                game.Player.Inventory.AddItem(item);
            }

            if (inventory.ItemsCount > 0)
            {
                RefreshItems(false);
            }
            else
            {
                Close();
            }
        }

        private void PickUpAll()
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
            Close();
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            UpdateButtonsState();
        }

        private void UpdateButtonsState()
        {
            pickUpOneItemButton.IsVisible = SelectedStack?.TopItem != null && SelectedStack.TopItem.Stackable;
            pickUpStackButton.IsVisible = SelectedStack != null;
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.A:
                    PickUpAll();
                    return true;
                case Keys.P:
                    PickUpStack();
                    return true;
                case Keys.O:
                    PickUpOneItem();
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }

        protected override InventoryStackItem CreateListBoxItem(InventoryStack stack)
        {
            return new CustomInventoryItem(stack);
        }

        protected override void OnClosed(DialogResult result)
        {
            base.OnClosed(result);

            if (actionPerformed)
            {
                game.PerformPlayerAction(new EmptyPlayerAction());
            }
        }
    }

    public class CustomInventoryItem : InventoryStackItem
    {
        public CustomInventoryItem(InventoryStack itemStack)
            : base(itemStack)
        {
        }

        protected override ColoredString[] GetAfterNameText(Color backColor)
        {
            if (Stack.TopItem.Stackable)
            {
                return new[]
                {
                    new ColoredString($" ({Stack.Count})", new Cell(StackCountColor, backColor))
                };
            }

            return new ColoredString[0];
        }
    }
}