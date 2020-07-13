using System;
using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Windows.Controls
{
    public interface IControl
    {
        Rectangle Location { get; set; }

        bool Enabled { get; set; }

        bool Visible { get; set; }

        void Draw(ICellSurface surface);

        void Update(TimeSpan elapsedTime);

        void ProcessMouse(IMouseState mouseState);
    }
}