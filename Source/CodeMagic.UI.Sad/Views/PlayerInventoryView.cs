using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Usable;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.GameProcess;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class PlayerInventoryView : InventoryViewBase
    {
        private const string Title = "Player Inventory";

        private readonly IGameCore game;

        private Button useItemButton;
        private Button equipItemButton;
        private Button takeOffItemButton;
        private Button dropItemButton;
        private Button dropAllItemsButton;
        private Button checkScrollButton;

        public PlayerInventoryView(IGameCore game) 
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
            var buttonsTheme = new ButtonLinesTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(Color.White, DefaultBackground)
                }
            };

            checkScrollButton = new Button(20, 3)
            {
                Position = new Point(Width - 35, 40),
                Text = "[C] Check Scroll",
                CanFocus = false,
                Theme = buttonsTheme
            };
            checkScrollButton.Click += (sender, args) => CheckSelectedScrollCode();
            Add(checkScrollButton);

            useItemButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 40),
                Text = "[U] Use",
                CanFocus = false,
                Theme = buttonsTheme
            };
            useItemButton.Click += (sender, args) => UseSelectedItem();
            Add(useItemButton);

            equipItemButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 40),
                Text = "[E] Equip",
                CanFocus = false,
                Theme = buttonsTheme
            };
            equipItemButton.Click += (sender, args) => EquipSelectedItem();
            Add(equipItemButton);

            takeOffItemButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 40),
                Text = "[T] Take Off",
                CanFocus = false,
                Theme = buttonsTheme
            };
            takeOffItemButton.Click += (sender, args) => TakeOffSelectedItem();
            Add(takeOffItemButton);

            dropItemButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 43),
                Text = "[D] Drop",
                CanFocus = false,
                Theme = buttonsTheme
            };
            dropItemButton.Click += (sender, args) => DropSelectedItem();
            Add(dropItemButton);

            dropAllItemsButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 46),
                Text = "[A] Drop All",
                CanFocus = false,
                Theme = buttonsTheme
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

            game.PerformPlayerAction(new EquipItemPlayerAction(equipableItem));
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
                equipItemButton.IsVisible = !equiped;
            }
            else
            {
                takeOffItemButton.IsVisible = false;
                equipItemButton.IsVisible = false;
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
        private static readonly Color EquipedTextColor = Color.Red;


        private readonly IPlayer player;

        public PlayerInventoryItem(InventoryStack itemStack, IPlayer player)
            : base(itemStack)
        {
            this.player = player;
        }

        protected override ColoredString[] GetNameText(Color backColor)
        {
            var itemColor = ItemDrawingHelper.GetItemColor(Stack.TopItem).ToXna();

            return new[]
            {
                new ColoredString(Stack.TopItem.Name.ConvertGlyphs(), new Cell(itemColor, backColor))
            };
        }

        protected override ColoredString[] GetAfterNameText(Color backColor)
        {
            var result = new List<ColoredString>();

            if (Stack.TopItem.Stackable)
            {
                result.Add(new ColoredString($" ({Stack.Count})", new Cell(StackCountColor, backColor)));
            }

            if (GetIfEquiped(player, Stack))
            {
                result.Add(new ColoredString($" {EquipedText}", new Cell(EquipedTextColor, backColor)));
            }

            return result.ToArray();
        }

        public static bool GetIfEquiped(IPlayer player, InventoryStack stack)
        {
            if (!(stack.TopItem is IEquipableItem equipable))
                return false;

            return player.Equipment.IsEquiped(equipable);
        }
    }
}