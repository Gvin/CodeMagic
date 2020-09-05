using System;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Fonts;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Windows
{
    public interface IActivePlane
    {
        Point Position { get; set; }

        int PixelWidth { get; }

        int PixelHeight { get; }

        int Width { get; }

        int Height { get; }

        IFont Font { get; }

        bool Enabled { get; set; }

        bool Visible { get; set; }

        void Draw(ICellSurface surface);

        void Update(TimeSpan elapsedTime);

        bool ProcessMouse(IMouseState mouseState);
    }
}