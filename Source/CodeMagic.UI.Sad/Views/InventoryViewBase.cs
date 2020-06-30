using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using SadConsole;
using SadConsole.Input;
using SadConsole.UI.Controls;
using SadConsole.UI.Themes;
using SadRogue.Primitives;
using Point = SadRogue.Primitives.Point;

namespace CodeMagic.UI.Sad.Views
{
    public abstract class InventoryViewBase : GameViewBase
    {
        private readonly string inventoryName;

        private CustomListBox<InventoryStackItem> itemsList;
        private ItemDetailsControl itemDetails;
        private StandardButton closeButton;

        private StandardButton filterAllButton;
        private StandardButton filterArmorButton;
        private StandardButton filterWeaponButton;
        private StandardButton filterUsableButton;
        private StandardButton filterOtherButton;

        private FilterType filter;

        protected InventoryViewBase(string inventoryName, Player player)
        {
            this.inventoryName = inventoryName;

            filter = FilterType.All;
            InitializeControls(player);
        }

        protected InventoryStack SelectedStack => itemsList.SelectedItem?.Stack;

        private void InitializeControls(Player player)
        {
            closeButton = new StandardButton(15, 3)
            {
                Position = new Point(Width - 17, Height - 4),
                Text = "[ESC] Close"
            };
            closeButton.Click += closeButton_Click;
            ControlHostComponent.Add(closeButton);

            itemDetails = new ItemDetailsControl(52, Height - 10, player)
            {
                Position = new Point(Width - 53, 3)
            };
            ControlHostComponent.Add(itemDetails);

            filterAllButton = new StandardButton(5)
            {
                Position = new Point(2, 3),
                DisabledColor = Color.Blue,
                Text = "All"
            };
            filterAllButton.Click += (sender, args) => ChangeFilter(FilterType.All);
            ControlHostComponent.Add(filterAllButton);

            filterWeaponButton = new StandardButton(5)
            {
                Position = new Point(7, 3),
                DisabledColor = Color.Blue,
                Text = "─┼ ".ConvertGlyphs()
            };
            filterWeaponButton.Click += (sender, args) => ChangeFilter(FilterType.Weapon);
            ControlHostComponent.Add(filterWeaponButton);
            
            filterArmorButton = new StandardButton(5)
            {
                Position = new Point(12, 3),
                DisabledColor = Color.Blue,
                Text = "╭█╮".ConvertGlyphs()
            };
            filterArmorButton.Click += (sender, args) => ChangeFilter(FilterType.Armor);
            ControlHostComponent.Add(filterArmorButton);

            filterUsableButton = new StandardButton(5)
            {
                Position = new Point(17, 3),
                DisabledColor = Color.Blue,
                Text = " ▲ ".ConvertGlyphs()
            };
            filterUsableButton.Click += (sender, args) => ChangeFilter(FilterType.Usable);
            ControlHostComponent.Add(filterUsableButton);

            filterOtherButton = new StandardButton(5)
            {
                Position = new Point(22, 3),
                DisabledColor = Color.Blue,
                Text = " ? ".ConvertGlyphs()
            };
            filterOtherButton.Click += (sender, args) => ChangeFilter(FilterType.Other);
            ControlHostComponent.Add(filterOtherButton);

            var scrollBarTheme = new ScrollBarTheme
            {
                Normal = new ColoredGlyph(DefaultForeground, DefaultBackground)
            };
            var itemListScroll = new ScrollBar(Orientation.Vertical, Height - 4)
            {
                Position = new Point(Width - 55, 6),
                Theme = scrollBarTheme,
                CanFocus = false
            };
            ControlHostComponent.Add(itemListScroll);
            itemsList = new CustomListBox<InventoryStackItem>(Width - 56, Height - 4, itemListScroll)
            {
                Position = new Point(1, 6)
            };
            itemsList.SelectionChanged += itemsListBox_SelectedItemChanged;
            ControlHostComponent.Add(itemsList);

            RefreshFilterButtonsState();
        }

        private void ChangeFilter(FilterType newFilter)
        {
            filter = newFilter;
            RefreshFilterButtonsState();
            RefreshItems(false);
        }

        protected abstract IEnumerable<InventoryStack> GetStacks();

        private void closeButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void itemsListBox_SelectedItemChanged(object sender, EventArgs e)
        {
            ProcessSelectedItemChanged();
        }

        protected virtual void ProcessSelectedItemChanged()
        {
            itemDetails.Stack = SelectedStack;
        }

        private bool CheckFilter(IItem item)
        {
            switch (filter)
            {
                case FilterType.All:
                    return true;
                case FilterType.Weapon:
                    return item is IWeaponItem;
                case FilterType.Armor:
                    return item is IEquipableItem && !(item is IWeaponItem);
                case FilterType.Usable:
                    return item is IUsableItem;
                case FilterType.Other:
                    return !(item is IEquipableItem) && !(item is IUsableItem);
                default:
                    throw new ArgumentException($"Unknown filter type: {filter}");
            }
        }

