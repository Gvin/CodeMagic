using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class InventoryView : View
    {
        private readonly IGameCore game;

        private ListBox itemsListBox;
        private ItemDetailsControl itemDetails;

        private Button useItemButton;
        private Button equipItemButton;
        private Button takeOffItemButton;

        private Button closeButton;

        public InventoryView(IGameCore game) 
            : base(Program.Width, Program.Height)
        {
            this.game = game;

            InitializeControls();
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
            closeButton = new Button(15, 3)
            {
                Position = new Point(Width - 17, Height - 4),
                Text = "[ESC] Close",
                CanFocus = false,
                Theme = buttonsTheme
            };
            closeButton.Click += closeButton_Click;
            Add(closeButton);

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

            itemDetails = new ItemDetailsControl(57, Height - 10)
            {
                Position = new Point(Width - 58, 3)
            };
            Add(itemDetails);

            itemsListBox = new ListBox(Width - 60, Height - 6)
            {
                Position = new Point(1, 3),
                CompareByReference = true,
                CanFocus = false
            };
            var scrollBarTheme = new ScrollBarTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(DefaultForeground, DefaultBackground)
                }
            };
            var listBoxTheme = new ListBoxTheme(scrollBarTheme)
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(Color.White, DefaultBackground)
                }
            };
            itemsListBox.Theme = listBoxTheme;
            itemsListBox.SelectedItemChanged += itemsListBox_SelectedItemChanged;
            Add(itemsListBox);

            RefreshItems(false);
        }

        private void takeOffItemButton_Click(object sender, EventArgs e)
        {
            TakeOffSelectedItem();
        }

        private void TakeOffSelectedItem()
        {
            var selectedStack = (itemsListBox.SelectedItem as InventoryStackListBoxItem)?.ItemStack;

            if (!(selectedStack?.Item is IEquipableItem equipableItem))
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
            var selectedStack = (itemsListBox.SelectedItem as InventoryStackListBoxItem)?.ItemStack;

            if (!(selectedStack?.Item is IEquipableItem equipableItem))
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
            var selectedStack = (itemsListBox.SelectedItem as InventoryStackListBoxItem)?.ItemStack;

            if (!(selectedStack?.Item is IUsableItem usableItem))
                return;

            game.PerformPlayerAction(new UseItemPlayerAction(usableItem));
            Close();
        }

        private void itemsListBox_SelectedItemChanged(object sender, ListBox.SelectedItemEventArgs e)
        {
            itemDetails.Stack = (itemsListBox.SelectedItem as InventoryStackListBoxItem)?.ItemStack;

            RefreshSelectedItemButtons();
        }

        private void RefreshSelectedItemButtons()
        {
            var selectedStack = (itemsListBox.SelectedItem as InventoryStackListBoxItem)?.ItemStack;

            useItemButton.IsVisible = selectedStack?.Item is IUsableItem;
            if (selectedStack?.Item is IEquipableItem equipable)
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

        private void RefreshItems(bool keepSelection)
        {
            var selectionIndex = itemsListBox.SelectedIndex;
            itemsListBox.Items.Clear();

            foreach (var inventoryStack in game.Player.Inventory.Stacks)
            {
                itemsListBox.Items.Add(new InventoryStackListBoxItem(inventoryStack, game.Player));
            }

            if (keepSelection)
            {
                itemsListBox.SelectedItem = itemsListBox.Items[selectionIndex];
            }

            RefreshSelectedItemButtons();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Print(2, 1, "Inventory");

            Fill(1, 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GlyphBoxSingleHorizontal);
            Print(0, 2, new ColoredGlyph(Glyphs.GlyphBoxDoubleVerticalSingleRight, FrameColor, DefaultBackground));
            Print(Width - 1, 2, new ColoredGlyph(Glyphs.GlyphBoxDoubleVerticalSingleLeft, FrameColor, DefaultBackground));

            Print(Width - 59, 2, new ColoredGlyph(Glyphs.GlyphBoxSingleHorizontalDown, FrameColor, DefaultBackground));
            Print(Width - 59, Height - 1, new ColoredGlyph(Glyphs.GlyphBoxDoubleHorizontalSingleUp, FrameColor, DefaultBackground));
            DrawVerticalLine(Width - 59, 3, Height - 4, new ColoredGlyph(Glyphs.GlyphBoxSingleVertical, FrameColor, DefaultBackground));
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
                case Keys.Escape:
                    Close();
                    return true;
            }
            return base.ProcessKeyPressed(key);
        }

        private class InventoryStackListBoxItem
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
                if (!(ItemStack.Item is IEquipableItem equipable))
                    return false;

                return player.Equipment.IsEquiped(equipable);
            }

            public override string ToString()
            {
                if (ItemStack.Item.Stackable)
                {
                    return $" {ItemStack.Item.Name} ({ItemStack.Count})";
                }

                if (GetIfEquiped())
                {
                    return $" {ItemStack.Item.Name} [Equiped]";
                }

                return $" {ItemStack.Item.Name}";
            }
        }
    }
}