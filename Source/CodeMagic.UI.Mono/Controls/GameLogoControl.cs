using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Controls
{
    public class GameLogoControl : Control
    {
        private const int Width = 16;
        private const int Height = 2;

        public GameLogoControl(int x, int y)
            : base(new Rectangle(x, y, Width, Height))
        {
        }

        public override void Draw(ICellSurface surface)
        {
            surface.Write(0, 0, "<- C0de Mag1c ->", Color.BlueViolet);

            surface.Write(4, +1, "│", Color.Red);
            surface.Write(6, 1, "'", Color.Red);
            surface.Write(10, 1, "│", Color.Red);
            surface.Write(12, 1,"`", Color.Red);
        }
    }
}