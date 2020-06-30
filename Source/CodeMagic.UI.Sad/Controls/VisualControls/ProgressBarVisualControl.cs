using System;
using CodeMagic.UI.Sad.Common;
using SadConsole;
using SadRogue.Primitives;

namespace CodeMagic.UI.Sad.Controls.VisualControls
{
    public class ProgressBarVisualControl : IVisualControl
    {
        public ProgressBarVisualControl(Rectangle position)
        {
            Position = position;
            IsVisible = true;

            Max = 100;
            Min = 0;
            Value = 50;

            FillCell = new ColoredGlyph(Color.Lime, Color.Lime, ' ');
            EmptyCell = new ColoredGlyph(Color.Green, Color.Green, ' ');
        }

        public Rectangle Position { get; }

        public bool IsVisible { get; set; }

        public int Max { get; set; }

        public int Min { get; set; }

        public int Value { get; set; }

        public ColoredGlyph FillCell { get; set; }

        public ColoredGlyph EmptyCell { get; set; }

        public void Draw(ICellSurface surface)
        {
            surface.Fill(
                new Rectangle(0, 0, Position.Width, Position.Height), 
                EmptyCell.Foreground, EmptyCell.Background, EmptyCell.Glyph);

            var sizeMultiplier = (double) Value / (Max - Min);
            var fillWidth = (int) Math.Round(Position.Width * sizeMultiplier);
            
            if (Value > Min && fillWidth == 0)
            {
                fillWidth = 1;
            }

            if (Value < Max && fillWidth == Position.Width)
            {
                fillWidth--;
            }

            surface.Fill(
                new Rectangle(0, 0, fillWidth, Position.Height),
                FillCell.Foreground, FillCell.Background, FillCell.Glyph);
        }
    }
}