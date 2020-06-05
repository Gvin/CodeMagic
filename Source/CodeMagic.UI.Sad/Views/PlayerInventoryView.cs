using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Usable;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.PlayerActions;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.GameProcess;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Input;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class PlayerInventoryView : InventoryViewBase
    {
        private const string Title = "Player Inventory";

        private readonly GameCore<Player> game;

        private StandardButton useItemButton;

        private StandardButton equipItemButton;
        private StandardButton equipRightWeaponButton;
        private StandardButton equipLeftWeaponButton;
        private StandardButton takeOffItemButton;

        private StandardButton dropItemButton;
        private StandardButton dropAllItemsButton;
        private StandardButton checkScrollButton;

        public PlayerInventoryView(GameCore<Player> game) 
            : base(GetTitleWithWeight(game.Player), game.Player)
        {
            this.game = game;

            InitializeControls();
            RefreshItems(false);
        }

        protected override IEnumerable<InventoryStack> GetStacks()
        {
            return game.Player.Inventory.Stacks.OrderByDescending(stack => PlayerInventoryItem.GetIfEquiped(game.Player, stack));
        }

        private void InitializeControls()
        {
            checkScrollButton = new StandardButton(20)
            {
                Position = new Point(Width - 30, 40),
                Text = "[C] Check Scroll"
            };
            checkScrollButton.Click += (sender, args) => CheckSelectedScrollCode();
            Add(checkScrollButton);

            useItemButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 40),
                Text = "[U] Use"
            };
            useItemButton.Click += (sender, args) => UseSelectedItem();
            Add(useItemButton);

            equipItemButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 40),
                Text = "[E] Equip"
            };
            equipItemButton.Click += (sender, args) => EquipSelectedItem();
            Add(equipItemButton);

            equipLeftWeaponButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 40),
                Text = "[Z] Equip Left"
            };
            equipLeftWeaponButton.Click += (sender, args) => EquipSelectedWeapon(false);
            Add(equipLeftWeaponButton);

            equipRightWeaponButton = new StandardButton(20)
            {
                Position = new Point(Width - 31, 40),
                Text = "[X] Equip Right"
            };
            equipRightWeaponButton.Click += (sender, args) => EquipSelectedWeapon(true);
            Add(equipRightWeaponButton);

            takeOffItemButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 40),
                Text = "[T] Take Off"
            };
            takeOffItemButton.Click += (sender, args) => TakeOffSelectedItem();
            Add(takeOffItemButton);

            dropItemButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 43),
                Text = "[D] Drop"
            };
            dropItemButton.Click += (sender, args) => DropSelectedItem();
            Add(dropItemButton);

            dropAllItemsButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 46),
                Text = "[A] Drop All"
            };
            dropAllItemsButton.Click += (sender, args) => DropAllItemsInStack();
            Add(dropAllItemsButton);
        }

        private void CheckSelectedScrollCode()
        {
            var selectedScroll = SelectedStack?.TopItem as ScrollBase;
            if (selectedScroll == null)
                return;

            var code = selectedScroll.GetSpellCode();
            var filePath = EditSpellHelper.PrepareSpellTemplate(code);
            EditSpellHelper.LaunchSpellFileEditor(filePath);
        }

        private void DropAllItemsInStack()
        {
            if (SelectedStack == null)
                return;

            game.PerformPlayerAction(new DropItemsPlayerAction(SelectedStack.Items));
            Close();
        }

        private void DropSelectedItem()
        {
            if (SelectedStack == null)
                return;

            game.PerformPlayerAction(new DropItemsPlayerAction(SelectedStack.TopItem));
            Close();
        }

        private void TakeOffSelectedItem()
        {
            if (!(SelectedStack?.TopItem is IEquipableItem equipableItem))
                return;

            if (!game.Player.Equipment.IsEquiped(equipableItem))
                return;

            game.PerformPlayerAction(new UnequipItemPlayerAction(equipableItem));
            Close();
        }

        private void EquipSelectedItem()
        {
            if (!(SelectedStack?.TopItem is IEquipableItem equipableItem))
                return;

            if (game.Player.Equipment.IsEquiped(equipableItem))
                return;

            if (equipableItem is WeaponItem)
                return;

            game.PerformPlayerAction(new EquipItemPlayerAction(equipableItem));
            Close();
        }

        private void EquipSelectedWeapon(bool isRight)
        {
            if (!(SelectedStack?.TopItem is WeaponItem weaponItem))
                return;

            if (game.Player.Equipment.IsEquiped(weaponItem))
                return;

            game.PerformPlayerAction(new EquipWeaponPlayerAction(weaponItem, isRight));
            Close();
        }

        private void UseSelectedItem()
        {
            if (!(SelectedStack?.TopItem is IUsableItem usableItem))
                return;

            game.PerformPlayerAction(new UseItemPlayerAction(usableItem));
            Close();
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
                var equiped = game.Player.Equipment.IsEquiped(equipable);
                takeOffItemButton.IsVisible = equiped;

                var isWeapon = equipable is WeaponItem;

                equipItemButton.IsVisible = !equiped && !isWeapon;
                equipLeftWeaponButton.IsVisible = !equiped && isWeapon;
                equipRightWeaponButton.IsVisible = !equiped && isWeapon;
            }
            else
            {
                takeOffItemButton.IsVisible = false;
                equipItemButton.IsVisible = false;
                equipLeftWeaponButton.IsVisible = false;
                equipRightWeaponButton.IsVisible = false;
            }
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
                case Keys.Z:
                    EquipSelectedWeapon(false);
                    return true;
                case Keys.X:
                    EquipSelectedWeapon(true);
                    return true;
                case Keys.T:
                    TakeOffSelectedItem();
                    return true;
                case Keys.D:
                    DropSelectedItem();
                    return true;
                case Keys.A:
                    DropAllItemsInStack();
                    return true;
                case Keys.C:
                    CheckSelectedScrollCode();
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
            return new PlayerInventoryItem(stack, game.Player);
        }
    }

    public class PlayerInventoryItem : InventoryStackItem
    {
        private const string EquipedText = "[Eq]";
        private const string EquipedLeftText = "[EL]";
        private const string EquipedRightText = "[ER]";
        private static readonly Color EquipedTextColor = Color.Red;

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

            var equipedText = GetEquipedText(player, Stack);
            if (!string.IsNullOrEmpty(equipedText))
            {
                result.Add(new ColoredString($" {equipedText}", new Cell(EquipedTextColor, backColor)));
            }

            return result.ToArray();
        }

        private static string GetEquipedText(Player player, InventoryStack stack)
        {
            if (!(stack.TopItem is IEquipableItem equipable))
                return null;

            if (!player.Equipment.IsEquiped(equipable))
                return null;

            if (!(equipable is WeaponItem weapon))
                return EquipedText;

            if (player.Equipment.LeftWeapon.Equals(weapon))
                return EquipedLeftText;

            return EquipedRightText;
        }

        public static bool GetIfEquiped(Player player, InventoryStack stack)
        {
            if (!(stack.TopItem is IEquipableItem equipable))
                return false;

            return player.Equipment.IsEquiped(equipable);
        }
    }
}