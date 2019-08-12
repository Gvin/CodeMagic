using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class CustomInventoryView : InventoryViewBase
    {
        private readonly IGameCore game;
        private bool actionPerformed;

        private Button pickUpStackButton;
        private Button pickUpOneItemButton;
        private Button pickUpAllButton;

        public CustomInventoryView(IGameCore game, string inventoryName, Inventory inventory) 
            : base(inventoryName, inventory, game.Player)
        {
            this.game = game;
            actionPerformed = false;

            InitializeControls();

            RefreshItems(false);
        }

        private void InitializeControls()
        {
            var buttonsTheme = new ButtonLinesTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(Color.White, DefaultBackground)
                }
            };

            pickUpOneItemButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 40),
                Text = "[O] Pick Up One",
                CanFocus = false,
                Theme = buttonsTheme
            };
            pickUpOneItemButton.Click += pickUpOneItemButton_Click;
            Add(pickUpOneItemButton);

            pickUpStackButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 43),
                Text = "[P] Pick Up",
                Theme = buttonsTheme,
                CanFocus = false
            };
            pickUpStackButton.Click += pickUpStackButton_Click;
            Add(pickUpStackButton);

            pickUpAllButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 46),
                Text = "[A] Pick Up All",
                Theme = buttonsTheme,
                CanFocus = false
            };
            pickUpAllButton.Click += pickUpAllButton_Click;
            Add(pickUpAllButton);
        }

        private void pickUpAllButton_Click(object sender, System.EventArgs e)
        {
            PickUpAll();
        }

        private void pickUpStackButton_Click(object sender, System.EventArgs e)
        {
            PickUpStack();
        }

        private void pickUpOneItemButton_Click(object sender, System.EventArgs e)
        {
            PickUpOneItem();
        }

        private void PickUpOneItem()
        {
            if (SelectedStack == null)
                return;

            actionPerformed = true;
            var item = SelectedStack.TopItem;
            Inventory.RemoveItem(item);
            game.Player.Inventory.AddItem(item);

            if (Inventory.ItemsCount > 0)
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
                Inventory.RemoveItem(item);
                game.Player.Inventory.AddItem(item);
            }

            if (Inventory.ItemsCount > 0)
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
            foreach (var stack in Inventory.Stacks)
            {
                var items = stack.Items.ToArray();
                foreach (var item in items)
                {
                    Inventory.RemoveItem(item);
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
                case Keys.Escape:
                    Close();
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }

        protected override IInventoryStackListBoxItem CreateListBoxItem(InventoryStack stack)
        {
            return new InventoryStackListBoxItem(stack);
        }

        protected override void OnClosed(System.Windows.Forms.DialogResult? result)
        {
            base.OnClosed(result);

            if (actionPerformed)
            {
                game.PerformPlayerAction(new EmptyPlayerAction());
            }
        }

        private class InventoryStackListBoxItem : IInventoryStackListBoxItem
        {
            public InventoryStackListBoxItem(InventoryStack itemStack)
            {
                ItemStack = itemStack;
            }

            public InventoryStack ItemStack { get; }

            public override string ToString()
            {
                if (ItemStack.TopItem.Stackable)
                {
                    return $" {ItemStack.TopItem.Name} ({ItemStack.Count})";
                }

                return $" {ItemStack.TopItem.Name}";
            }
        }
    }
}