using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Sad.Common;
using Microsoft.Xna.Framework;
using SadConsole;

namespace CodeMagic.UI.Sad.Controls.VisualControls
{
    public class PlayerStatsVisualControl : ContainerVisualControl
    {
        private static readonly Color FrameColor = Color.Gray;
        private static readonly Color BackgroundColor = Color.Black;

        private readonly GameCore<Player> game;

        private readonly ProgressBarVisualControl healthBar;
        private readonly ProgressBarVisualControl manaBar;
        private readonly ProgressBarVisualControl staminaBar;
        private readonly ProgressBarVisualControl hungerBar;

        public PlayerStatsVisualControl(Rectangle position, GameCore<Player> game)
            : base(position)
        {
            this.game = game;

            var progressBarsWidth = Width - 3;

            var healthBackColor = Color.FromNonPremultiplied(112, 0, 0, 255);
            healthBar = new ProgressBarVisualControl(
                new Rectangle(2, 4, progressBarsWidth, 1))
            {
                Max = game.Player.MaxHealth,
                Min = 0,
                Value = game.Player.Health,
                FillCell = new Cell(Color.Black, Color.Red, '.'),
                EmptyCell = new Cell(healthBackColor, healthBackColor)
            };
            Add(healthBar);

            manaBar = new ProgressBarVisualControl(
                new Rectangle(2, 7, progressBarsWidth, 1))
            {
                Max = game.Player.MaxMana,
                Min = 0,
                Value = game.Player.Mana,
                FillCell = new Cell(Color.Black, Color.Blue, '.'),
                EmptyCell = new Cell(Color.DarkBlue, Color.DarkBlue)
            };
            Add(manaBar);

            var staminaBackColor = Color.FromNonPremultiplied(142, 102, 8, 255);
            staminaBar = new ProgressBarVisualControl(
                new Rectangle(2, 10, progressBarsWidth, 1))
            {
                Max = game.Player.MaxStamina,
                Min = 0,
                Value = game.Player.Stamina,
                FillCell = new Cell(Color.Black, Color.Gold, '.'),
                EmptyCell = new Cell(staminaBackColor, staminaBackColor)
            };
            Add(staminaBar);

            var hungerBackColor = Color.FromNonPremultiplied(0, 102, 0, 255);
            hungerBar = new ProgressBarVisualControl(
                new Rectangle(2, 13, progressBarsWidth, 1))
            {
                Max = 100,
                Min = 0,
                Value = game.Player.HungerPercent,
                FillCell = new Cell(Color.Black, Color.Lime, '.'),
                EmptyCell = new Cell(hungerBackColor, hungerBackColor)
            };
            Add(hungerBar);
        }

        private int Width => Position.Width;

        private int Height => Position.Height;

        public override void Draw(CellSurface surface)
        {
            healthBar.Max = game.Player.MaxHealth;
            healthBar.Value = game.Player.Health;

            manaBar.Max = game.Player.MaxMana;
            manaBar.Value = game.Player.Mana;

            staminaBar.Max = game.Player.MaxStamina;
            staminaBar.Value = game.Player.Stamina;

            hungerBar.Value = game.Player.HungerPercent;

            base.Draw(surface);

            DrawStats(surface);
            DrawCellManaLevel(surface);
        }

        private void DrawCellManaLevel(CellSurface surface)
        {
            const int yPos = 15;

            var cell = game.Map.GetCell(game.PlayerPosition);

            surface.Print(2, yPos, "Area Mana");

            var manaBarLength = Width - 3;
            var manaLevelPercent = (float)cell.MagicEnergyLevel() / cell.MaxMagicEnergyLevel();
            var disturbanceLevelPercent = (float)cell.MagicDisturbanceLevel() / cell.MaxMagicEnergyLevel();

            var manaLevelLength = (int)Math.Floor(manaBarLength * manaLevelPercent);
            var disturbanceLevelLength = (int)Math.Ceiling(manaBarLength * disturbanceLevelPercent);
            var leftLength = manaBarLength - (manaLevelLength + disturbanceLevelLength);

            var shiftX = 2;

            for (int i = 0; i < disturbanceLevelLength; i++)
            {
                surface.Print(shiftX + i, yPos + 1, new ColoredGlyph(Glyphs.GetGlyph('.'), Color.Black, Color.BlueViolet));
            }

            shiftX += disturbanceLevelLength;
            for (int i = 0; i < leftLength; i++)
            {
                surface.Print(shiftX + i, yPos + 1, new ColoredGlyph(Glyphs.GetGlyph('.'), Color.Black, Color.DarkBlue));
            }

            shiftX += leftLength;
            for (int i = 0; i < manaLevelLength; i++)
            {
                surface.Print(shiftX + i, yPos + 1, new ColoredGlyph(Glyphs.GetGlyph('.'), Color.Black, Color.DeepSkyBlue));
            }
        }

        private void DrawStats(CellSurface surface)
        {
            surface.Print(2, 1, "Player Status");
            surface.Fill(1, 2, Width - 1, FrameColor, BackgroundColor, Glyphs.GetGlyph('─'));
            surface.DrawVerticalLine(0, 0, Height,
                new ColoredGlyph(Glyphs.GetGlyph('│'), FrameColor, BackgroundColor));
            surface.Print(0, 2, new ColoredGlyph(Glyphs.GetGlyph('├'), FrameColor, BackgroundColor));

            surface.Print(2, 3, $"Health [{game.Player.Health} / {game.Player.MaxHealth}]");

            surface.Print(2, 6, $"Mana [{game.Player.Mana} / {game.Player.MaxMana}]");

            surface.Print(2, 9, $"Stamina [{game.Player.Stamina} / {game.Player.MaxStamina}]");

            surface.Print(2, 12, $"Hunger [{game.Player.HungerPercent}%]");
        }
    }
}