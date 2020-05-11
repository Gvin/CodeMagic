using System;
using System.Threading.Tasks;

namespace CodeMagic.UI.Sad.Views
{
    public class WaitMessageView : View
    {
        private readonly Action waitAction;

        public WaitMessageView(string message, Action waitAction)
        {
            this.waitAction = waitAction;

            Print(5, 5, message);
        }

        protected override void OnShown()
        {
            base.OnShown();

            Task.Run(() =>
            {
                waitAction();
                Close();
            });
        }
    }
}