using CodeMagic.UI.Presenters;

namespace CodeMagic.UI
{
    public interface IApplicationController
    {
        TPresenter CreatePresenter<TPresenter>() where TPresenter : class, IPresenter;
    }
}