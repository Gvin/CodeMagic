using CodeMagic.UI.Services;

namespace CodeMagic.UI.Presenters
{
    public interface IMainSpellsLibraryView : ISpellLibraryView
    {
    }

    public class MainSpellsLibraryPresenter : SpellLibraryPresenterBase
    {
        public MainSpellsLibraryPresenter(
            IMainSpellsLibraryView view, 
            ISpellsLibraryService libraryService, 
            IApplicationController controller)
            : base(view, libraryService, controller)
        {
        }

        protected override void Exit()
        {
            View.Close();
        }

        public void Run()
        {
            View.Show();
        }
    }
}