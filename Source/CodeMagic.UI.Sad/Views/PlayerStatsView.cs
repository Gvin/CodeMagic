using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Game;
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
            PrintPlayerStats(25, 4);
            PrintWeapon(2, 20);
        }

        private void PrintPlayerStats(int dX, int dY)
        {
            Print(dX, dY, "Stats:");

            var stats = Enum.GetValues(typeof(PlayerStats)).Cast<PlayerStats>().ToArray();
            var maxLength = stats.Select(TextHelper.GetStatName).Select(name => name.Length).Max();
            for (var index = 0; index < stats.Length; index++)
            {
                var stat = stats[index];

                var pureValue = player.GetPureStat(stat);
                var bonusValue = player.Equipment.GetStatsBonus(stat);
                
                var name = TextHelper.GetStatName(stat);

                var y = dY + 2 + index;
                Print(dX, y, name);
                var bonusText = new StyledLine
                {
                    pureValue.ToString()
                };
                if (bonusValue != 0)
                {
                    var bonusSymbol = bonusValue > 0 ? "+" : "-";
                    var bonusColor = bonusValue > 0 ? TextHelper.PositiveValueColor : TextHelper.NegativeValueColor;
                    bonusText.Add(" (");
                    bonusText.Add($"{bonusSymbol}{bonusValue}", bonusColor);
                    bonusText.Add(")");
                }
                PrintStyledText(dX + maxLength + 1, y, bonusText);
            }

            var charsY = dY + 3 + stats.Length;
            PrintStyledText(dX, charsY + 0, new StyledLine { "Max Health           ", new StyledString(player.MaxHealth.ToString(), TextHelper.HealthColor) });
            PrintStyledText(dX, charsY + 1, new StyledLine { "Max Mana             ", new StyledString(player.MaxMana.ToString(), TextHelper.ManaColor) });
            PrintStyledText(dX, charsY + 2, new StyledLine { "Mana Regeneration    ", new StyledString(player.ManaRegeneration.ToString(), TextHelper.ManaRegenerationColor) });

            PrintStyledText(dX, charsY + 4, new StyledLine {$"Level: {player.Level}"});
            PrintStyledText(dX, charsY + 5, new StyledLine {"XP:    ", new StyledString($"{player.Experience} / {player.GetXpToLevelUp()}", TextHelper.XpColor)});
        }

        private void PrintWeapon(int dX, int dY)
        {
            Fill(1, dY, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('═'));
            Print(0, dY, new ColoredGlyph(Glyphs.GetGlyph('╠'), FrameColor, DefaultBackground));
            Print(Width - 1, dY, new ColoredGlyph(Glyphs.GetGlyph('╣'), FrameColor, DefaultBackground));

            PrintStyledText(dX, dY + 1, new StyledLine
            {
                "Right Hand Weapon: ",
                new StyledString(player.Equipment.RightWeapon.Name, ItemDrawingHelper.GetItemColor(player.Equipment.RightWeapon))
            });

            Fill(1, dY + 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
            Print(0, dY + 2, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            Print(Width - 1, dY + 2, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            var rightWeaponDetails = GetWeaponDetails(player.Equipment.RightWeapon);
            for (int yShift = 0; yShift < rightWeaponDetails.Length; yShift++)
            {
                var y = dY + 3 + yShift;
                PrintStyledText(dX, y, rightWeaponDetails[yShift]);
            }

            var leftDy = dY + rightWeaponDetails.Length + 4;
            Fill(1, leftDy, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('═'));
            Print(0, leftDy, new ColoredGlyph(Glyphs.GetGlyph('╠'), FrameColor, DefaultBackground));
            Print(Width - 1, leftDy, new ColoredGlyph(Glyphs.GetGlyph('╣'), FrameColor, DefaultBackground));

            PrintStyledText(dX, leftDy + 1, new StyledLine
            {
                "Left Hand Weapon:  ",
                new StyledString(player.Equipment.LeftWeapon.Name, ItemDrawingHelper.GetItemColor(player.Equipment.LeftWeapon))
            });

            Fill(1, leftDy + 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
            Print(0, leftDy + 2, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            Print(Width - 1, leftDy + 2, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            var leftWeaponDetails = GetWeaponDetails(player.Equipment.LeftWeapon);
            for (int yShift = 0; yShift < leftWeaponDetails.Length; yShift++)
            {
                var y = leftDy + 3 + yShift;
                PrintStyledText(dX, y, leftWeaponDetails[yShift]);
            }
        }

        private StyledLine[] GetWeaponDetails(IWeaponItem weapon)
        {
            var result = new List<StyledLine>
            {
                new StyledLine
                {
                    $"Hit Chance: {weapon.HitChance}%"
                },
                StyledLine.Empty
            };

            foreach (Element element in Enum.GetValues(typeof(Element)))
            {
                var maxDamage = WeaponItem.GetMaxDamage(weapon, element);
                var minDamage = WeaponItem.GetMinDamage(weapon, element);

                if (maxDamage == 0 && minDamage == 0)
                    continue;

                var damageLine = new StyledLine
                {
                    new StyledString($"{TextHelper.GetElementName(element)}",
                        TextHelper.GetElementColor(element)),
                    " Damage: ",
                    $"{minDamage} - {maxDamage}"
                };

                result.Add(damageLine);
            }

            return result.ToArray();
        }

        private void PrintProtection(int dX, int dY)
        {
            Print(dX, dY, "Protection:");

            var elements = Enum.GetValues(typeof(Element)).Cast<Element>().ToArray();
            var maxLength = elements.Select(TextHelper.GetElementName).Select(name => name.Length).Max();
            for (var index = 0; index < elements.Length; index++)
            {
                var element = elements[index];

                var protection = player.GetProtection(element);
                var color = TextHelper.GetElementColor(element).ToXna();
                var name = TextHelper.GetElementName(element);

                var y = dY + 2 + index;
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