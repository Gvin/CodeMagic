using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Views
{
    public class StorageInventoryView : View
    {
        private readonly GameCore<Player> game;
        private CustomListBox<InventoryStackItem> itemsList1;
        private CustomListBox<InventoryStackItem> itemsList2;
        private ItemDetailsControl itemDetails;
        private Button closeButton;
        private readonly string inventoryName;
        private readonly Inventory storage;
        private readonly int? maxWeight;
        private readonly IItem inventoryItem;

        private StandardButton moveStackUpButton;
        private StandardButton moveStackDownButton;
        private StandardButton moveItemUpButton;
        private StandardButton moveItemDownButton;

        public StorageInventoryView(GameCore<Player> game, string inventoryName, Inventory storage, int? maxWeight, IItem inventoryItem)
        {
            this.game = game;
            this.inventoryName = inventoryName;
            this.storage = storage;
            this.maxWeight = maxWeight;
            this.inventoryItem = inventoryItem;

            InitializeControls(game.Player);

            RefreshAllItems(false);
        }

        protected InventoryStack SelectedStack1 => itemsList1.SelectedItem?.Stack;

        protected InventoryStack SelectedStack2 => itemsList2.SelectedItem?.Stack;

        private void InitializeControls(Player player)
        {
            closeButton = new StandardButton(15)
            {
                Position = new Microsoft.Xna.Framework.Point(Width - 17, Height - 4),
                Text = "[ESC] Close"
            };
            closeButton.Click += (sender, args) => Close();
            Add(closeButton);

            itemDetails = new ItemDetailsControl(57, Height - 10, player)
            {
                Position = new Microsoft.Xna.Framework.Point(Width - 58, 3)
            };
            Add(itemDetails);

            var scrollBarTheme = new ScrollBarTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(DefaultForeground, DefaultBackground)
                }
            };

            var listBoxHeight = (int) Math.Floor((Height - 4) / 2d);

            var itemList1Scroll = new ScrollBar(Orientation.Vertical, listBoxHeight)
            {
                Position = new Microsoft.Xna.Framework.Point(Width - 60, 3),
                Theme = scrollBarTheme,
                CanFocus = false
            };
            Add(itemList1Scroll);
            itemsList1 = new CustomListBox<InventoryStackItem>(Width - 61, listBoxHeight, itemList1Scroll)
            {
                Position = new Microsoft.Xna.Framework.Point(1, 3)
            };
            itemsList1.SelectionChanged += ItemsList1OnSelectionChanged;
            Add(itemsList1);

            Print(0, 3 + listBoxHeight, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            Fill(1, 3 + listBoxHeight, Width - 60, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));

            var itemList2Scroll = new ScrollBar(Orientation.Vertical, listBoxHeight - 1)
            {
                Position = new Microsoft.Xna.Framework.Point(Width - 60, 4 + listBoxHeight),
                Theme = scrollBarTheme,
                CanFocus = false
            };
            Add(itemList2Scroll);
            itemsList2 = new CustomListBox<InventoryStackItem>(Width - 61, listBoxHeight - 1, itemList2Scroll)
            {
                Position = new Microsoft.Xna.Framework.Point(1, 4 + listBoxHeight)
            };
            itemsList2.SelectionChanged += ItemsList2OnSelectionChanged;
            Add(itemsList2);

            moveStackUpButton = new StandardButton(21)
            {
                Position = new Microsoft.Xna.Framework.Point(Width - 57, 40),
                Text = "Move To Storage"
            };
            Add(moveStackUpButton);

            moveStackDownButton = new StandardButton(21)
            {
                Position = new Microsoft.Xna.Framework.Point(Width - 57, 40),
                Text = "Move To Inventory"
            };
            Add(moveStackDownButton);

            moveItemUpButton = new StandardButton(25)
            {
                Position = new Microsoft.Xna.Framework.Point(Width - 34, 40),
                Text = "Move One To Storage"
            };
            Add(moveItemUpButton);

            moveItemDownButton = new StandardButton(25)
            {
                Position = new Microsoft.Xna.Framework.Point(Width - 34, 40),
                Text = "Move One To Inventory"
            };
            Add(moveItemDownButton);
        }

        private void RefreshAllItems(bool keepSelection)
        {
            RefreshItems(itemsList1, storage, stack => new CustomInventoryItem(stack), keepSelection);
            RefreshItems(itemsList2, game.Player.Inventory, stack => new PlayerInventoryItem(stack, game.Player), keepSelection);
        }

        private void MoveItemDown()
        {
            if (SelectedStack1 == null)
                return;

            var item = SelectedStack1.TopItem;
            storage.RemoveItem(item);
            game.Player.Inventory.AddItem(item);

            RefreshAllItems(true);
        }

        public override bool ProcessMouse(MouseConsoleState state)
        {
            if (!state.Mouse.LeftClicked)
                return base.ProcessMouse(state);

            if (moveStackUpButton.IsVisible && moveStackUpButton.MouseBounds.Contains(state.CellPosition))
            {
                MoveStackUp();
                return true;
            }

            if (moveStackDownButton.IsVisible && moveStackDownButton.MouseBounds.Contains(state.CellPosition))
            {
                MoveStackDown();
                return true;
            }

            if (moveItemUpButton.IsVisible && moveItemUpButton.MouseBounds.Contains(state.CellPosition))
            {
                MoveItemUp();
                return true;
            }

            if (moveItemDownButton.IsVisible && moveItemDownButton.MouseBounds.Contains(state.CellPosition))
            {
                MoveItemDown();
                return true;
            }

            return base.ProcessMouse(state);
        }

        private void MoveItemUp()
        {
            if (SelectedStack2 == null)
                return;

            var item = SelectedStack2.TopItem;
            if (item.Equals(inventoryItem))
                return;

            if (maxWeight.HasValue && storage.GetWeight() + item.Weight > maxWeight.Value)
                return;

            game.Player.Inventory.RemoveItem(item);
            storage.AddItem(item);

            RefreshAllItems(true);
        }

        private void MoveStackDown()
        {
            if (SelectedStack1 == null)
                return;

            foreach (var item in SelectedStack1.Items.ToArray())
            {
                storage.RemoveItem(item);
                game.Player.Inventory.AddItem(item);
            }

            RefreshAllItems(false);
        }

        private void MoveStackUp()
        {
            if (SelectedStack2 == null)
                return;

            if (maxWeight.HasValue && storage.GetWeight() + SelectedStack2.Weight > maxWeight.Value)
                return;

            if (SelectedStack2.TopItem.Equals(inventoryItem))
                return;
            foreach (var item in SelectedStack2.Items.ToArray())
            {
                game.Player.Inventory.RemoveItem(item);
                storage.AddItem(item);
            }

            RefreshAllItems(false);
        }

        private void ItemsList2OnSelectionChanged(object sender, EventArgs e)
        {
            if (SelectedStack2 != null)
            {
                itemsList1.SelectedItemIndex = -1;
            }
            ProcessSelectedItemChanged();
        }

        private void ItemsList1OnSelectionChanged(object sender, EventArgs e)
        {
            if (SelectedStack1 != null)
            {
                itemsList2.SelectedItemIndex = -1;
            }
            ProcessSelectedItemChanged();
        }

        private void ProcessSelectedItemChanged()
        {
            itemDetails.Stack = SelectedStack1 ?? SelectedStack2;

            moveStackUpButton.IsVisible = SelectedStack2 != null && SelectedStack2.Count > 0 && !SelectedStack2.TopItem.Equals(inventoryItem);
            moveItemUpButton.IsVisible = SelectedStack2 != null && SelectedStack2.Count > 0 && !SelectedStack2.TopItem.Equals(inventoryItem);

            moveStackDownButton.IsVisible = SelectedStack1 != null;
            moveItemDownButton.IsVisible = SelectedStack1 != null;
        }

        private void RefreshItems(
            CustomListBox<InventoryStackItem> listBox,
            Inventory inventory,
            Func<InventoryStack, InventoryStackItem> stackItemFactory,
            bool keepSelection)
        {
            var selectionIndex = listBox.SelectedItemIndex;
            listBox.ClearItems();

            foreach (var inventoryStack in inventory.Stacks)
            {
                listBox.AddItem(stackItemFactory(inventoryStack));
            }

            if (keepSelection && selectionIndex < listBox.Items.Length)
            {
                listBox.SelectedItemIndex = selectionIndex;
            }
            else if (listBox.Items.Length > 0)
            {
                listBox.SelectedItemIndex = 0;
            }

            ProcessSelectedItemChanged();
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.Escape:
                    Close();
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Print(2, 1, GetTitleWithWeight());

            Fill(1, 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
            Print(0, 2, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            Print(Width - 1, 2, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            Print(Width - 59, 2, new ColoredGlyph(Glyphs.GetGlyph('┬'), FrameColor, DefaultBackground));
            Print(Width - 59, Height - 1, new ColoredGlyph(Glyphs.GetGlyph('╧'), FrameColor, DefaultBackground));
            DrawVerticalLine(Width - 59, 3, Height - 4, new ColoredGlyph(Glyphs.GetGlyph('│'), FrameColor, DefaultBackground));

            Print(Width - 59, 4, new ColoredGlyph(Glyphs.GetGlyph('├'), FrameColor, DefaultBackground));
            Print(Width - 1, 4, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            Print(Width - 59, 3 + 23, new ColoredGlyph(Glyphs.GetGlyph('┤'), FrameColor, DefaultBackground));
        }

        private string GetTitleWithWeight()
        {
            if (!maxWeight.HasValue)
                return inventoryName;

            const double kgWeightMultiplier = 1000d;

            var currentWeight = storage.GetWeight() / kgWeightMultiplier;
            var maxWeightValue = maxWeight.Value / kgWeightMultiplier;
            return $"{inventoryName} [Weight: {currentWeight:F2} / {maxWeightValue:F2}]";
        }
    }
}