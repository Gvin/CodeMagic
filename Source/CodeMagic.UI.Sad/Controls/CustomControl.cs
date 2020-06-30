using SadConsole;
using SadConsole.UI.Controls;
using SadConsole.UI.Themes;
using SadRogue.Primitives;

namespace CodeMagic.UI.Sad.Controls
{
    public abstract class CustomControl : ControlBase
    {
        protected CustomControl(int width, int height) 
            : base(width, height)
        {
            ThemeColors = new Colors
            {
                Appearance_ControlNormal = new ColoredGlyph(Color.White, Color.Black),
                Appearance_ControlDisabled = new ColoredGlyph(Color.White, Color.Black)
            };
            Theme = new DrawingSurfaceTheme
            {
                Normal = new ColoredGlyph(Color.White, Color.Black)
            };
        }
    }
}