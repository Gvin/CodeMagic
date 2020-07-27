using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Mono.Controls;
using CodeMagic.UI.Mono.Drawing;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Mono.Fonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Views
{
    public abstract class InventoryViewBase : BaseWindow
    {
        public event EventHandler Exit;

        private ListBox<InventoryStackItem> itemsList;
        private ItemDetailsControl itemDetails;
        private FramedButton closeButton;

        private FramedButton filterAllButton;
        private FramedButton filterArmorButton;
        private FramedButton filterWeaponButton;
        private FramedButton filterUsableButton;
        private FramedButton filterOtherButton;

        private FilterType filter;

        protected InventoryViewBase() : base(FontTarget.Interface)
        {
            filter = FilterType.All;
        }

        protected abstract string InventoryName { get; }

        public Player Player { protected get; set; }

        public InventoryStack SelectedStack => itemsList.SelectedItem?.Stack;

        public virtual void Initialize()
        {
            closeButton = new FramedButton(new Rectangle(Width - 17, Height - 4, 15, 3))
            {
                Text = "[ESC] Close"
            };
            closeButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Controls.Add(closeButton);

            itemDetails = new ItemDetailsControl(new Rectangle(Width - 53, 3, 52, Height - 10), Player);
            Controls.Add(itemDetails);

            filterAllButton = new FramedButton(new Rectangle(2, 3, 5, 3))
            {
                Text = "All"
            };
            filterAllButton.Theme.Disabled.Text = new ColorsPalette(Color.Blue);
            filterAllButton.Click += (sender, args) => ChangeFilter(FilterType.All);
            Controls.Add(filterAllButton);

            filterWeaponButton = new FramedButton(new Rectangle(7, 3, 5, 3))
            {
                Text = "─┼ "
            };
            filterWeaponButton.Theme.Disabled.Text = new ColorsPalette(Color.Blue);
            filterWeaponButton.Click += (sender, args) => ChangeFilter(FilterType.Weapon);
            Controls.Add(filterWeaponButton);

            filterArmorButton = new FramedButton(new Rectangle(12, 3, 5, 3))
            {
                Text = "╭█╮"
            };
            filterArmorButton.Theme.Disabled.Text = new ColorsPalette(Color.Blue);
            filterArmorButton.Click += (sender, args) => ChangeFilter(FilterType.Armor);
            Controls.Add(filterArmorButton);

            filterUsableButton = new FramedButton(new Rectangle(17, 3, 5, 3))
            {
                Text = " ▲ "
            };
            filterUsableButton.Theme.Disabled.Text = new ColorsPalette(Color.Blue);
            filterUsableButton.Click += (sender, args) => ChangeFilter(FilterType.Usable);
            Controls.Add(filterUsableButton);

            filterOtherButton = new FramedButton(new Rectangle(22, 3, 5, 3))
            {
                Text = " ? "
            };
            filterOtherButton.Theme.Disabled.Text = new ColorsPalette(Color.Blue);
            filterOtherButton.Click += (sender, args) => ChangeFilter(FilterType.Other);
            Controls.Add(filterOtherButton);

            var itemListScroll = new VerticalScrollBar(new Point(Width - 55, 6), Height - 7);
            Controls.Add(itemListScroll);
            itemsList = new ListBox<InventoryStackItem>(new Rectangle(1, 6, Width - 56, Height - 7), itemListScroll);
            itemsList.SelectionChanged += itemsListBox_SelectedItemChanged;
            Controls.Add(itemsList);

            RefreshFilterButtonsState();
        }

        private void ChangeFilter(FilterType newFilter)
        {
            filter = newFilter;
            RefreshFilterButtonsState();
            RefreshItems(false);
        }

        public InventoryStack[] Stacks
        {
            private get;
            set;
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

            foreach (var inventoryStack in Stacks.Where(stack => CheckFilter(stack.TopItem)))
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

        public override bool ProcessKeyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Escape:
                    Exit?.Invoke(this, EventArgs.Empty);
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
            filterAllButton.Enabled = true;
            filterWeaponButton.Enabled = true;
            filterArmorButton.Enabled = true;
            filterUsableButton.Enabled = true;
            filterOtherButton.Enabled = true;

            switch (filter)
            {
                case FilterType.All:
                    filterAllButton.Enabled = false;
                    break;
                case FilterType.Weapon:
                    filterWeaponButton.Enabled = false;
                    break;
                case FilterType.Armor:
                    filterArmorButton.Enabled = false;
                    break;
                case FilterType.Usable:
                    filterUsableButton.Enabled = false;
                    break;
                case FilterType.Other:
                    filterOtherButton.Enabled = false;
                    break;
                default:
                    throw new ArgumentException($"Unknown filter: {filter}");
            }
        }

        public override void Draw(ICellSurface surface)
        {
            base.Draw(surface);

            surface.Write(2, 1, InventoryName);

            surface.Fill(new Rectangle(1, 2, Width - 2, 1), new Cell('─', FrameColor));
            surface.SetCell(0, 2, '╟', FrameColor);
            surface.SetCell(Width - 1, 2, '╢', FrameColor);

            surface.SetCell(Width - 54, 2, '┬', FrameColor);
            surface.SetCell(Width - 54, Height - 1, '╧', FrameColor);
            surface.Fill(new Rectangle(Width - 54, 3, 1, Height - 4), new Cell('│', FrameColor));

            surface.SetCell(Width - 54, 4, '├', FrameColor);
            surface.SetCell(Width - 1, 4, '╢', FrameColor);
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

    public abstract class InventoryStackItem : IListBoxItem
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

        public bool Equals(IListBoxItem other)
        {
            return ReferenceEquals(this, other);
        }

        private ColoredString[] GetNameText(bool selected, Color backColor)
        {
            var itemColor = selected ? SelectedItemTextColor : ItemDrawingHelper.GetItemColor(Stack.TopItem).ToXna();

            return new[]
            {
                new ColoredString(Stack.TopItem.Name, itemColor, backColor)
            };
        }

        protected abstract ColoredString[] GetAfterNameText(Color backColor);

        public void Draw(ICellSurface surface, int y, int maxWidth, bool selected)
        {
            var backColor = selected ? SelectedItemBackColor : DefaultBackColor;
            surface.Fill(new Rectangle(0, y, maxWidth, 1), new Cell(' ', backColor: backColor));

            var text = GetNameText(selected, backColor);

            var afterNameText = new List<ColoredString>();
            if (Stack.TopItem is DurableItem durableItem)
            {
                var durabilityColor = TextHelper.GetDurabilityColor(durableItem.Durability, durableItem.MaxDurability);
                afterNameText.Add(new ColoredString("•", durabilityColor.ToXna(), backColor));
            }
            afterNameText.AddRange(GetAfterNameText(backColor));
            var formattedText = FormatText(text, afterNameText.ToArray(), backColor, maxWidth - 1);
            surface.Write(1, y, formattedText);
            surface.Write(1 + formattedText.Cells.Length, y, afterNameText.ToArray());
            surface.Write(maxWidth - 6, y, new ColoredString(GetWeightText(), WeightColor, backColor));
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
            var leftWidth = maxWidth - afterNameText.Sum(part => part.Cells.Length) - 9;
            var glyphs = initialText.SelectMany(part => part.Cells).ToArray();
            var maxTextWidth = Math.Min(leftWidth, glyphs.Length);
            var textPart = glyphs.Take(maxTextWidth).ToArray();

            if (textPart.Length < glyphs.Length)
            {
                var result = new List<Cell>(textPart);
                result.AddRange(new[]
                {
                        new Cell('.', WeightColor, backColor),
                        new Cell('.', WeightColor, backColor),
                        new Cell('.', WeightColor, backColor)
                    });
                textPart = result.ToArray();
            }
            return new ColoredString(textPart);
        }
    }
}