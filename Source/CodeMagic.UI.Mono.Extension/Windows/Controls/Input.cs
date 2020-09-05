using System;
using System.Text.RegularExpressions;
using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Extension.Windows.Controls
{
    public class Input : Control
    {
        private static readonly TimeSpan CursorBlinkTime = TimeSpan.FromMilliseconds(500);

        private string text;
        private int cursorPosition;
        private bool drawCursor;
        private TimeSpan cursorPhaseTime;

        public Input(Rectangle location)
            : base(location)
        {
            Location = location;
            cursorPosition = 0;
            Theme = new InputTheme();
            cursorPhaseTime = TimeSpan.Zero;
            drawCursor = true;

            text = string.Empty;
        }

        public string Text
        {
            get => text;
            set
            {
                text = value ?? string.Empty;
                cursorPosition = Math.Max(0, text.Length);
            }
        }

        public string FilterRegex { get; set; }

        public InputTheme Theme { get; set; }

        public override bool CanFocus => Enabled;

        public override void Draw(ICellSurface surface)
        {
            var textToDraw = text;
            if (Focused)
            {
                var cursorSymbol = drawCursor ? "│" : " ";
                textToDraw = textToDraw.Insert(cursorPosition, cursorSymbol);
            }
            textToDraw = textToDraw.Substring(0, Math.Min(Location.Width, textToDraw.Length));
            while (textToDraw.Length < Location.Width)
            {
                textToDraw += " ";
            }

            var themeColors = GetThemeColors();

            surface.Write(0, 0, textToDraw, themeColors.TextColor.ForeColor, themeColors.TextColor.BackColor);
        }

        private InputThemeColors GetThemeColors()
        {
            if (!Enabled)
                return Theme.Disabled;

            if (Focused)
                return Theme.Focused;

            return Theme.Enabled;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            cursorPhaseTime += elapsedTime;
            if (cursorPhaseTime > CursorBlinkTime)
            {
                drawCursor = !drawCursor;
                cursorPhaseTime = TimeSpan.Zero;
            }
        }

        public override void ProcessTextInput(char symbol)
        {
            base.ProcessTextInput(symbol);

            var keyboardState = Keyboard.GetState();
            var isShift = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift);

            var textToAdd = string.Empty + symbol;
            if (isShift)
                textToAdd = textToAdd.ToUpper();

            var newText = text.Insert(cursorPosition, textToAdd);
            if (IsTextAllowed(newText))
            {
                text = newText;
                cursorPosition++;
            }
        }

        public override void ProcessKeysPressed(Keys[] keys)
        {
            if (keys.Length == 1)
            {
                if (keys[0] == Keys.Left)
                {
                    cursorPosition = Math.Max(0, cursorPosition - 1);
                    return;
                }

                if (keys[0] == Keys.Right)
                {
                    cursorPosition = Math.Min(text.Length, cursorPosition + 1);
                    return;
                }

                if (keys[0] == Keys.Back && Text.Length > 0 && cursorPosition > 0)
                {
                    text = text.Remove(cursorPosition - 1, 1);
                    cursorPosition = Math.Max(0, cursorPosition - 1);
                    return;
                }

                if (keys[0] == Keys.Delete && Text.Length > 0 && cursorPosition < Text.Length)
                {
                    text = text.Remove(cursorPosition, 1);
                    return;
                }
            }
        }

        private bool IsTextAllowed(string newText)
        {
            if (string.IsNullOrEmpty(FilterRegex))
                return true;

            var matches = Regex.Matches(newText, FilterRegex);
            if (matches.Count != 1)
                return false;

            return string.Equals(matches[0].Value, newText);
        }

        public override bool ProcessMouse(IMouseState mouseState)
        {
            if (!Location.Contains(mouseState.Position))
                return false;

            return true;
        }
    }

    public class InputTheme
    {
        public InputTheme()
        {
            var backColor = Color.FromNonPremultiplied(61, 61, 61, 255);

            Enabled = new InputThemeColors
            {
                TextColor = new Cell(foreColor: Color.White, backColor: backColor)
            };

            Focused = new InputThemeColors
            {
                TextColor = new Cell(foreColor: Color.Lime, backColor: backColor)
            };

            Disabled = new InputThemeColors
            {
                TextColor = new Cell(foreColor: Color.DarkGray, backColor: backColor)
            };
        }

        public InputThemeColors Enabled { get; set; }

        public InputThemeColors Focused { get; set; }

        public InputThemeColors Disabled { get; set; }
    }

    public class InputThemeColors
    {
        public Cell TextColor { get; set; }
    }
}