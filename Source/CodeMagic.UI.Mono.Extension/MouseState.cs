using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension
{
    internal class MouseState : IMouseState
    {
        public MouseState(Point position, bool leftButtonState, bool rightButtonState, int scrollChange)
        {
            Position = position;
            LeftButtonState = leftButtonState;
            RightButtonState = rightButtonState;
            ScrollChange = scrollChange;
        }

        public Point Position { get; }

        public bool LeftButtonState { get; }

        public bool RightButtonState { get; }

        public int ScrollChange { get; }
    }
}