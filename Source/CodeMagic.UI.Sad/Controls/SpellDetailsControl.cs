using System;
using CodeMagic.Game.Spells;
using CodeMagic.UI.Sad.Common;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Controls
{
    public class SpellDetailsControl : ControlBase
    {
        private static readonly Color EnoughManaColor = Color.Lime;
        private static readonly Color NotEnoughManaColor = Color.Red;
        private static readonly Color DefaultManaColor = Color.Blue;

        private static readonly Color FrameColor = Color.Gray;

        static SpellDetailsControl()
        {
            Library.Default.SetControlTheme(typeof(SpellDetailsControl), new DrawingSurfaceTheme());
        }

        private readonly int? playerMana;

        public SpellDetailsControl(int width, int height, int? playerMana = null) 
            : base(width, height)
        {
            CanFocus = false;
            this.playerMana = playerMana;
        }

        public BookSpell Spell { get; set; }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Surface.Print(1, 0, "Selected Spell Details");
            Surface.Fill(0, 1, Width, FrameColor, null, Glyphs.GetGlyph('─'));

            Surface.Fill(1, 3, 15, Color.Black, Color.Black, null);
            if (Spell == null)
            {
                Surface.Print(1, 3, new ColoredString("Spell is empty", new Cell(Color.Gray, Color.Black)));
            }
            else
            {
                var manaColor = DefaultManaColor;
                if (playerMana.HasValue)
                {
                    manaColor = playerMana.Value >= Spell.ManaCost ? EnoughManaColor : NotEnoughManaColor;
                }
                
                Surface.Print(1, 3, new ColoredString("Mana cost:", Color.White, Color.Black));
                Surface.Print(12, 3, new ColoredString(Spell.ManaCost.ToString(), manaColor, Color.Black));
            }
        }
    }
}