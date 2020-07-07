using System;
using CodeMagic.Game.Spells;

namespace CodeMagic.UI.Presenters
{
    public interface ILoadSpellView : ISpellLibraryView
    {
        event EventHandler Ok;
    }

    public class LoadSpellPresenter : SpellLibraryPresenterBase
    {
        private Action<bool, BookSpell> callback;

        public LoadSpellPresenter(ILoadSpellView view, ISpellsLibraryService libraryService, IApplicationController controller)
            : base(view, libraryService, controller)
        {
            view.Ok += View_Ok;
        }

        private void View_Ok(object sender, EventArgs e)
        {
            View.Close();

            callback?.Invoke(true, View.SelectedSpell);
        }

        protected override void Exit()
        {
            View.Close();

            callback?.Invoke(false, null);
        }

        public void Run(Action<bool, BookSpell> callbackAction)
        {
            callback = callbackAction;

            View.Show();
        }
    }
}