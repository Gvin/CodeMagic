using SadConsole;
using SadRogue.Primitives;

namespace CodeMagic.UI.Sad.Common
{
    public interface IVisualControl
    {
        Rectangle Position { get; }

        bool IsVisible { get; set; }

        void Draw(ICellSurface surface);
    }
}