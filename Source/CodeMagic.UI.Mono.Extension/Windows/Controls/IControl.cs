using System;
using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Extension.Windows.Controls
{
    public interface IControl
    {
        Rectangle Location { get; set; }

        bool Enabled { get; set; }

        bool Visible { get; set; }

        bool CanFocus { get; }

        bool Focused { get; set; }

        void Draw(ICellSurface surface);

        void Update(TimeSpan elapsedTime);

        void ProcessKeysPressed(Keys[] keys);

        void ProcessTextInput(char symbol);

        bool ProcessMouse(IMouseState mouseState);
    }
}