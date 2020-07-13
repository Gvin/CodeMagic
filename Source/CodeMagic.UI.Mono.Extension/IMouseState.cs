using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension
{
    public interface IMouseState
    {
        Point Position { get; }

        bool LeftButtonState { get; }

        bool RightButtonState { get; }

        int ScrollChange { get; }
    }
}