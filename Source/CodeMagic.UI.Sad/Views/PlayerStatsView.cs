using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Game;
using CodeMagic.Game.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Input;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class PlayerStatsView : GameViewBase, IPlayerStatsView
    {
        private StandardButton closeButton;

        public PlayerStatsView()
        {
            InitializeControls();
        }

        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            surface.Print(2, 1, "Player Status");

            surface.Fill(1, 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
            surface.Print(0, 2, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            surface.Print(Width - 1, 2, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            PrintProtection(2, 4, surface);
            PrintPlayerStats(25, 4, surface);
            PrintWeapon(2, 20, surface);
        }

        private void PrintPlayerStats(int dX, int dY, CellSurface surface)
        {
            surface.Print(dX, dY, "Stats:");

            var stats = Enum.GetValues(typeof(PlayerStats)).Cast<PlayerStats>().ToArray();
            var maxLength = stats.Select(TextHelper.GetStatName).Select(name => name.Length).Max();
            for (var index = 0; index < stats.Length; index++)
            {
                var stat = stats[index];

                var pureValue = Player.GetPureStat(stat);
                var bonusValue = Player.Equipment.GetStatsBonus(stat);
                
                var name = TextHelper.GetStatName(stat);

                var y = dY + 2 + index;
                surface.Print(dX, y, name);
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
                surface.PrintStyledText(dX + maxLength + 1, y, bonusText.ToColoredString(DefaultBackground));
            }

            var xPos = dX + maxLength + 10;
            surface.PrintStyledText(xPos, dY + 2, new StyledLine { "Max Health           ", new StyledString(Player.MaxHealth.ToString(), TextHelper.HealthColor) }.ToColoredString(DefaultBackground));
            surface.PrintStyledText(xPos, dY + 3, new StyledLine { "Max Mana             ", new StyledString(Player.MaxMana.ToString(), TextHelper.ManaColor) }.ToColoredString(DefaultBackground));
            surface.PrintStyledText(xPos, dY + 4, new StyledLine { "Mana Regeneration    ", new StyledString(Player.ManaRegeneration.ToString(), TextHelper.ManaRegenerationColor) }.ToColoredString(DefaultBackground));
            surface.PrintStyledText(xPos, dY + 5, new StyledLine { "Dodge Chance         ", $"{Player.DodgeChance}%"}.ToColoredString(DefaultBackground));

            surface.PrintStyledText(dX, dY + 4 + stats.Length, new StyledLine {$"Level: {Player.Level}"}.ToColoredString(DefaultBackground));
            surface.PrintStyledText(dX, dY + 5 + stats.Length, new StyledLine {"XP:    ", new StyledString($"{Player.Experience} / {Player.GetXpToLevelUp()}", TextHelper.XpColor)}.ToColoredString(DefaultBackground));
        }

        private void PrintWeapon(int dX, int dY, CellSurface surface)
        {
            surface.Fill(1, dY, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('═'));
            surface.Print(0, dY, new ColoredGlyph(Glyphs.GetGlyph('╠'), FrameColor, DefaultBackground));
            surface.Print(Width - 1, dY, new ColoredGlyph(Glyphs.GetGlyph('╣'), FrameColor, DefaultBackground));

            surface.PrintStyledText(dX, dY + 1, new StyledLine
            {
                "Right Hand: ",
                new StyledString(Player.Equipment.RightHandItem.Name, ItemDrawingHelper.GetItemColor(Player.Equipment.RightHandItem))
            }.ToColoredString(DefaultBackground));

            surface.Fill(1, dY + 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
            surface.Print(0, dY + 2, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            surface.Print(Width - 1, dY + 2, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            var rightWeaponDetails = GetHoldableDetails(Player.Equipment.RightHandItem);
            for (int yShift = 0; yShift < rightWeaponDetails.Length; yShift++)
            {
                var y = dY + 3 + yShift;
                surface.PrintStyledText(dX, y, rightWeaponDetails[yShift].ToColoredString(DefaultBackground));
            }

            var leftDy = dY + rightWeaponDetails.Length + 4;
            surface.Fill(1, leftDy, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('═'));
            surface.Print(0, leftDy, new ColoredGlyph(Glyphs.GetGlyph('╠'), FrameColor, DefaultBackground));
            surface.Print(Width - 1, leftDy, new ColoredGlyph(Glyphs.GetGlyph('╣'), FrameColor, DefaultBackground));

            surface.PrintStyledText(dX, leftDy + 1, new StyledLine
            {
                "Left Hand:  ",
                new StyledString(Player.Equipment.LeftHandItem.Name, ItemDrawingHelper.GetItemColor(Player.Equipment.LeftHandItem))
            }.ToColoredString(DefaultBackground));

            surface.Fill(1, leftDy + 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
            surface.Print(0, leftDy + 2, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            surface.Print(Width - 1, leftDy + 2, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            var leftWeaponDetails = GetHoldableDetails(Player.Equipment.LeftHandItem);
            for (int yShift = 0; yShift < leftWeaponDetails.Length; yShift++)
            {
                var y = leftDy + 3 + yShift;
                surface.PrintStyledText(dX, y, leftWeaponDetails[yShift].ToColoredString(DefaultBackground));
            }
        }

        private StyledLine[] GetHoldableDetails(IHoldableItem item)
        {
            if (item is IWeaponItem weapon)
            {
                return GetWeaponDetails(weapon);
            }
            return new StyledLine[0];
        }

        private StyledLine[] GetWeaponDetails(IWeaponItem weapon)
        {
            var result = new List<StyledLine>
            {
                new StyledLine
                {
                    $"Accuracy: {weapon.Accuracy + Player.AccuracyBonus}%"
                },
                StyledLine.Empty
            };

            foreach (Element element in Enum.GetValues(typeof(Element)))
            {
                var maxDamage = WeaponItem.GetMaxDamage(weapon, element);
                var minDamage = WeaponItem.GetMinDamage(weapon, element);

                maxDamage = AttackHelper.CalculateDamage(maxDamage, element, Player);
                minDamage = AttackHelper.CalculateDamage(minDamage, element, Player);

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

        private void PrintProtection(int dX, int dY, CellSurface surface)
        {
            surface.Print(dX, dY, "Protection:");

            var elements = Enum.GetValues(typeof(Element)).Cast<Element>().ToArray();
            var maxLength = elements.Select(TextHelper.GetElementName).Select(name => name.Length).Max();
            for (var index = 0; index < elements.Length; index++)
            {
                var element = elements[index];

                var protection = Player.GetProtection(element);
                var color = TextHelper.GetElementColor(element).ToXna();
                var name = TextHelper.GetElementName(element);

                var y = dY + 2 + index;
                surface.Print(dX, y, name, color);
                surface.Print(dX + maxLength + 1, y, $"{protection}%");
            }
        }

        private void InitializeControls()
        {
            closeButton = new StandardButton(15)
            {
                Position = new Point(Width - 17, Height - 4),
                Text = "[ESC] Close"
            };
            closeButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Add(closeButton);
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.Escape:
                    Exit?.Invoke(this, EventArgs.Empty);
                    return true;
            }

            return false;
        }

        public void Close()
        {
            Close(DialogResult.None);
        }

        public event EventHandler Exit;
        public Player Player { private get; set; }
    }
}