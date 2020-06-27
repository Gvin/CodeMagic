using Microsoft.Xna.Framework;
using SadConsole;

namespace CodeMagic.UI.Sad.Common
{
    public interface IVisualControl
    {
        Rectangle Position { get; }

        bool IsVisible { get; set; }

        void Draw(CellSurface surface);
    }
}