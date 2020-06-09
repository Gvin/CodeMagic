using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Objects.Creatures;
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

        private readonly GameCore<Player> game;

        public PlayerStatsControl(int width, int height, GameCore<Player> game)
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
            Surface.Clear(new Rectangle(2, 1, Width - 3, 6));

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

            Surface.PrintStyledText(2, 3, 
                new ColoredString("HP: "), 
                new ColoredString($"{game.Player.Health} / {game.Player.MaxHealth}", Color.Red, BackgroundColor));
            Surface.PrintStyledText(2, 4, 
                new ColoredString("Mana: "),
                new ColoredString($"{game.Player.Mana} / {game.Player.MaxMana}", Color.Blue, BackgroundColor));
            Surface.PrintStyledText(2, 5,
                new ColoredString("Stamina: "),
                new ColoredString($"{game.Player.Stamina} / {game.Player.MaxStamina}", Color.Gold, BackgroundColor));
            Surface.PrintStyledText(2, 6, 
                new ColoredString("Hunger: "), 
                new ColoredString($"{game.Player.HungerPercent}%", Color.Green, BackgroundColor));
        }

        private void DrawCellManaLevel()
        {
            const int yPos = 8;

            var cell = game.Map.GetCell(game.PlayerPosition);

            Surface.Print(2, yPos, "Area Mana:");

            const int manaBarLength = 30;
            var manaLevelPercent = (float) cell.MagicEnergyLevel() / cell.MaxMagicEnergyLevel();
            var disturbanceLevelPercent = (float)cell.MagicDisturbanceLevel() / cell.MaxMagicEnergyLevel();

            var manaLevelLength = (int) Math.Floor(manaBarLength * manaLevelPercent);
            var disturbanceLevelLength = (int) Math.Ceiling(manaBarLength * disturbanceLevelPercent);
            var leftLength = manaBarLength - (manaLevelLength + disturbanceLevelLength);

            var shiftX = 2;

            for (int i = 0; i < disturbanceLevelLength; i++)
            {
                Surface.Print(shiftX + i, yPos + 1, new ColoredGlyph(Glyphs.GetGlyph('.'), Color.Black, Color.BlueViolet));
            }

            shiftX += disturbanceLevelLength;
            for (int i = 0; i < leftLength; i++)
            {
                Surface.Print(shiftX + i, yPos + 1, new ColoredGlyph(Glyphs.GetGlyph('.'), Color.Black, Color.DarkBlue));
            }

            shiftX += leftLength;
            for (int i = 0; i < manaLevelLength; i++)
            {
                Surface.Print(shiftX + i, yPos + 1, new ColoredGlyph(Glyphs.GetGlyph('.'), Color.Black, Color.Blue));
            }
        }
    }
}