using System;
using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Windows.Controls
{
    public class ProgressBar : Control
    {
        public ProgressBar(Rectangle location)
            : base(location)
        {
            Theme = new ProgressBarTheme();

            MaxValue = 100;
            MinValue = 0;
            Value = 50;
        }

        public int MaxValue { get; set; }

        public int MinValue { get; set; }

        public int Value { get; set; }

        public ProgressBarTheme Theme { get; set; }

        public override void Draw(ICellSurface surface)
        {
            surface.Fill(
                new Rectangle(0, 0, Location.Width, Location.Height), Theme.EmptyBar);

            var sizeMultiplier = (double)Value / (MaxValue - MinValue);
            var fillWidth = (int)Math.Round(Location.Width * sizeMultiplier);

            if (Value > MinValue && fillWidth == 0)
            {
                fillWidth = 1;
            }

            if (Value < MaxValue && fillWidth == Location.Width)
            {
                fillWidth--;
            }

            surface.Fill(
                new Rectangle(0, 0, fillWidth, Location.Height), Theme.FilledBar);
        }
    }

    public class ProgressBarTheme
    {
        public ProgressBarTheme()
        {
            FilledBar = new Cell('█', Color.Green);
            EmptyBar = new Cell('█', Color.Red);
        }

        public Cell FilledBar { get; set; }

        public Cell EmptyBar { get; set; }
    }
}