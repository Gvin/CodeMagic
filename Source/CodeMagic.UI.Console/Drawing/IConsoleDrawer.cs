using System.Drawing;

namespace CodeMagic.UI.Console.Drawing
{
    public interface IConsoleDrawer
    {
        void Draw(SymbolsImage image, Color backgroundColor);
    }
}