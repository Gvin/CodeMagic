using System;
using CodeMagic.Game.Spells;
using CodeMagic.UI.Services;

namespace CodeMagic.UI.Presenters
{
    public interface ISpellLibraryView : IView
    {
        event EventHandler Exit;
        event EventHandler RemoveSpell;
        event EventHandler EditSpell;

        BookSpell SelectedSpell { get; }

        BookSpell[] Spells { get; set; }

        void RefreshSpells(bool keepSelection);
    }

    public abstract class SpellLibraryPresenterBase : IPresenter
    {
        private const string DefaultSpellName = "New Spell";

        protected readonly ISpellLibraryView View;
        private readonly ISpellsLibraryService libraryService;
        private readonly IApplicationController controller;

        protected SpellLibraryPresenterBase(ISpellLibraryView view, ISpellsLibraryService libraryService, IApplicationController controller)
        {
            View = view;
            this.libraryService = libraryService;
            this.controller = controller;

            View.Spells = libraryService.ReadSpells();

            View.Exit += View_Exit;

            View.RemoveSpell += View_RemoveSpell;
            View.EditSpell += View_EditSpell;

            View.RefreshSpells(false);
        }

        private void View_EditSpell(object sender, EventArgs e)
        {
            var selectedSpell = View.SelectedSpell;
            if (selectedSpell == null)
                return;

            var editSpellPresenter = controller.CreatePresenter<EditSpellPresenter>();
            editSpellPresenter.Run(selectedSpell, (result, newSpell) =>
            {
                if (result)
                {
                    newSpell.Name = string.IsNullOrEmpty(newSpell.Name) ? DefaultSpellName : newSpell.Name;
                    libraryService.RemoveSpell(selectedSpell);
                    libraryService.SaveSpell(newSpell);
                    View.RefreshSpells(true);
                }
            });
        }

        private void View_RemoveSpell(object sender, EventArgs e)
        {
            if (View.SelectedSpell == null)
                return;

            libraryService.RemoveSpell(View.SelectedSpell);
            View.RefreshSpells(false);
        }

        private void View_Exit(object sender, EventArgs e)
        {
            Exit();
        }

        protected abstract void Exit();
    }
}