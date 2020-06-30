using System;
using SadConsole;
using SadConsole.UI.Controls;
using SadConsole.UI.Themes;
using SadRogue.Primitives;

namespace CodeMagic.UI.Sad.Controls
{
    public class StandardButton : Button
    {
        private static readonly Color DefaultBackground = Color.Black;

        static StandardButton()
        {
            var theme = new ButtonLinesTheme
            {
                Disabled = new ColoredGlyph(Color.Gray, DefaultBackground),
                Normal = new ColoredGlyph(Color.White, DefaultBackground)
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
                    Appearance_ControlNormal = new ColoredGlyph(EnabledColor, DefaultBackground),
                    Appearance_ControlDisabled = new ColoredGlyph(DisabledColor, DefaultBackground)
                };
            }
            else
            {
                ThemeColors = new Colors
                {
                    Appearance_ControlNormal = new ColoredGlyph(DisabledColor, DefaultBackground),
                    Appearance_ControlDisabled = new ColoredGlyph(DisabledColor, DefaultBackground),
                    Appearance_ControlOver = new ColoredGlyph(DisabledColor, DefaultBackground)
                };
            }
        }
    }
}