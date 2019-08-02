using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
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

            itemDetails = new ItemDetailsControl(57, Height - 10)
            {
                Position = new Point(Width - 58, 3)
            };
            Add(itemDetails);

            itemsListBox = new ListBox(Width - 60, Height - 6)
            {
                Position = new Point(1, 3),
                CompareByReference = true,
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
            Add(itemsListBox);

            RefreshItems(false);
        }

        private void RefreshItems(bool keepSelection)
        {
            var selectionIndex = itemsListBox.SelectedIndex;
            itemsListBox.Items.Clear();

            foreach (var inventoryStack in game.Player.Inventory.Stacks)
            {
                itemsListBox.Items.Add(new InventoryStackListBoxItem(inventoryStack));
            }

            if (keepSelection)
            {
                itemsListBox.SelectedItem = itemsListBox.Items[selectionIndex];
            }
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
                case Keys.Escape:
                    Close();
                    return true;
            }
            return base.ProcessKeyPressed(key);
        }

        private class InventoryStackListBoxItem
        {
            public InventoryStackListBoxItem(InventoryStack itemStack)
            {
                ItemStack = itemStack;
            }

            public InventoryStack ItemStack { get; }

            public override string ToString()
            {
                if (ItemStack.Item.Stackable)
                {
                    return $" {ItemStack.Item.Name} ({ItemStack.Count})";
                }

                return $" {ItemStack.Item.Name}";
            }
        }
    }
}