using System;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Controls
{
    public class StandardButton : Button
    {
        private static readonly Color DefaultBackground = Color.Black;

        private static readonly ButtonTheme NormalButtonTheme = new ButtonLinesTheme
        {
            Colors = new Colors
            {
                Appearance_ControlNormal = new Cell(Color.White, DefaultBackground),
                Appearance_ControlDisabled = new Cell(Color.Gray, DefaultBackground)
            },
        };

        private static readonly ButtonTheme DisabledButtonTheme = new ButtonLinesTheme
        {
            Colors = new Colors
            {
                Appearance_ControlNormal = new Cell(Color.Gray, DefaultBackground),
                Appearance_ControlDisabled = new Cell(Color.Gray, DefaultBackground)
            },
        };

        public StandardButton(int width, int height = 3) : base(width, height)
        {
            Theme = NormalButtonTheme;
            CanFocus = false;
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            var theme = IsEnabled ? NormalButtonTheme : DisabledButtonTheme;
            if (!ReferenceEquals(theme, Theme))
            {
                Theme = theme;
            }
        }
    }
}