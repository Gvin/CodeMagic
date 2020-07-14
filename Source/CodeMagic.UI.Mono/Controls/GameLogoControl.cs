using System;
using CodeMagic.UI.Mono.Extension;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Controls
{
    public class GameLogoControl : IControl
    {
        private const int Width = 16;
        private const int Height = 2;

        public GameLogoControl(int x, int y)
        {
            Location = new Rectangle(x, y, Width, Height);

            Enabled = false;
            Visible = true;
        }

        public Rectangle Location { get; set; }

        public bool Enabled { get; set; }

        public bool Visible { get; set; }

        public void Draw(ICellSurface surface)
        {
            surface.Write(0, 0, "<- C0de Mag1c ->", Color.BlueViolet);

            surface.Write(4, +1, "│", Color.Red);
            surface.Write(6, 1, "'", Color.Red);
            surface.Write(10, 1, "│", Color.Red);
            surface.Write(12, 1,"`", Color.Red);
        }

        public void Update(TimeSpan elapsedTime)
        {
            // Do nothing
        }

        public bool ProcessMouse(IMouseState mouseState)
        {
            return false;
        }
    }
}