using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items
{
    public class WeaponItem : HoldableDurableItemBase, IWeaponItem
    {
        private const string SaveKeyAccuracy = "Accuracy";
        private const string SaveKeyMinDamage = "MinDamage";
        private const string SaveKeyMaxDamage = "MaxDamage";

        public WeaponItem(SaveData data) : base(data)
        {
            MinDamage = data.GetObject<DictionarySaveable>(SaveKeyMinDamage).Data
                .ToDictionary(pair => (Element) int.Parse((string) pair.Key), pair => int.Parse((string) pair.Value));
            MaxDamage = data.GetObject<DictionarySaveable>(SaveKeyMaxDamage).Data
                .ToDictionary(pair => (Element)int.Parse((string)pair.Key), pair => int.Parse((string)pair.Value));

            Accuracy = data.GetIntValue(SaveKeyAccuracy);
        }

        public WeaponItem(WeaponItemConfiguration configuration) 
            : base(configuration)
        {
            MinDamage = configuration.MinDamage.ToDictionary(pair => pair.Key, pair => pair.Value);
            MaxDamage = configuration.MaxDamage.ToDictionary(pair => pair.Key, pair => pair.Value);

            Accuracy = configuration.HitChance;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();

            data.Add(SaveKeyAccuracy, Accuracy);
            data.Add(SaveKeyMinDamage,
                new DictionarySaveable(MinDamage.ToDictionary(pair => (object) (int) pair.Key,
                    pair => (object) pair.Value)));
            data.Add(SaveKeyMaxDamage,
                new DictionarySaveable(MaxDamage.ToDictionary(pair => (object)(int)pair.Key,
                    pair => (object)pair.Value)));
            return data;
        }

        public Dictionary<Element, int> MaxDamage { get; }

        public Dictionary<Element, int> MinDamage { get; }

        public Dictionary<Element, int> GenerateDamage()
        {
            Durability--;

            return MaxDamage.ToDictionary(pair => pair.Key,
                pair => RandomHelper.GetRandomValue(MinDamage[pair.Key], pair.Value));
        }

        public int Accuracy { get; }

        public override bool Stackable => false;

        public override StyledLine[] GetDescription(Player player)
        {
            var result = GetCharacteristicsDescription(player).ToList();

            result.Add(StyledLine.Empty);
            result.AddRange(TextHelper.ConvertDescription(Description));

            return result.ToArray();
        }

        private StyledLine[] GetCharacteristicsDescription(Player player)
        {
            var rightHandWeapon = player.Equipment.RightHandItem as IWeaponItem;
            var leftHandWeapon = player.Equipment.LeftHandItem as IWeaponItem;

            var result = new List<StyledLine>();

            if (Equals(rightHandWeapon) || Equals(leftHandWeapon) || rightHandWeapon == null)
            {
                result.Add(TextHelper.GetWeightLine(Weight));
            }
            else
            {
                result.Add(TextHelper.GetCompareWeightLine(Weight, rightHandWeapon.Weight));
            }

            result.Add(StyledLine.Empty);

            result.Add(TextHelper.GetDurabilityLine(Durability, MaxDurability));

            result.Add(StyledLine.Empty);

            AddDamageDescription(result, leftHandWeapon, rightHandWeapon);

            var hitChanceLine = new StyledLine { "Accuracy: " };
            if (Equals(rightHandWeapon) || Equals(leftHandWeapon) || rightHandWeapon == null)
            {
                hitChanceLine.Add(TextHelper.GetValueString(Accuracy, "%", false));
            }
            else
            {
                hitChanceLine.Add(TextHelper.GetCompareValueString(Accuracy, rightHandWeapon.Accuracy, "%", false));
            }
            result.Add(hitChanceLine);

            result.Add(StyledLine.Empty);
            if (Equals(rightHandWeapon) || Equals(leftHandWeapon) || rightHandWeapon == null)
            {
                TextHelper.AddBonusesDescription(this, null, result);
            }
            else
            {
                TextHelper.AddBonusesDescription(this, rightHandWeapon, result);
            }
            result.Add(StyledLine.Empty);
            TextHelper.AddLightBonusDescription(this, result);

            return result.ToArray();
        }

        private void AddDamageDescription(List<StyledLine> descriptionResult, IWeaponItem equippedWeaponLeft, IWeaponItem equippedWeaponRight)
        {
            foreach (Element element in Enum.GetValues(typeof(Element)))
            {
                var maxDamage = GetMaxDamage(this, element);
                var minDamage = GetMinDamage(this, element);

                var otherMaxDamage = GetMaxDamage(equippedWeaponRight, element);
                var otherMinDamage = GetMinDamage(equippedWeaponRight, element);

                if (maxDamage == 0 && minDamage == 0 && otherMaxDamage == 0 && otherMinDamage == 0)
                    continue;

                var damageLine = new StyledLine
                {
                    new StyledString($"{TextHelper.GetElementName(element)}",
                        TextHelper.GetElementColor(element)),
                    " Damage: "
                };

                if (Equals(equippedWeaponRight) || Equals(equippedWeaponLeft) || equippedWeaponRight == null)
                {
                    damageLine.Add($"{minDamage} - {maxDamage}");
                }
                else
                {
                    var thisMinColor = GetValueDependentColor(minDamage, otherMinDamage);
                    var thisMaxColor = GetValueDependentColor(maxDamage, otherMaxDamage);

                    var otherMinColor = GetValueDependentColor(otherMinDamage, minDamage);
                    var otherMaxColor = GetValueDependentColor(otherMaxDamage, maxDamage);

                    damageLine.Add(new StyledString(minDamage.ToString(), thisMinColor));
                    damageLine.Add(" - ");
                    damageLine.Add(new StyledString(maxDamage.ToString(), thisMaxColor));
                    damageLine.Add(" (now ");
                    damageLine.Add(new StyledString(otherMinDamage.ToString(), otherMinColor));
                    damageLine.Add(" - ");
                    damageLine.Add(new StyledString(otherMaxDamage.ToString(), otherMaxColor));
                    damageLine.Add(")");
                }

                descriptionResult.Add(damageLine);
            }
        }

        private Color GetValueDependentColor(int value, int otherValue)
        {
            if (value == otherValue)
                return TextHelper.NeutralColor;
            return value > otherValue ? TextHelper.PositiveValueColor : TextHelper.NegativeValueColor;
        }

        public static int GetMaxDamage(IWeaponItem item, Element element)
        {
            if (item == null)
                return 0;

            return item.MaxDamage.ContainsKey(element) ? item.MaxDamage[element] : 0;
        }

        public static int GetMinDamage(IWeaponItem item, Element element)
        {
            if (item == null)
                return 0;

            return item.MinDamage.ContainsKey(element) ? item.MinDamage[element] : 0;
        }
    }

    public class WeaponItemConfiguration : HoldableItemConfiguration
    {
        public Dictionary<Element, int> MaxDamage { get; set; }

        public Dictionary<Element, int> MinDamage { get; set; }

        public int HitChance { get; set; }
    }
}