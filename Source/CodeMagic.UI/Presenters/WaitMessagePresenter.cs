using System;
using System.Threading.Tasks;

namespace CodeMagic.UI.Presenters
{
    public interface IWaitMessageView : IView
    {
        string Message { set; }
    }

    public class WaitMessagePresenter : IPresenter
    {
        private readonly IWaitMessageView view;

        public WaitMessagePresenter(IWaitMessageView view)
        {
            this.view = view;
        }

        public void Run(string message, Action waitAction)
        {
            view.Message = message;
            view.Show();

            Task.Run(() =>
            {
                waitAction();
                view.Close();
            });
        }
    }
}