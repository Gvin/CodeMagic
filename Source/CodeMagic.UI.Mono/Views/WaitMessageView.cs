using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Fonts;
using CodeMagic.UI.Presenters;

namespace CodeMagic.UI.Mono.Views
{
    public class WaitMessageView : BaseWindow, IWaitMessageView
    {
        public WaitMessageView() : base(FontTarget.Interface)
        {
        }

        public string Message { private get; set; }

        public override void Draw(ICellSurface surface)
        {
            base.Draw(surface);

            surface.Write(5, 5, Message);
        }
    }
}