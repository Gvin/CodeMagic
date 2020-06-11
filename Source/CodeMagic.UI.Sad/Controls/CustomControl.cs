using System;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Controls
{
    public abstract class CustomControl : ControlBase
    {
        protected CustomControl(int width, int height) 
            : base(width, height)
        {
            ThemeColors = new Colors
            {
                Appearance_ControlNormal = new Cell(Color.White, Color.Black),
                Appearance_ControlDisabled = new Cell(Color.White, Color.Black)
            };
            Theme = new DrawingSurfaceTheme
            {
                Normal = new Cell(Color.White, Color.Black)
            };
        }
    }
}