using System;
using CodeMagic.Core.Area.EnvironmentData;
using CodeMagic.Core.Game;
using CodeMagic.UI.Sad.Common;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Controls
{
    public class PlayerStatsControl : ControlBase
    {
        private static readonly Color FrameColor = Color.Gray;
        private static readonly Color BackgroundColor = Color.Black;

        private readonly IGameCore game;

        public PlayerStatsControl(int width, int height, IGameCore game)
            : base(width, height)
        {
            this.game = game;
            Theme = new DrawingSurfaceTheme();
            CanFocus = false;
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Draw();

            DrawCellManaLevel();
        }

        private void Draw()
        {
            Surface.Print(2, 1, "Player Status:");
            Surface.Fill(1, 2, Width - 2, FrameColor, BackgroundColor, Glyphs.GetGlyph('─'));
            Surface.Print(Width - 1, 2,
                new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, BackgroundColor));
            Surface.DrawVerticalLine(0, 1, Height,
                new ColoredGlyph(Glyphs.GetGlyph('│'), FrameColor, BackgroundColor));
            Surface.Print(0, 2, new ColoredGlyph(Glyphs.GetGlyph('├'), FrameColor, BackgroundColor));
            Surface.Print(Width - 1, 2,
                new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, BackgroundColor));
            Surface.Print(0, 0, new ColoredGlyph(Glyphs.GetGlyph('╤'), FrameColor, BackgroundColor));

            Surface.Print(2, 3, "HP:");
            Surface.Fill(6, 3, 10, BackgroundColor, BackgroundColor, null);
            Surface.Print(6, 3, new ColoredString($"{game.Player.Health} / {game.Player.MaxHealth}", Color.Red, BackgroundColor));
            Surface.Print(2, 4, "Mana:");
            Surface.Fill(8, 4, 15, BackgroundColor, BackgroundColor, null);
            Surface.Print(8, 4, new ColoredString($"{game.Player.Mana} / {game.Player.MaxMana}", Color.Blue, BackgroundColor));
        }

        private void DrawCellManaLevel()
        {
            var cell = game.Map.GetCell(game.PlayerPosition);

            Surface.Print(2, 6, "Area Mana:");

            const int manaBarLength = 30;
            var manaLevelPercent = (float) cell.MagicEnergy.Energy / MagicEnergy.MaxEnergy;
            var disturbanceLevelPercent = (float)cell.MagicEnergy.Disturbance / MagicEnergy.MaxEnergy;

            var manaLevelLength = (int) Math.Floor(manaBarLength * manaLevelPercent);
            var disturbanceLevelLength = (int) Math.Ceiling(manaBarLength * disturbanceLevelPercent);
            var leftLength = manaBarLength - (manaLevelLength + disturbanceLevelLength);

            var shiftX = 2;

            for (int i = 0; i < disturbanceLevelLength; i++)
            {
                Surface.Print(shiftX + i, 7, new ColoredGlyph(Glyphs.GetGlyph('.'), Color.Black, Color.BlueViolet));
            }

            shiftX += disturbanceLevelLength;
            for (int i = 0; i < leftLength; i++)
            {
                Surface.Print(shiftX + i, 7, new ColoredGlyph(Glyphs.GetGlyph('.'), Color.Black, Color.DarkBlue));
            }

            shiftX += leftLength;
            for (int i = 0; i < manaLevelLength; i++)
            {
                Surface.Print(shiftX + i, 7, new ColoredGlyph(Glyphs.GetGlyph('.'), Color.Black, Color.Blue));
            }
        }
    }
}