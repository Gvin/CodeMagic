using System;
using System.Collections.Generic;
using System.Linq;
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
    public abstract class InventoryViewBase : View
    {
        private readonly string inventoryName;

        private CustomListBox<InventoryStackItem> itemsList;
        private ItemDetailsControl itemDetails;
        private Button closeButton;

        protected InventoryViewBase(string inventoryName, IPlayer player) 
            : base(Program.Width, Program.Height)
        {
            this.inventoryName = inventoryName;

            InitializeControls(player);
        }

        protected InventoryStack SelectedStack => itemsList.SelectedItem?.Stack;

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

            var scrollBarTheme = new ScrollBarTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(DefaultForeground, DefaultBackground)
                }
            };
            var itemListScroll = new ScrollBar(Orientation.Vertical, Height - 4)
            {
                Position = new Point(Width - 60, 3),
                Theme = scrollBarTheme,
                CanFocus = false
            };
            Add(itemListScroll);
            itemsList = new CustomListBox<InventoryStackItem>(Width - 61, Height - 4, itemListScroll)
            {
                Position = new Point(1, 3)
            };
            itemsList.SelectionChanged += itemsListBox_SelectedItemChanged;
            Add(itemsList);
        }

        protected abstract IEnumerable<InventoryStack> GetStacks();

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void itemsListBox_SelectedItemChanged(object sender, EventArgs e)
        {
            ProcessSelectedItemChanged();
        }

        protected virtual void ProcessSelectedItemChanged()
        {
            itemDetails.Stack = SelectedStack;
        }

        protected void RefreshItems(bool keepSelection)
        {
            var selectionIndex = itemsList.SelectedItemIndex;
            itemsList.ClearItems();

            foreach (var inventoryStack in GetStacks())
            {
                itemsList.AddItem(CreateListBoxItem(inventoryStack));
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
                case Keys.Up:
                case Keys.W:
                    MoveSelectionUp();
                    return true;
                case Keys.Down:
                case Keys.S:
                    MoveSelectionDown();
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }

        private void MoveSelectionUp()
        {
            itemsList.SelectedItemIndex = Math.Max(0, itemsList.SelectedItemIndex - 1);
        }

        private void MoveSelectionDown()
        {
            itemsList.SelectedItemIndex = Math.Min(itemsList.Items.Length - 1, itemsList.SelectedItemIndex + 1);
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Print(2, 1, inventoryName);

            Fill(1, 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
            Print(0, 2, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            Print(Width - 1, 2, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            Print(Width - 59, 2, new ColoredGlyph(Glyphs.GetGlyph('┬'), FrameColor, DefaultBackground));
            Print(Width - 59, Height - 1, new ColoredGlyph(Glyphs.GetGlyph('╧'), FrameColor, DefaultBackground));
            DrawVerticalLine(Width - 59, 3, Height - 4, new ColoredGlyph(Glyphs.GetGlyph('│'), FrameColor, DefaultBackground));

            Print(Width - 59, 4, new ColoredGlyph(Glyphs.GetGlyph('├'), FrameColor, DefaultBackground));
            Print(Width - 1, 4, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));
        }

        protected abstract class InventoryStackItem : ICustomListBoxItem
        {
            private static readonly Color SelectedItemBackColor = Color.FromNonPremultiplied(255, 128, 0, 255);
            private static readonly Color DefaultBackColor = Color.Black;
            protected static readonly Color StackCountColor = Color.White;
            private static readonly Color WeightColor = Color.Gray;

            protected InventoryStackItem(InventoryStack stack)
            {
                Stack = stack;
            }

            public InventoryStack Stack { get; }

            public bool Equals(ICustomListBoxItem other)
            {
                return ReferenceEquals(this, other);
            }

            protected abstract ColoredString[] GetNameText(Color backColor);

            protected abstract ColoredString[] GetAfterNameText(Color backColor);

            public void Draw(CellSurface surface, int y, int maxWidth, bool selected)
            {
                var backColor = selected ? SelectedItemBackColor : DefaultBackColor;
                surface.Fill(0, y, maxWidth, null, backColor, null);
                var text = GetNameText(backColor);
                var afterNameText = GetAfterNameText(backColor);
                var formattedText = FormatText(text, afterNameText, backColor, maxWidth);
                surface.Print(1, y, formattedText);
                surface.PrintStyledText(1 + formattedText.Count, y, afterNameText);
                surface.Print(maxWidth - 4, y, new ColoredString(GetWeightText(), WeightColor, backColor));
            }

            private string GetWeightText()
            {
                if (Stack.Weight >= 100)
                    return Stack.Weight.ToString();
                if (Stack.Weight >= 10)
                    return $" {Stack.Weight}";

                return $"  {Stack.Weight}";
            }

            private ColoredString FormatText(ColoredString[] initialText, ColoredString[] afterNameText, Color backColor, int maxWidth)
            {
                var leftWidth = maxWidth - afterNameText.Sum(part => part.Count) - 8;
                var glyphs = initialText.SelectMany(part => part.ToArray()).ToArray();
                var maxTextWidth = Math.Min(leftWidth, glyphs.Length);
                var textPart = glyphs.Take(maxTextWidth).ToArray();

                if (textPart.Length < glyphs.Length)
                {
                    var result = new List<ColoredGlyph>(textPart);
                    result.AddRange(new []
                    {
                        new ColoredGlyph('.', WeightColor, backColor),
                        new ColoredGlyph('.', WeightColor, backColor),
                        new ColoredGlyph('.', WeightColor, backColor)
                    });
                    textPart = result.ToArray();
                }
                return new ColoredString(textPart);
            }
        }

        protected abstract InventoryStackItem CreateListBoxItem(InventoryStack stack);
    }
}