        protected void RefreshItems(bool keepSelection)
        {
            var selectionIndex = itemsList.SelectedItemIndex;
            itemsList.ClearItems();

            foreach (var inventoryStack in GetStacks().Where(stack => CheckFilter(stack.TopItem)))
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
                    Hide();
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

        private void RefreshFilterButtonsState()
        {
            filterAllButton.IsEnabled = true;
            filterWeaponButton.IsEnabled = true;
            filterArmorButton.IsEnabled = true;
            filterUsableButton.IsEnabled = true;
            filterOtherButton.IsEnabled = true;

            switch (filter)
            {
                case FilterType.All:
                    filterAllButton.IsEnabled = false;
                    break;
                case FilterType.Weapon:
                    filterWeaponButton.IsEnabled = false;
                    break;
                case FilterType.Armor:
                    filterArmorButton.IsEnabled = false;
                    break;
                case FilterType.Usable:
                    filterUsableButton.IsEnabled = false;
                    break;
                case FilterType.Other:
                    filterOtherButton.IsEnabled = false;
                    break;
                default:
                    throw new ArgumentException($"Unknown filter: {filter}");
            }
        }

        protected override void DrawView(ICellSurface surface)
        {
            base.DrawView(surface);

            surface.Print(2, 1, inventoryName);

            surface.Fill(1, 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
            surface.Print(0, 2, new ColoredGlyph(FrameColor, DefaultBackground, Glyphs.GetGlyph('╟')));
            surface.Print(Width - 1, 2, new ColoredGlyph(FrameColor, DefaultBackground, Glyphs.GetGlyph('╢')));

            surface.Print(Width - 54, 2, new ColoredGlyph(FrameColor, DefaultBackground, Glyphs.GetGlyph('┬')));
            surface.Print(Width - 54, Height - 1, new ColoredGlyph(FrameColor, DefaultBackground, Glyphs.GetGlyph('╧')));
            surface.DrawVerticalLine(Width - 54, 3, Height - 4, new ColoredGlyph(FrameColor, DefaultBackground, Glyphs.GetGlyph('│')));

            surface.Print(Width - 54, 4, new ColoredGlyph(FrameColor, DefaultBackground, Glyphs.GetGlyph('├')));
            surface.Print(Width - 1, 4, new ColoredGlyph(FrameColor, DefaultBackground, Glyphs.GetGlyph('╢')));
        }

        protected abstract InventoryStackItem CreateListBoxItem(InventoryStack stack);

        private enum FilterType
        {
            All,
            Weapon,
            Armor,
            Usable,
            Other
        }
    }

    public abstract class InventoryStackItem : ICustomListBoxItem
    {
        private static readonly Color SelectedItemTextColor = Color.FromNonPremultiplied(0, 0, 0, 255);
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

        private ColoredString[] GetNameText(bool selected, Color backColor)
        {
            var itemColor = selected ? SelectedItemTextColor : ItemDrawingHelper.GetItemColor(Stack.TopItem).ToSad();

            return new[]
            {
                new ColoredString(Stack.TopItem.Name.ConvertGlyphs(), itemColor, backColor)
            };
        }

        protected abstract ColoredString[] GetAfterNameText(Color backColor);

        public void Draw(ICellSurface surface, int y, int maxWidth, bool selected)
        {
            var backColor = selected ? SelectedItemBackColor : DefaultBackColor;
            surface.Fill(0, y, maxWidth, null, backColor, null);

            var text = GetNameText(selected, backColor);

            var afterNameText = new List<ColoredString>();
            if (Stack.TopItem is DurableItem durableItem)
            {
                var durabilityColor = TextHelper.GetDurabilityColor(durableItem.Durability, durableItem.MaxDurability);
                afterNameText.Add(new ColoredString(new ColoredString.ColoredGlyphEffect()
                {
                    Glyph = Glyphs.GetGlyph('•'),
                    Foreground = durabilityColor.ToSad(),
                    Background = backColor
                }));
            }
            afterNameText.AddRange(GetAfterNameText(backColor));
            var formattedText = FormatText(text, afterNameText.ToArray(), backColor, maxWidth - 1);
            surface.Print(1, y, formattedText);
            surface.PrintStyledText(1 + formattedText.Count, y, afterNameText.ToArray());
            surface.Print(maxWidth - 6, y, new ColoredString(GetWeightText(), WeightColor, backColor));
        }

        private string GetWeightText()
        {
            const double kgMultiplier = 1000d;
            var weightText = $"{Stack.Weight / kgMultiplier:F2}";

            if (weightText.Length >= 5)
                return weightText;
            if (weightText.Length >= 4)
                return $" {weightText}";

            return $"  {weightText}";
        }

        private ColoredString FormatText(ColoredString[] initialText, ColoredString[] afterNameText, Color backColor, int maxWidth)
        {
            var leftWidth = maxWidth - afterNameText.Sum(part => part.Count) - 9;
            var glyphs = initialText.SelectMany(part => part.ToArray()).ToArray();
            var maxTextWidth = Math.Min(leftWidth, glyphs.Length);
            var textPart = glyphs.Take(maxTextWidth).ToArray();

            if (textPart.Length < glyphs.Length)
            {
                var result = new List<ColoredGlyph>(textPart);
                result.AddRange(new[]
                {
                        new ColoredGlyph(WeightColor, backColor, '.'),
                        new ColoredGlyph(WeightColor, backColor, '.'),
                        new ColoredGlyph(WeightColor, backColor, '.')
                    });
                textPart = result.Select(glyph => new ColoredString.ColoredGlyphEffect()
                {
                    Glyph = glyph.Glyph,
                    Foreground = glyph.Foreground,
                    Background = glyph.Background
                }).ToArray();
            }
            return new ColoredString(textPart);
        }
    }
}