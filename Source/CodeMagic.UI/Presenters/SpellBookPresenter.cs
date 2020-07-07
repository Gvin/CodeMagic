using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Materials;
using CodeMagic.Game.Items.Usable;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.PlayerActions;
using CodeMagic.Game.Spells;

namespace CodeMagic.UI.Presenters
{
    public interface ISpellBookView : IView
    {
        event EventHandler Exit;
        event EventHandler SaveToLibrary;
        event EventHandler LoadFromLibrary;
        event EventHandler RemoveSpell;
        event EventHandler CastSpell;
        event EventHandler ScribeSpell;
        event EventHandler EditSpell;

        BookSpell SelectedSpell { get; }

        int SelectedSpellIndex { get; }

        SpellBook SpellBook { set; }

        int? PlayerMana { set; }

        bool CanScribe { set; }

        void RefreshSpells();

        void Initialize();
    }

    public class SpellBookPresenter : IPresenter
    {
        private const string DefaultSpellName = "New Spell";

        private readonly ISpellBookView view;
        private readonly IApplicationController controller;
        private readonly ISpellsLibraryService spellsLibraryService;
        private GameCore<Player> game;

        public SpellBookPresenter(ISpellBookView view, IApplicationController controller, ISpellsLibraryService spellsLibraryService)
        {
            this.view = view;
            this.controller = controller;
            this.spellsLibraryService = spellsLibraryService;

            this.view.Exit += View_Exit;
            this.view.SaveToLibrary += View_SaveToLibrary;
            this.view.RemoveSpell += View_RemoveSpell;
            this.view.CastSpell += View_CastSpell;
            this.view.ScribeSpell += View_ScribeSpell;
            this.view.EditSpell += View_EditSpell;
            this.view.LoadFromLibrary += View_LoadFromLibrary;
        }

        private void View_LoadFromLibrary(object sender, EventArgs e)
        {
            controller.CreatePresenter<LoadSpellPresenter>().Run(((result, spell) =>
            {
                if (!result)
                    return;

                game.Player.Equipment.SpellBook.Spells[view.SelectedSpellIndex] = spell;
                view.RefreshSpells();
            }));
        }

        private void View_EditSpell(object sender, EventArgs e)
        {
            var editSpellPresenter = controller.CreatePresenter<EditSpellPresenter>();

            void EditCallback(bool result, BookSpell spell)
            {
                if (!result)
                    return;

                spell.Name = string.IsNullOrEmpty(spell.Name) ? DefaultSpellName : spell.Name;
                game.Player.Equipment.SpellBook.Spells[view.SelectedSpellIndex] = spell;
                view.RefreshSpells();
            }

            if (view.SelectedSpell == null)
            {
                editSpellPresenter.Run(EditCallback);
            }
            else
            {
                editSpellPresenter.Run(view.SelectedSpell, EditCallback);
            }
        }

        private void View_ScribeSpell(object sender, EventArgs e)
        {
            if (view.SelectedSpell == null)
                return;

            var blankScroll = game.Player.Inventory.GetItem(BlankScroll.ItemKey);
            if (blankScroll == null)
                return;

            var scrollCreationCost = view.SelectedSpell.ManaCost * 2;
            if (game.Player.Mana < scrollCreationCost)
            {
                game.Journal.Write(new NotEnoughManaToScrollMessage());
                return;
            }

            game.Player.Mana -= scrollCreationCost;

            game.Player.Inventory.RemoveItem(blankScroll);
            var newScroll = new Scroll(new ScrollItemConfiguration
            {
                Name = $"{view.SelectedSpell.Name} Scroll ({view.SelectedSpell.ManaCost})",
                Key = Guid.NewGuid().ToString(),
                Weight = 1,
                Code = view.SelectedSpell.Code,
                SpellName = view.SelectedSpell.Name,
                Mana = view.SelectedSpell.ManaCost,
                Rareness = ItemRareness.Uncommon
            });
            game.Player.Inventory.AddItem(newScroll);

            view.Close();
            game.PerformPlayerAction(new EmptyPlayerAction());
        }

        private void View_CastSpell(object sender, EventArgs e)
        {
            if (view.SelectedSpell == null)
                return;

            view.Close();
            game.PerformPlayerAction(new CastSpellPlayerAction(view.SelectedSpell));
        }

        private void View_RemoveSpell(object sender, EventArgs e)
        {
            if (view.SelectedSpell != null)
                return;

            game.Player.Equipment.SpellBook.Spells[view.SelectedSpellIndex] = null;
            view.RefreshSpells();
        }

        private void View_SaveToLibrary(object sender, EventArgs e)
        {
            if (view.SelectedSpell == null)
                return;

            spellsLibraryService.SaveSpell(view.SelectedSpell);
        }

        private void View_Exit(object sender, EventArgs e)
        {
            view.Close();
        }

        public void Run(GameCore<Player> currentGame)
        {
            game = currentGame;
            view.PlayerMana = game.Player.Mana;
            view.SpellBook = game.Player.Equipment.SpellBook;
            view.CanScribe = game.Player.Inventory.Contains(BlankScroll.ItemKey);

            view.Initialize();

            view.Show();
        }
    }
}