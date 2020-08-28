using System;
using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Windows.Controls
{
    public class VerticalScrollBar : Control
    {
        private bool isHover;
        private int value;
        private int maxValue;
        private int minValue;

        public VerticalScrollBar(Point position, int height)
            : base(new Rectangle(position.X, position.Y, 1, height))
        {
            Theme = new ScrollBarTheme();

            MaxValue = 100;
            MinValue = 0;
            Value = 50;
        }

        public int MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                Value = Value;
            }
        }

        public int MinValue
        {
            get => minValue;
            set
            {
                minValue = value;
                Value = Value;
            }
        }

        public int Value
        {
            get => value;
            set => this.value = Math.Min(MaxValue, Math.Max(MinValue, value));
        }

        public ScrollBarTheme Theme { get; set; }

        public override void Draw(ICellSurface surface)
        {
            var theme = GetThemeColors();

            surface.SetCell(0, 0, theme.UpButton);
            surface.Fill(new Rectangle(0, 1, Location.Width, Location.Height - 2), theme.BarFill);
            surface.SetCell(0, Location.Height - 1, theme.DownButton);

            var scrollHeight = Location.Height - 3;
            var valuePosMultiplier = 0d;
            if (MaxValue - MinValue > 0)
            {
                valuePosMultiplier = (double) Value / (MaxValue - MinValue);
            }
            var valuePos = (int) Math.Round(scrollHeight * valuePosMultiplier);

            surface.SetCell(0, valuePos + 1, theme.Roller);
        }

        private ScrollBarThemeColors GetThemeColors()
        {
            if (!Enabled)
                return Theme.Disabled;

            if (isHover)
                return Theme.Hover;

            return Theme.Enabled;
        }

        public override bool ProcessMouse(IMouseState mouseState)
        {
            isHover = Location.Contains(mouseState.Position);

            if (!isHover)
                return false;

            if (mouseState.ScrollChange > 0)
            {
                Value--;
            }

            if (mouseState.ScrollChange < 0)
            {
                Value++;
            }

            if (mouseState.LeftButtonState)
            {
                if (mouseState.Position.Y == Location.Y) // Up button
                {
                    Value--;
                }

                if (mouseState.Position.Y == Location.Bottom - 1) // Down button
                {
                    Value++;
                }
            }

            return true;
        }
    }

    public class ScrollBarTheme
    {
        public ScrollBarTheme()
        {
            var colorEnabled = Color.White;
            var colorDisabled = Color.Gray;
            var colorHover = Color.Lime;

            Enabled = new ScrollBarThemeColors
            {
                UpButton = new Cell('▲', colorEnabled),
                DownButton = new Cell('▼', colorEnabled),
                BarFill = new Cell('░', colorEnabled),
                Roller = new Cell('↕', Color.Black, colorEnabled)
            };

            Disabled = new ScrollBarThemeColors
            {
                UpButton = new Cell('▲', colorDisabled),
                DownButton = new Cell('▼', colorDisabled),
                BarFill = new Cell('░', colorDisabled),
                Roller = new Cell('↕', Color.Black, colorDisabled)
            };

            Hover = new ScrollBarThemeColors
            {
                UpButton = new Cell('▲', colorHover),
                DownButton = new Cell('▼', colorHover),
                BarFill = new Cell('░', Color.White),
                Roller = new Cell('↕', Color.Black, Color.White)
            };
        }

        public ScrollBarThemeColors Enabled { get; set; }

        public ScrollBarThemeColors Disabled { get; set; }

        public ScrollBarThemeColors Hover { get; set; }
    }

    public class ScrollBarThemeColors
    {
        public Cell UpButton { get; set; }

        public Cell DownButton { get; set; }

        public Cell BarFill { get; set; }

        public Cell Roller { get; set; }
    }
}