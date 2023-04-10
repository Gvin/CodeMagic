using System;
using System.Collections.Generic;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Usable;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Mono.Drawing.ImageProviding;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Views;

public class PlayerInventoryView : InventoryViewBase, IPlayerInventoryView
{
    private const string Title = "Player Inventory";

    private FramedButton _useItemButton;
    private FramedButton _equipItemButton;
    private FramedButton _equipRightHoldableButton;
    private FramedButton _equipLeftHoldableButton;
    private FramedButton _takeOffItemButton;

    private FramedButton _dropItemButton;
    private FramedButton _dropAllItemsButton;
    private FramedButton _checkScrollButton;

    public PlayerInventoryView(IInventoryImagesFactory inventoryImagesFactory)
        : base(inventoryImagesFactory)
    {
    }

    protected override string InventoryName => GetTitleWithWeight(Player);

    private static string GetTitleWithWeight(IPlayer player)
    {
        const double kgWeightMultiplier = 1000d;

        var currentWeight = player.Inventory.GetWeight() / kgWeightMultiplier;
        var maxWeight = player.MaxCarryWeight / kgWeightMultiplier;
        return $"{Title} [Weight: {currentWeight:F2} / {maxWeight:F2}]";
    }

    public override void Initialize()
    {
        base.Initialize();

        _checkScrollButton = new FramedButton(new Rectangle(Width - 30, 40, 20, 3))
        {
            Text = "[C] Check Scroll"
        };
        _checkScrollButton.Click += (_, _) => CheckScroll?.Invoke(this, EventArgs.Empty);
        Controls.Add(_checkScrollButton);

        _useItemButton = new FramedButton(new Rectangle(Width - 52, 40, 20, 3))
        {
            Text = "[U] Use"
        };
        _useItemButton.Click += (_, _) => UseItem?.Invoke(this, EventArgs.Empty);
        Controls.Add(_useItemButton);

        _equipItemButton = new FramedButton(new Rectangle(Width - 52, 40, 20, 3))
        {
            Text = "[E] Equip"
        };
        _equipItemButton.Click += (_, _) => EquipItem?.Invoke(this, EventArgs.Empty);
        Controls.Add(_equipItemButton);

        _equipLeftHoldableButton = new FramedButton(new Rectangle(Width - 52, 40, 20, 3))
        {
            Text = "[Z] Equip Left"
        };
        _equipLeftHoldableButton.Click += (_, _) => EquipHoldableItemLeft?.Invoke(this, EventArgs.Empty);
        Controls.Add(_equipLeftHoldableButton);

        _equipRightHoldableButton = new FramedButton(new Rectangle(Width - 31, 40, 20, 3))
        {
            Text = "[X] Equip Right"
        };
        _equipRightHoldableButton.Click += (_, _) => EquipHoldableItemRight?.Invoke(this, EventArgs.Empty);
        Controls.Add(_equipRightHoldableButton);

        _takeOffItemButton = new FramedButton(new Rectangle(Width - 52, 40, 20, 3))
        {
            Text = "[T] Take Off"
        };
        _takeOffItemButton.Click += (_, _) => TakeOffItem?.Invoke(this, EventArgs.Empty);
        Controls.Add(_takeOffItemButton);

        _dropItemButton = new FramedButton(new Rectangle(Width - 52, 43, 20, 3))
        {
            Text = "[D] Drop"
        };
        _dropItemButton.Click += (_, _) => DropItem?.Invoke(this, EventArgs.Empty);
        Controls.Add(_dropItemButton);

        _dropAllItemsButton = new FramedButton(new Rectangle(Width - 52, 46, 20, 3))
        {
            Text = "[A] Drop All"
        };
        _dropAllItemsButton.Click += (_, _) => DropStack?.Invoke(this, EventArgs.Empty);
        Controls.Add(_dropAllItemsButton);

        RefreshItems(false);
    }

    protected override void ProcessSelectedItemChanged()
    {
        base.ProcessSelectedItemChanged();

        RefreshSelectedItemButtons();
    }

    private void RefreshSelectedItemButtons()
    {
        _dropItemButton.Visible = SelectedStack != null;
        _dropAllItemsButton.Visible = SelectedStack != null;

        _useItemButton.Visible = SelectedStack?.TopItem is IUsableItem;
        _checkScrollButton.Visible = SelectedStack?.TopItem is ScrollBase;

        if (SelectedStack?.TopItem is IEquipableItem equipable)
        {
            var isEquipped = Player.Equipment.IsEquiped(equipable);
            _takeOffItemButton.Visible = isEquipped;

            var isHoldable = equipable is IHoldableItem;

            _equipItemButton.Visible = !isEquipped && !isHoldable;
            _equipLeftHoldableButton.Visible = !isEquipped && isHoldable;
            _equipRightHoldableButton.Visible = !isEquipped && isHoldable;
        }
        else
        {
            _takeOffItemButton.Visible = false;
            _equipItemButton.Visible = false;
            _equipLeftHoldableButton.Visible = false;
            _equipRightHoldableButton.Visible = false;
        }
    }

    public override bool ProcessKeysPressed(Keys[] keys)
    {
        if (keys.Length == 1)
        {
            switch (keys[0])
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
        }
        return base.ProcessKeysPressed(keys);
    }

    protected override InventoryStackItem CreateListBoxItem(InventoryStack stack)
    {
        return new PlayerInventoryItem(stack, Player);
    }

    public event EventHandler UseItem;
    public event EventHandler EquipItem;
    public event EventHandler EquipHoldableItemLeft;
    public event EventHandler EquipHoldableItemRight;
    public event EventHandler DropItem;
    public event EventHandler DropStack;
    public event EventHandler TakeOffItem;
    public event EventHandler CheckScroll;
}

public class PlayerInventoryItem : InventoryStackItem
{
    private const string EquippedText = "[Eq]";
    private const string EquippedLeftText = "[EL]";
    private const string EquippedRightText = "[ER]";
    private static readonly Color EquippedTextColor = Color.Red;

    private readonly Player _player;

    public PlayerInventoryItem(InventoryStack itemStack, Player player)
        : base(itemStack)
    {
        this._player = player;
    }

    protected override ColoredString[] GetAfterNameText(Color backColor)
    {
        var result = new List<ColoredString>();

        if (Stack.TopItem.Stackable)
        {
            result.Add(new ColoredString($" ({Stack.Count})", StackCountColor, backColor));
        }

        var equippedText = GetEquippedText(_player, Stack);
        if (!string.IsNullOrEmpty(equippedText))
        {
            result.Add(new ColoredString($" {equippedText}", EquippedTextColor, backColor));
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
