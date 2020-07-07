using System;
using System.Collections.Generic;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Usable;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Input;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class PlayerInventoryView : InventoryViewBase, IPlayerInventoryView
    {
        private const string Title = "Player Inventory";

        private StandardButton useItemButton;
        private StandardButton equipItemButton;
        private StandardButton equipRightHoldableButton;
        private StandardButton equipLeftHoldableButton;
        private StandardButton takeOffItemButton;

        private StandardButton dropItemButton;
        private StandardButton dropAllItemsButton;
        private StandardButton checkScrollButton;

        protected override string InventoryName => GetTitleWithWeight(Player);

        public event EventHandler UseItem;
        public event EventHandler EquipItem;
        public event EventHandler EquipHoldableItemLeft;
        public event EventHandler EquipHoldableItemRight;
        public event EventHandler DropItem;
        public event EventHandler DropStack;
        public event EventHandler TakeOffItem;
        public event EventHandler CheckScroll;

        public override void Initialize()
        {
            base.Initialize();
            InitializeControls();

            RefreshItems(false);
        }

        private void InitializeControls()
        {
            checkScrollButton = new StandardButton(20)
            {
                Position = new Point(Width - 30, 40),
                Text = "[C] Check Scroll"
            };
            checkScrollButton.Click += (sender, args) => CheckScroll?.Invoke(this, EventArgs.Empty);
            Add(checkScrollButton);

            useItemButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 40),
                Text = "[U] Use"
            };
            useItemButton.Click += (sender, args) => UseItem?.Invoke(this, EventArgs.Empty);
            Add(useItemButton);

            equipItemButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 40),
                Text = "[E] Equip"
            };
            equipItemButton.Click += (sender, args) => EquipItem?.Invoke(this, EventArgs.Empty);
            Add(equipItemButton);

            equipLeftHoldableButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 40),
                Text = "[Z] Equip Left"
            };
            equipLeftHoldableButton.Click += (sender, args) => EquipHoldableItemLeft?.Invoke(this, EventArgs.Empty);
            Add(equipLeftHoldableButton);

            equipRightHoldableButton = new StandardButton(20)
            {
                Position = new Point(Width - 31, 40),
                Text = "[X] Equip Right"
            };
            equipRightHoldableButton.Click += (sender, args) => EquipHoldableItemRight?.Invoke(this, EventArgs.Empty);
            Add(equipRightHoldableButton);

            takeOffItemButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 40),
                Text = "[T] Take Off"
            };
            takeOffItemButton.Click += (sender, args) => TakeOffItem?.Invoke(this, EventArgs.Empty);
            Add(takeOffItemButton);

            dropItemButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 43),
                Text = "[D] Drop"
            };
            dropItemButton.Click += (sender, args) => DropItem?.Invoke(this, EventArgs.Empty);
            Add(dropItemButton);

            dropAllItemsButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 46),
                Text = "[A] Drop All"
            };
            dropAllItemsButton.Click += (sender, args) => DropStack?.Invoke(this, EventArgs.Empty);
            Add(dropAllItemsButton);
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
            checkScrollButton.IsVisible = SelectedStack?.TopItem is ScrollBase;

            if (SelectedStack?.TopItem is IEquipableItem equipable)
            {
                var isEquipped = Player.Equipment.IsEquiped(equipable);
                takeOffItemButton.IsVisible = isEquipped;

                var isHoldable = equipable is IHoldableItem;

                equipItemButton.IsVisible = !isEquipped && !isHoldable;
                equipLeftHoldableButton.IsVisible = !isEquipped && isHoldable;
                equipRightHoldableButton.IsVisible = !isEquipped && isHoldable;
            }
            else
            {
                takeOffItemButton.IsVisible = false;
                equipItemButton.IsVisible = false;
                equipLeftHoldableButton.IsVisible = false;
                equipRightHoldableButton.IsVisible = false;
            }
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.U:
                    UseItem?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.E:
                    EquipItem?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.Z:
                    EquipHoldableItemLeft?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.X:
                    EquipHoldableItemRight?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.T:
                    TakeOffItem?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.D:
                    DropItem?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.A:
                    DropStack?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.C:
                    CheckScroll?.Invoke(this, EventArgs.Empty);
                    return true;
            }
            return base.ProcessKeyPressed(key);
        }

        private static string GetTitleWithWeight(IPlayer player)
        {
            const double kgWeightMultiplier = 1000d;

            var currentWeight = player.Inventory.GetWeight() / kgWeightMultiplier;
            var maxWeight = player.MaxCarryWeight / kgWeightMultiplier;
            return $"{Title} [Weight: {currentWeight:F2} / {maxWeight:F2}]";
        }

        protected override InventoryStackItem CreateListBoxItem(InventoryStack stack)
        {
            return new PlayerInventoryItem(stack, Player);
        }

        public void Close()
        {
            Close(DialogResult.None);
        }
    }

    public class PlayerInventoryItem : InventoryStackItem
    {
        private const string EquippedText = "[Eq]";
        private const string EquippedLeftText = "[EL]";
        private const string EquippedRightText = "[ER]";
        private static readonly Color EquippedTextColor = Color.Red;

        private readonly Player player;

        public PlayerInventoryItem(InventoryStack itemStack, Player player)
            : base(itemStack)
        {
            this.player = player;
        }

        protected override ColoredString[] GetAfterNameText(Color backColor)
        {
            var result = new List<ColoredString>();

            if (Stack.TopItem.Stackable)
            {
                result.Add(new ColoredString($" ({Stack.Count})", new Cell(StackCountColor, backColor)));
            }

            var equippedText = GetEquippedText(player, Stack);
            if (!string.IsNullOrEmpty(equippedText))
            {
                result.Add(new ColoredString($" {equippedText}", new Cell(EquippedTextColor, backColor)));
            }

            return result.ToArray();
        }

        private static string GetEquippedText(Player player, InventoryStack stack)
        {
            if (!(stack.TopItem is IEquipableItem equipable))
                return null;

            if (!player.Equipment.IsEquiped(equipable))
                return null;

            if (!(equipable is IHoldableItem holdableItem))
                return EquippedText;

            if (player.Equipment.LeftHandItem.Equals(holdableItem))
                return EquippedLeftText;

            return EquippedRightText;
        }

        public static bool GetIfEquipped(Player player, InventoryStack stack)
        {
            if (!(stack.TopItem is IEquipableItem equipable))
                return false;

            return player.Equipment.IsEquiped(equipable);
        }
    }
}