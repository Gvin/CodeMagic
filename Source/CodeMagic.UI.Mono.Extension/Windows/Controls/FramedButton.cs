using System;
using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Windows.Controls
{
    public class FramedButton : ButtonBase
    {
        private bool isHover;

        public FramedButton(Rectangle location) 
            : base(location)
        {
            isHover = false;
            Theme = new FramedButtonTheme();
        }

        public FramedButtonTheme Theme { get; set; }

        public override void Draw(ICellSurface surface)
        {
            var colors = GetColors();

            surface.Fill(new Rectangle(0, 0, Location.Width, Location.Height), new Cell(' ', colors.Fill.Fore, colors.Fill.Back));

            DrawFrame(surface, colors);
            DrawText(surface, colors);
        }

        private void DrawText(ICellSurface surface, FramedButtonThemeColors colors)
        {
            var textY = (int) Math.Floor(Location.Height / 2d);
            var textX = (int) Math.Floor((Location.Width - Text.Length) / 2d);

            var textToPrint = Text;
            if (textToPrint.Length > Location.Width - 2)
            {
                textToPrint = textToPrint.Substring(0, Location.Width - 2);
            }
            surface.Write(textX, textY, textToPrint, colors.Text.Fore, colors.Text.Back);
        }

        private void DrawFrame(ICellSurface surface, FramedButtonThemeColors colors)
        {
            surface.Write(0, 0, "┌", colors.FrameTop.Fore, colors.FrameTop.Back);
            surface.Fill(new Rectangle(1, 0, Location.Width - 2, 1),
                new Cell('─', colors.FrameTop.Fore, colors.FrameTop.Back));
            surface.Fill(new Rectangle(0, 1, 1, Location.Height - 2),
                new Cell('│', colors.FrameTop.Fore, colors.FrameTop.Back));

            surface.Write(0, Location.Height - 1, "└", colors.FrameBottom.Fore, colors.FrameBottom.Back);
            surface.Fill(new Rectangle(1, Location.Height - 1, Location.Width - 2, 1),
                new Cell('─', colors.FrameBottom.Fore, colors.FrameBottom.Back));
            surface.Write(Location.Width - 1, Location.Height - 1, "┘", colors.FrameBottom.Fore, colors.FrameBottom.Back);
            surface.Fill(new Rectangle(Location.Width - 1, 1, 1, Location.Height - 2),
                new Cell('│', colors.FrameBottom.Fore, colors.FrameBottom.Back));
            surface.Write(Location.Width - 1, 0, "┐", colors.FrameBottom.Fore, colors.FrameBottom.Back);
        }

        private FramedButtonThemeColors GetColors()
        {
            if (!Enabled)
                return Theme.Disabled;

            if (IsHoldingClick)
                return Theme.Push;

            if (isHover)
                return Theme.Hover;

            return Theme.Enabled;
        }

        public override void ProcessMouse(IMouseState mouseState)
        {
            base.ProcessMouse(mouseState);

            isHover = Location.Contains(mouseState.Position);
        }
    }

    public class FramedButtonTheme
    {
        public FramedButtonTheme()
        {
            Enabled = new FramedButtonThemeColors
            {
                Text = new ColorsPalette(Color.White),
                Fill = new ColorsPalette(),
                FrameTop = new ColorsPalette(Color.FromNonPremultiplied(176, 196, 222, 255)),
                FrameBottom = new ColorsPalette(Color.FromNonPremultiplied(66, 66, 66, 255))
            };
            Disabled = new FramedButtonThemeColors
            {
                Text = new ColorsPalette(Color.DarkGray),
                Fill = new ColorsPalette(),
                FrameTop = new ColorsPalette(Color.FromNonPremultiplied(176, 196, 222, 255)),
                FrameBottom = new ColorsPalette(Color.FromNonPremultiplied(66, 66, 66, 255))
            };
            Hover = new FramedButtonThemeColors
            {
                Text = new ColorsPalette(Color.Lime),
                Fill = new ColorsPalette(),
                FrameTop = new ColorsPalette(Color.FromNonPremultiplied(176, 196, 222, 255)),
                FrameBottom = new ColorsPalette(Color.FromNonPremultiplied(66, 66, 66, 255))
            };
            Push = new FramedButtonThemeColors
            {
                Text = new ColorsPalette(Color.Lime),
                Fill = new ColorsPalette(),
                FrameTop = new ColorsPalette(Color.FromNonPremultiplied(66, 66, 66, 255)),
                FrameBottom = new ColorsPalette(Color.FromNonPremultiplied(176, 196, 222, 255))
            };
        }

        public FramedButtonThemeColors Enabled { get; set; }

        public FramedButtonThemeColors Disabled { get; set; }

        public FramedButtonThemeColors Hover { get; set; }

        public FramedButtonThemeColors Push { get; set; }

        public FramedButtonTheme Clone()
        {
            return new FramedButtonTheme
            {
                Enabled = Enabled.Clone(),
                Disabled = Disabled.Clone(),
                Hover = Hover.Clone()
            };
        }
    }

    public class FramedButtonThemeColors
    {
        public ColorsPalette FrameTop { get; set; }

        public ColorsPalette FrameBottom { get; set; }

        public ColorsPalette Fill { get; set; }

        public ColorsPalette Text { get; set; }

        public FramedButtonThemeColors Clone()
        {
            return new FramedButtonThemeColors
            {
                FrameTop = FrameTop.Clone(),
                FrameBottom = FrameBottom.Clone(),
                Fill = Fill.Clone(),
                Text = Text.Clone()
            };
        }
    }
}