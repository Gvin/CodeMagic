using System;
using System.Threading.Tasks;
using SadConsole;

namespace CodeMagic.UI.Sad.Views
{
    public class WaitMessageView : GameViewBase
    {
        private readonly Action waitAction;
        private readonly string message;

        public WaitMessageView(string message, Action waitAction)
        {
            this.waitAction = waitAction;
            this.message = message;
        }

        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            surface.Print(5, 5, message);
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