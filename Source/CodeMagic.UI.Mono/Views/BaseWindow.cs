using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows;
using CodeMagic.UI.Mono.Fonts;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Views
{
    public class BaseWindow : Window
    {
        public static Color FrameColor = Color.Gray;

        public BaseWindow(FontTarget font) 
            : base(
                new Point(0, 0), 
                FontProvider.GetScreenWidthSymbols(font), 
                FontProvider.GetScreenHeightSymbols(font), 
                FontProvider.Instance.GetFont(font))
        {
        }

        public override void Draw(ICellSurface surface)
        {
            base.Draw(surface);

            surface.Fill(new Rectangle(1, 0, Width - 2, 1), new Cell('═', FrameColor));
            surface.Fill(new Rectangle(1, Height - 1, Width - 2, 1), new Cell('═', FrameColor));
            surface.Fill(new Rectangle(0, 1, 1, Height - 2), new Cell('║', FrameColor));
            surface.Fill(new Rectangle(Width - 1, 1, 1, Height - 2), new Cell('║', FrameColor));

            surface.SetCell(0, 0, '╔', FrameColor);
            surface.SetCell(Width - 1, 0, '╗', FrameColor);
            surface.SetCell(0, Height - 1, '╚', FrameColor);
            surface.SetCell(Width - 1, Height - 1, '╝', FrameColor);
        }
    }
}