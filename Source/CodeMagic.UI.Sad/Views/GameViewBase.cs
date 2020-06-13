using CodeMagic.Core.Logging;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Drawing;
using ILog = CodeMagic.UI.Sad.Common.ILog;

namespace CodeMagic.UI.Sad.Views
{
    public abstract class GameViewBase : View
    {
        protected GameViewBase(FontTarget font = FontTarget.Interface) 
            : this(
                (ILog) LogManager.GetLog<GameViewBase>(), 
                font)
        {
        }

        protected GameViewBase(ILog log, FontTarget font = FontTarget.Interface)
            : base(
                log,
                FontProvider.GetScreenWidth(font),
                FontProvider.GetScreenHeight(font),
                FontProvider.GetFont(font))
        {
        }
    }
}