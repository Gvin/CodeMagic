﻿using CodeMagic.Game.Spells;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Controls
{
    public class SpellDetailsControl : Control
    {
        private static readonly Color EnoughManaColor = Color.Lime;
        private static readonly Color NotEnoughManaColor = Color.Red;
        private static readonly Color DefaultManaColor = Color.Blue;

        private static readonly Color FrameColor = Color.Gray;

        private readonly int? playerMana;

        public SpellDetailsControl(Rectangle location, int? playerMana = null)
            : base(location)
        {
            this.playerMana = playerMana;
        }

        public BookSpell Spell { get; set; }

        public override void Draw(ICellSurface surface)
        {
            surface.Write(1, 0, "Selected Spell Details");
            surface.Fill(new Rectangle(0, 1, Location.Width, 1), new Cell('─', FrameColor));

            if (Spell == null)
            {
                surface.Write(1, 3, new ColoredString("Spell is empty", Color.Gray));
            }
            else
            {
                var manaColor = DefaultManaColor;
                if (playerMana.HasValue)
                {
                    manaColor = playerMana.Value >= Spell.ManaCost ? EnoughManaColor : NotEnoughManaColor;
                }

                surface.Write(1, 3, new ColoredString("Mana cost:", Color.White, Color.Black));
                surface.Write(12, 3, new ColoredString(Spell.ManaCost.ToString(), manaColor, Color.Black));
            }
        }
    }
}