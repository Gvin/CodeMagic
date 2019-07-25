using System;
using CodeMagic.Core.Objects.PlayerData;
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

        private readonly IPlayer player;

        public PlayerStatsControl(int width, int height, IPlayer player)
            : base(width, height)
        {
            this.player = player;
            Theme = new DrawingSurfaceTheme();
            CanFocus = false;
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Draw();
        }

        private void Draw()
        {
            Surface.Print(2, 1, "Player Status:");
            Surface.Fill(1, 2, Width - 2, FrameColor, BackgroundColor, Glyphs.GlyphBoxSingleHorizontal);
            Surface.Print(Width - 1, 2,
                new ColoredGlyph(Glyphs.GlyphBoxDoubleVerticalSingleLeft, FrameColor, BackgroundColor));
            Surface.DrawVerticalLine(0, 1, Height,
                new ColoredGlyph(Glyphs.GlyphBoxSingleVertical, FrameColor, BackgroundColor));
            Surface.Print(0, 2, new ColoredGlyph(Glyphs.GlyphBoxSingleVerticalRight, FrameColor, BackgroundColor));
            Surface.Print(Width - 1, 2,
                new ColoredGlyph(Glyphs.GlyphBoxDoubleVerticalSingleLeft, FrameColor, BackgroundColor));
            Surface.Print(0, 0, new ColoredGlyph(Glyphs.GlyphBoxDoubleHorizontalSingleDown, FrameColor, BackgroundColor));

            Surface.Print(2, 3, "HP:");
            Surface.Fill(6, 3, 10, BackgroundColor, BackgroundColor, null);
            Surface.Print(6, 3, new ColoredString($"{player.Health} / {player.MaxHealth}", Color.Red, BackgroundColor));
            Surface.Print(2, 4, "Mana:");
            Surface.Fill(8, 4, 10, BackgroundColor, BackgroundColor, null);
            Surface.Print(8, 4, new ColoredString($"{player.Mana} / {player.MaxMana}", Color.Blue, BackgroundColor));
        }
    }
}