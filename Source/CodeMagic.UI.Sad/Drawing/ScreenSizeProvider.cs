using System.Windows.Forms;

namespace CodeMagic.UI.Sad.Drawing
{
    public interface IScreenSizeProvider
    {
        int Width { get; }

        int Height { get; }

        bool FitsScreen(int width, int height);
    }

    public class WinFormsScreenSizeProvider : IScreenSizeProvider
    {
        public int Width => SystemInformation.VirtualScreen.Width;

        public int Height => SystemInformation.VirtualScreen.Height;

        public bool FitsScreen(int width, int height)
        {
            return width < Width && height < Height;
        }
    }
}