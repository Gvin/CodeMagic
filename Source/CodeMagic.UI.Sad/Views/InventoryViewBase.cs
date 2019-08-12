using System;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public abstract class InventoryViewBase : View
    {
        private readonly string inventoryName;
        protected readonly Inventory Inventory;

        private ListBox itemsListBox;
        private ItemDetailsControl itemDetails;
        private Button closeButton;

        protected InventoryViewBase(string inventoryName, Inventory inventory, IPlayer player) 
            : base(Program.Width, Program.Height)
        {
            this.inventoryName = inventoryName;
            Inventory = inventory;

            InitializeControls(player);
        }

        protected InventoryStack SelectedStack => (itemsListBox.SelectedItem as IInventoryStackListBoxItem)?.ItemStack;

        private void InitializeControls(IPlayer player)
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

            itemDetails = new ItemDetailsControl(57, Height - 10, player)
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
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void itemsListBox_SelectedItemChanged(object sender, ListBox.SelectedItemEventArgs e)
        {
            ProcessSelectedItemChanged();
        }

        protected virtual void ProcessSelectedItemChanged()
        {
            itemDetails.Stack = (itemsListBox.SelectedItem as IInventoryStackListBoxItem)?.ItemStack;
        }

        protected void RefreshItems(bool keepSelection)
        {
            var selectionIndex = itemsListBox.SelectedIndex;
            itemsListBox.Items.Clear();

            foreach (var inventoryStack in Inventory.Stacks)
            {
                itemsListBox.Items.Add(CreateListBoxItem(inventoryStack));
            }

            if (keepSelection && selectionIndex < itemsListBox.Items.Count)
            {
                itemsListBox.SelectedItem = itemsListBox.Items[selectionIndex];
            }

            ProcessSelectedItemChanged();
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Print(2, 1, inventoryName);

            Fill(1, 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GlyphBoxSingleHorizontal);
            Print(0, 2, new ColoredGlyph(Glyphs.GlyphBoxDoubleVerticalSingleRight, FrameColor, DefaultBackground));
            Print(Width - 1, 2, new ColoredGlyph(Glyphs.GlyphBoxDoubleVerticalSingleLeft, FrameColor, DefaultBackground));

            Print(Width - 59, 2, new ColoredGlyph(Glyphs.GlyphBoxSingleHorizontalDown, FrameColor, DefaultBackground));
            Print(Width - 59, Height - 1, new ColoredGlyph(Glyphs.GlyphBoxDoubleHorizontalSingleUp, FrameColor, DefaultBackground));
            DrawVerticalLine(Width - 59, 3, Height - 4, new ColoredGlyph(Glyphs.GlyphBoxSingleVertical, FrameColor, DefaultBackground));
        }

        protected abstract IInventoryStackListBoxItem CreateListBoxItem(InventoryStack stack);

        protected interface IInventoryStackListBoxItem
        {
            InventoryStack ItemStack { get; }
        }
    }
}