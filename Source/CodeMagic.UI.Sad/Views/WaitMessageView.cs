using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using SadConsole;

namespace CodeMagic.UI.Sad.Views
{
    public class WaitMessageView : GameViewBase, IWaitMessageView
    {
        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            surface.Print(5, 5, Message);
        }

        public void Close()
        {
            Close(DialogResult.None);
        }

        public string Message { private get; set; }
    }
}