using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Views.BuildingUI
{
    public abstract class BuildingUIView : View
    {
        protected readonly IGameCore Game;
        private CustomListBox<InventoryStackItem> itemsList;
        private Button closeButton;
        private readonly string buildingName;

        protected BuildingUIView(IGameCore game, string buildingName) : base(Program.Width, Program.Height)
        {
            Game = game;
            this.buildingName = buildingName;

            InitializeControls();
        }

        private void InitializeControls()
        {
            closeButton = new StandardButton(15)
            {
                Position = new Microsoft.Xna.Framework.Point(Width - 17, Height - 4),
                Text = "[ESC] Close"
            };
            closeButton.Click += (sender, args) => Close();
            Add(closeButton);

            var scrollBarTheme = new ScrollBarTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(DefaultForeground, DefaultBackground)
                }
            };

            var listBoxHeight = (int)Math.Floor((Height - 4) / 2d);

            var itemListScroll = new ScrollBar(Orientation.Vertical, listBoxHeight - 1)
            {
                Position = new Microsoft.Xna.Framework.Point(Width - 40, 4 + listBoxHeight),
                Theme = scrollBarTheme,
                CanFocus = false
            };
            Add(itemListScroll);
            itemsList = new CustomListBox<InventoryStackItem>(Width - 41, listBoxHeight, itemListScroll)
            {
                Position = new Microsoft.Xna.Framework.Point(1, 4 + listBoxHeight)
            };
            itemsList.SelectionChanged += (sender, args) => ProcessSelectedItemChanged();
            Add(itemsList);

            Print(0, 3 + listBoxHeight, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            Fill(1, 3 + listBoxHeight, Width - 40, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
        }

        protected InventoryStack SelectedStack => itemsList.SelectedItem?.Stack;

        protected void RefreshItems(bool keepSelection)
        {
            var selectionIndex = itemsList.SelectedItemIndex;
            itemsList.ClearItems();

            foreach (var inventoryStack in Game.Player.Inventory.Stacks)
            {
                itemsList.AddItem(new PlayerInventoryItem(inventoryStack, Game.Player));
            }

            if (keepSelection && selectionIndex < itemsList.Items.Length)
            {
                itemsList.SelectedItemIndex = selectionIndex;
            }
            else if (itemsList.Items.Length > 0)
            {
                itemsList.SelectedItemIndex = 0;
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

            Print(2, 1, buildingName);

            Fill(1, 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
            Print(0, 2, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            Print(Width - 1, 2, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            Print(Width - 39, 2, new ColoredGlyph(Glyphs.GetGlyph('┬'), FrameColor, DefaultBackground));
            Print(Width - 39, Height - 1, new ColoredGlyph(Glyphs.GetGlyph('╧'), FrameColor, DefaultBackground));
            DrawVerticalLine(Width - 39, 3, Height - 4, new ColoredGlyph(Glyphs.GetGlyph('│'), FrameColor, DefaultBackground));

            Print(Width - 39, 3 + 23, new ColoredGlyph(Glyphs.GetGlyph('┤'), FrameColor, DefaultBackground));
        }

        protected virtual void ProcessSelectedItemChanged()
        {
        }
    }
}