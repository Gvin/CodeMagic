﻿using System;
using CodeMagic.Core.Spells;
using CodeMagic.UI.Sad.Common;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Controls
{
    public class SpellDetailsControl : ControlBase
    {
        private static readonly Color FrameColor = Color.Gray;

        public SpellDetailsControl(int width, int height) 
            : base(width, height)
        {
            Theme = new DrawingSurfaceTheme();
            CanFocus = false;
        }

        public BookSpell Spell { get; set; }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Surface.Print(1, 0, "Selected Spell Details");
            Surface.Fill(0, 1, Width, FrameColor, null, Glyphs.GlyphBoxSingleHorizontal);

            Surface.Fill(1, 3, 15, Color.Black, Color.Black, null);
            if (Spell == null)
            {
                Surface.Print(1, 3, new ColoredString("Spell is empty", new Cell(Color.Gray, Color.Black)));
            }
            else
            {
                Surface.Print(1, 3, new ColoredString("Mana cost:", Color.White, Color.Black));
                Surface.Print(12, 3, new ColoredString(Spell.ManaCost.ToString(), Color.Blue, Color.Black));
            }
        }
    }
}