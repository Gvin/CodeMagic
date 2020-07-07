using System;
using CodeMagic.Game.Spells;
using CodeMagic.UI.Services;

namespace CodeMagic.UI.Presenters
{
    public interface IEditSpellView : IView
    {
        event EventHandler Ok;
        event EventHandler Cancel;
        event EventHandler LaunchEditor;

        string SpellName { get; set; }

        int ManaCost { get; set; }
    }

    public class EditSpellPresenter : IPresenter
    {
        private readonly IEditSpellView view;
        private readonly IEditSpellService editSpellService;

        private Action<bool, BookSpell> callback;

        private string spellFilePath;

        public EditSpellPresenter(
            IEditSpellView view,
            IEditSpellService editSpellService)
        {
            this.view = view;
            this.editSpellService = editSpellService;

            this.view.Cancel += View_Cancel;
            this.view.Ok += View_Ok;
            this.view.LaunchEditor += View_LaunchEditor;
        }

        private void View_LaunchEditor(object sender, EventArgs e)
        {
            editSpellService.LaunchSpellFileEditor(spellFilePath);
        }

        private void View_Ok(object sender, EventArgs e)
        {
            view.Close();

            var code = editSpellService.ReadSpellCodeFromFile(spellFilePath);
            var spell = new BookSpell
            {
                Name = view.SpellName,
                ManaCost = view.ManaCost,
                Code = code
            };

            callback(true, spell);
        }

        private void View_Cancel(object sender, EventArgs e)
        {
            view.Close();
            callback(false, null);
        }

        public void Run(Action<bool, BookSpell> callbackOnExit)
        {
            view.SpellName = string.Empty;
            view.ManaCost = 0;
            callback = callbackOnExit;

            spellFilePath = editSpellService.PrepareSpellTemplate(null);

            view.Show();
        }

        public void Run(BookSpell spellToEdit, Action<bool, BookSpell> callbackOnExit)
        {
            view.SpellName = spellToEdit.Name;
            view.ManaCost = spellToEdit.ManaCost;
            callback = callbackOnExit;

            spellFilePath = editSpellService.PrepareSpellTemplate(spellToEdit.Code);

            view.Show();
        }
    }
}