using System;
using CodeMagic.UI.Sad.Common;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Controls
{
    public class GameLogoControl : ControlBase
    {
        private static readonly Color DefaultBackground = Color.Black;

        public GameLogoControl() 
            : base(16, 2)
        {
            Theme = new DrawingSurfaceTheme();
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Surface.Print(0, 0, "<- C0de Mag1c ->", Color.BlueViolet);

            Surface.Print(4, +1,
                new ColoredGlyph(Glyphs.GlyphBoxSingleVertical, Color.Red,
                    DefaultBackground));
            Surface.Print(6, 1,
                new ColoredGlyph('\'', Color.Red,
                    DefaultBackground));
            Surface.Print(10, 1,
                new ColoredGlyph(Glyphs.GlyphBoxSingleVertical, Color.Red,
                    DefaultBackground));
            Surface.Print(12, 1,
                new ColoredGlyph('`', Color.Red,
                    DefaultBackground));
        }
    }
}