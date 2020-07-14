using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Game;
using CodeMagic.Game.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Mono.Drawing;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Mono.Fonts;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Views
{
    public class PlayerStatsView : BaseWindow, IPlayerStatsView
    {
        public PlayerStatsView() : base(FontTarget.Interface)
        {
            var closeButton = new FramedButton(new Rectangle(Width - 17, Height - 4, 15, 3))
            {
                Text = "[ESC] Close"
            };
            closeButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Controls.Add(closeButton);
        }

        public event EventHandler Exit;

        public Player Player { get; set; }

        public override bool ProcessKeyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Escape:
                    Exit?.Invoke(this, EventArgs.Empty);
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }

        public override void Draw(ICellSurface surface)
        {
            base.Draw(surface);

            surface.Write(2, 1, "Player Status");

            surface.Fill(new Rectangle(1, 2, Width - 2, 1), new Cell('─', FrameColor));
            surface.SetCell(0, 2, '╟', FrameColor);
            surface.SetCell(Width - 1, 2, '╢', FrameColor);

            PrintProtection(2, 4, surface);
            PrintPlayerStats(25, 4, surface);
            PrintWeapon(2, 20, surface);
        }

        private void PrintPlayerStats(int dX, int dY, ICellSurface surface)
        {
            surface.Write(dX, dY, "Stats:");

            var stats = Enum.GetValues(typeof(PlayerStats)).Cast<PlayerStats>().ToArray();
            var maxLength = stats.Select(TextHelper.GetStatName).Select(name => name.Length).Max();
            for (var index = 0; index < stats.Length; index++)
            {
                var stat = stats[index];

                var pureValue = Player.GetPureStat(stat);
                var bonusValue = Player.Equipment.GetStatsBonus(stat);

                var name = TextHelper.GetStatName(stat);

                var y = dY + 2 + index;
                surface.Write(dX, y, name);
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
                surface.Write(dX + maxLength + 1, y, bonusText.ToColoredString());
            }

            var xPos = dX + maxLength + 10;
            surface.Write(xPos, dY + 2, new StyledLine { "Max Health           ", new StyledString(Player.MaxHealth.ToString(), TextHelper.HealthColor) });
            surface.Write(xPos, dY + 3, new StyledLine { "Max Mana             ", new StyledString(Player.MaxMana.ToString(), TextHelper.ManaColor) });
            surface.Write(xPos, dY + 4, new StyledLine { "Mana Regeneration    ", new StyledString(Player.ManaRegeneration.ToString(), TextHelper.ManaRegenerationColor) });
            surface.Write(xPos, dY + 5, new StyledLine { "Dodge Chance         ", $"{Player.DodgeChance}%" });

            surface.Write(dX, dY + 4 + stats.Length, new StyledLine { $"Level: {Player.Level}" });
            surface.Write(dX, dY + 5 + stats.Length, new StyledLine { "XP:    ", new StyledString($"{Player.Experience} / {Player.GetXpToLevelUp()}", TextHelper.XpColor) });
        }

        private void PrintWeapon(int dX, int dY, ICellSurface surface)
        {
            surface.Fill(new Rectangle(1, dY, Width - 2, 1), new Cell('═', FrameColor));
            surface.SetCell(0, dY, '╠', FrameColor);
            surface.SetCell(Width - 1, dY, '╣', FrameColor);

            surface.Write(dX, dY + 1, new StyledLine
            {
                "Right Hand: ",
                new StyledString(Player.Equipment.RightHandItem.Name, ItemDrawingHelper.GetItemColor(Player.Equipment.RightHandItem))
            });

            surface.Fill(new Rectangle(1, dY + 2, Width - 2, 1), new Cell('─', FrameColor));
            surface.SetCell(0, dY + 2, '╟', FrameColor);
            surface.SetCell(Width - 1, dY + 2, '╢', FrameColor);

            var rightWeaponDetails = GetHoldableDetails(Player.Equipment.RightHandItem);
            for (int yShift = 0; yShift < rightWeaponDetails.Length; yShift++)
            {
                var y = dY + 3 + yShift;
                surface.Write(dX, y, rightWeaponDetails[yShift]);
            }

            var leftDy = dY + rightWeaponDetails.Length + 4;
            surface.Fill(new Rectangle(1, leftDy, Width - 2, 1), new Cell('═', FrameColor));
            surface.SetCell(0, leftDy, '╠', FrameColor);
            surface.SetCell(Width - 1, leftDy, '╣', FrameColor);

            surface.Write(dX, leftDy + 1, new StyledLine
            {
                "Left Hand:  ",
                new StyledString(Player.Equipment.LeftHandItem.Name, ItemDrawingHelper.GetItemColor(Player.Equipment.LeftHandItem))
            });

            surface.Fill(new Rectangle(1, leftDy + 2, Width - 2, 1), new Cell('─', FrameColor));
            surface.SetCell(0, leftDy + 2, '╟', FrameColor);
            surface.SetCell(Width - 1, leftDy + 2, '╢', FrameColor);

            var leftWeaponDetails = GetHoldableDetails(Player.Equipment.LeftHandItem);
            for (int yShift = 0; yShift < leftWeaponDetails.Length; yShift++)
            {
                var y = leftDy + 3 + yShift;
                surface.Write(dX, y, leftWeaponDetails[yShift]);
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

        private void PrintProtection(int dX, int dY, ICellSurface surface)
        {
            surface.Write(dX, dY, "Protection:");

            var elements = Enum.GetValues(typeof(Element)).Cast<Element>().ToArray();
            var maxLength = elements.Select(TextHelper.GetElementName).Select(name => name.Length).Max();
            for (var index = 0; index < elements.Length; index++)
            {
                var element = elements[index];

                var protection = Player.GetProtection(element);
                var color = TextHelper.GetElementColor(element).ToXna();
                var name = TextHelper.GetElementName(element);

                var y = dY + 2 + index;
                surface.Write(dX, y, name, color);
                surface.Write(dX + maxLength + 1, y, $"{protection}%");
            }
        }
    }
}