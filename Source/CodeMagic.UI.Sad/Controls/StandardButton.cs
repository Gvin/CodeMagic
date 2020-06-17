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

        static StandardButton()
        {
            var theme = new ButtonLinesTheme
            {
                Disabled = new Cell(Color.Gray, DefaultBackground),
                Normal = new Cell(Color.White, DefaultBackground)
            };
            Library.Default.SetControlTheme(typeof(StandardButton), theme);
        }

        public StandardButton(int width, int height = 3) : base(width, height)
        {
            CanFocus = false;
            EnabledColor = Color.White;
            DisabledColor = Color.Gray;
        }

        public Color EnabledColor { get; set; }

        public Color DisabledColor { get; set; }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            if (IsEnabled)
            {
                ThemeColors = new Colors
                {
                    Appearance_ControlNormal = new Cell(EnabledColor, DefaultBackground),
                    Appearance_ControlDisabled = new Cell(DisabledColor, DefaultBackground)
                };
            }
            else
            {
                ThemeColors = new Colors
                {
                    Appearance_ControlNormal = new Cell(DisabledColor, DefaultBackground),
                    Appearance_ControlDisabled = new Cell(DisabledColor, DefaultBackground),
                    Appearance_ControlOver = new Cell(DisabledColor, DefaultBackground)
                };
            }
        }
    }
}