using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class PlayerInventoryView : InventoryViewBase
    {
        private readonly IGameCore game;

        private Button useItemButton;
        private Button equipItemButton;
        private Button takeOffItemButton;
        private Button dropItemButton;
        private Button dropAllItemsButton;

        public PlayerInventoryView(IGameCore game) 
            : base("Player Inventory", game.Player.Inventory)
        {
            this.game = game;

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

            useItemButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 40),
                Text = "[U] Use",
                CanFocus = false,
                Theme = buttonsTheme
            };
            useItemButton.Click += useItemButton_Click;
            Add(useItemButton);

            equipItemButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 40),
                Text = "[E] Equip",
                CanFocus = false,
                Theme = buttonsTheme
            };
            equipItemButton.Click += equipItemButton_Click;
            Add(equipItemButton);

            takeOffItemButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 40),
                Text = "[T] Take Off",
                CanFocus = false,
                Theme = buttonsTheme
            };
            takeOffItemButton.Click += takeOffItemButton_Click;
            Add(takeOffItemButton);

            dropItemButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 43),
                Text = "[D] Drop",
                CanFocus = false,
                Theme = buttonsTheme
            };
            dropItemButton.Click += dropItemButton_Click;
            Add(dropItemButton);

            dropAllItemsButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 46),
                Text = "[A] Drop All",
                CanFocus = false,
                Theme = buttonsTheme
            };
            dropAllItemsButton.Click += dropAllItemsButton_Click;
            Add(dropAllItemsButton);
        }

        private void dropAllItemsButton_Click(object sender, EventArgs e)
        {
            DropAllItemsInStack();
        }

        private void DropAllItemsInStack()
        {
            if (SelectedStack == null)
                return;

            game.PerformPlayerAction(new DropItemsPlayerAction(SelectedStack.Items));
            Close();
        }

        private void dropItemButton_Click(object sender, EventArgs e)
        {
            DropSelectedItem();
        }

        private void DropSelectedItem()
        {
            if (SelectedStack == null)
                return;

            game.PerformPlayerAction(new DropItemsPlayerAction(SelectedStack.TopItem));
            Close();
        }

        private void takeOffItemButton_Click(object sender, EventArgs e)
        {
            TakeOffSelectedItem();
        }

        private void TakeOffSelectedItem()
        {
            if (!(SelectedStack?.TopItem is IEquipableItem equipableItem))
                return;

            if (!game.Player.Equipment.IsEquiped(equipableItem))
                return;

            game.PerformPlayerAction(new UnequipItemPlayerAction(equipableItem));
            Close();
        }

        private void equipItemButton_Click(object sender, EventArgs e)
        {
            EquipSelectedItem();
        }

        private void EquipSelectedItem()
        {
            if (!(SelectedStack?.TopItem is IEquipableItem equipableItem))
                return;

            if (game.Player.Equipment.IsEquiped(equipableItem))
                return;

            game.PerformPlayerAction(new EquipItemPlayerAction(equipableItem));
            Close();
        }

        private void useItemButton_Click(object sender, EventArgs e)
        {
            UseSelectedItem();
        }

        private void UseSelectedItem()
        {
            if (!(SelectedStack?.TopItem is IUsableItem usableItem))
                return;

            game.PerformPlayerAction(new UseItemPlayerAction(usableItem));
            Close();
        }

        protected override void ProcessSelectedItemChanged()
        {
            base.ProcessSelectedItemChanged();

            RefreshSelectedItemButtons();
        }

        private void RefreshSelectedItemButtons()
        {
            dropItemButton.IsVisible = SelectedStack != null;
            dropAllItemsButton.IsVisible = SelectedStack != null;

            useItemButton.IsVisible = SelectedStack?.TopItem is IUsableItem;
            if (SelectedStack?.TopItem is IEquipableItem equipable)
            {
                var equiped = game.Player.Equipment.IsEquiped(equipable);
                takeOffItemButton.IsVisible = equiped;
                equipItemButton.IsVisible = !equiped;
            }
            else
            {
                takeOffItemButton.IsVisible = false;
                equipItemButton.IsVisible = false;
            }
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.U:
                    UseSelectedItem();
                    return true;
                case Keys.E:
                    EquipSelectedItem();
                    return true;
                case Keys.T:
                    TakeOffSelectedItem();
                    return true;
                case Keys.D:
                    DropSelectedItem();
                    return true;
                case Keys.A:
                    DropAllItemsInStack();
                    return true;
                case Keys.Escape:
                    Close();
                    return true;
            }
            return base.ProcessKeyPressed(key);
        }

        protected override IInventoryStackListBoxItem CreateListBoxItem(InventoryStack stack)
        {
            return new InventoryStackListBoxItem(stack, game.Player);
        }

        private class InventoryStackListBoxItem : IInventoryStackListBoxItem
        {
            private readonly IPlayer player;

            public InventoryStackListBoxItem(InventoryStack itemStack, IPlayer player)
            {
                ItemStack = itemStack;
                this.player = player;
            }

            public InventoryStack ItemStack { get; }

            private bool GetIfEquiped()
            {
                if (!(ItemStack.TopItem is IEquipableItem equipable))
                    return false;

                return player.Equipment.IsEquiped(equipable);
            }

            public override string ToString()
            {
                if (ItemStack.TopItem.Stackable)
                {
                    return $" {ItemStack.TopItem.Name} ({ItemStack.Count})";
                }

                if (GetIfEquiped())
                {
                    return $" {ItemStack.TopItem.Name} [Equiped]";
                }

                return $" {ItemStack.TopItem.Name}";
            }
        }
    }
}