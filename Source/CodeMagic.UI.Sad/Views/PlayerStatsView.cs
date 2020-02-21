using System;
using System.Linq;
using System.Xml.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Game.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Sad.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class PlayerStatsView : View
    {
        private readonly Player player;

        private Button closeButton;

        public PlayerStatsView(Player player) 
            : base(Program.Width, Program.Height)
        {
            this.player = player;
            
            InitializeControls();

            Print(2, 1, "Player Status");

            Fill(1, 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
            Print(0, 2, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            Print(Width - 1, 2, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            PrintProtection(2, 4);
        }

        private void PrintProtection(int dX, int dY)
        {
            Print(dX, dY, "Protection:");

            var elements = Enum.GetValues(typeof(Element)).Cast<Element>().ToArray();
            var maxLength = elements.Select(ItemTextHelper.GetElementName).Select(name => name.Length).Max();
            for (var index = 0; index < elements.Length; index++)
            {
                var element = elements[index];

                var protection = player.GetProtection(element);
                var color = ItemTextHelper.GetElementColor(element).ToXna();
                var name = ItemTextHelper.GetElementName(element);

                var y = dY + 1 + index;
                Print(dX, y, name, color);
                Print(dX + maxLength + 1, y, $"{protection}%");
            }
        }

        private void InitializeControls()
        {
            var buttonsTheme = new ButtonLinesTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(Color.White, DefaultBackground)
                }
            };
            closeButton = new Button(15, 3)
            {
                Position = new Point(Width - 17, Height - 4),
                Text = "[ESC] Close",
                CanFocus = false,
                Theme = buttonsTheme
            };
            closeButton.Click += closeButton_Click;
            Add(closeButton);
        }

        private void closeButton_Click(object sender, EventArgs args)
        {
            Close();
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.Escape:
                    Close();
                    return true;
            }

            return false;
        }
    }
}