using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Usable;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.PlayerActions;
using CodeMagic.UI.Services;

namespace CodeMagic.UI.Presenters
{
    public interface IPlayerInventoryView : IView
    {
        event EventHandler Exit;
        event EventHandler UseItem;
        event EventHandler EquipItem;
        event EventHandler EquipHoldableItemLeft;
        event EventHandler EquipHoldableItemRight;
        event EventHandler DropItem;
        event EventHandler DropStack;
        event EventHandler TakeOffItem;
        event EventHandler CheckScroll;

        Player Player { set; }

        InventoryStack[] Stacks { set; }

        InventoryStack SelectedStack { get; }

        void Initialize();
    }

    public class PlayerInventoryPresenter : IPresenter
    {
        private readonly IPlayerInventoryView view;
        private readonly IEditSpellService editSpellService;
        private GameCore<Player> game;

        public PlayerInventoryPresenter(IPlayerInventoryView view, IEditSpellService editSpellService)
        {
            this.view = view;
            this.editSpellService = editSpellService;

            this.view.Exit += View_Exit;
            this.view.UseItem += View_UseItem;
            this.view.EquipItem += View_EquipItem;
            this.view.EquipHoldableItemLeft += View_EquipHoldableItemLeft;
            this.view.EquipHoldableItemRight += View_EquipHoldableItemRight;
            this.view.DropItem += View_DropItem;
            this.view.DropStack += View_DropStack;
            this.view.TakeOffItem += View_TakeOffItem;
            this.view.CheckScroll += View_CheckScroll;
        }

        private void View_CheckScroll(object sender, EventArgs e)
        {
            if (!(view.SelectedStack?.TopItem is ScrollBase selectedScroll))
                return;

            var code = selectedScroll.GetSpellCode();
            var filePath = editSpellService.PrepareSpellTemplate(code);
            editSpellService.LaunchSpellFileEditor(filePath);
        }

        private void View_TakeOffItem(object sender, EventArgs e)
        {
            if (!(view.SelectedStack?.TopItem is IEquipableItem equipableItem))
                return;

            if (!game.Player.Equipment.IsEquiped(equipableItem))
                return;

            game.PerformPlayerAction(new UnequipItemPlayerAction(equipableItem));
            view.Close();
        }

        private void View_DropStack(object sender, EventArgs e)
        {
            if (view.SelectedStack == null)
                return;

            game.PerformPlayerAction(new DropItemsPlayerAction(view.SelectedStack.Items));
            view.Close();
        }

        private void View_DropItem(object sender, EventArgs e)
        {
            if (view.SelectedStack == null)
                return;

            game.PerformPlayerAction(new DropItemsPlayerAction(view.SelectedStack.TopItem));
            view.Close();
        }

        private void View_EquipHoldableItemRight(object sender, EventArgs e)
        {
            EquipSelectedHoldable(true);
        }

        private void View_EquipHoldableItemLeft(object sender, EventArgs e)
        {
            EquipSelectedHoldable(false);
        }

        private void EquipSelectedHoldable(bool isRight)
        {
            if (!(view.SelectedStack?.TopItem is IHoldableItem holdableItem))
                return;

            if (game.Player.Equipment.IsEquiped(holdableItem))
                return;

            game.PerformPlayerAction(new EquipHoldablePlayerAction(holdableItem, isRight));
            view.Close();
        }

        private void View_EquipItem(object sender, EventArgs e)
        {
            if (!(view.SelectedStack?.TopItem is IEquipableItem equipableItem))
                return;

            if (game.Player.Equipment.IsEquiped(equipableItem))
                return;

            if (equipableItem is IHoldableItem)
                return;

            game.PerformPlayerAction(new EquipItemPlayerAction(equipableItem));
            view.Close();
        }

        private void View_UseItem(object sender, EventArgs e)
        {
            if (!(view.SelectedStack?.TopItem is IUsableItem usableItem))
                return;

            game.PerformPlayerAction(new UseItemPlayerAction(usableItem));
            view.Close();
        }

        private void View_Exit(object sender, EventArgs e)
        {
            view.Close();
        }

        public void Run(GameCore<Player> currentGame)
        {
            game = currentGame;
            view.Player = game.Player;
            view.Stacks = game.Player.Inventory.Stacks.OrderByDescending(stack => GetIfEquipped(game.Player, stack)).ToArray();

            view.Initialize();
            view.Show();
        }

        private static bool GetIfEquipped(Player player, InventoryStack stack)
        {
            if (!(stack.TopItem is IEquipableItem equipable))
                return false;

            return player.Equipment.IsEquiped(equipable);
        }
    }
}