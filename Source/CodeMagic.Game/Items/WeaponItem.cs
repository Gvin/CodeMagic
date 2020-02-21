using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public class WeaponItem : EquipableItem, IDescriptionProvider, IInventoryImageProvider, IWorldImageProvider
    {
        private readonly string[] description;
        private readonly SymbolsImage inventoryImage;
        private readonly SymbolsImage worldImage;

        public WeaponItem(WeaponItemConfiguration configuration) 
            : base(configuration)
        {
            MinDamage = configuration.MinDamage.ToDictionary(pair => pair.Key, pair => pair.Value);
            MaxDamage = configuration.MaxDamage.ToDictionary(pair => pair.Key, pair => pair.Value);

            HitChance = configuration.HitChance;

            description = configuration.Description;

            inventoryImage = configuration.InventoryImage;
            worldImage = configuration.WorldImage;
        }

        public Dictionary<Element, int> MaxDamage { get; }

        public Dictionary<Element, int> MinDamage { get; }

        public Dictionary<Element, int> GenerateDamage()
        {
            return MaxDamage.ToDictionary(pair => pair.Key,
                pair => RandomHelper.GetRandomValue(MinDamage[pair.Key], pair.Value));
        }

        public int HitChance { get; }

        public override bool Stackable => false;

        public virtual StyledLine[] GetDescription(Player player)
        {
            var result = GetCharacteristicsDescription(player).ToList();

            result.Add(StyledLine.Empty);
            result.AddRange(TextHelper.ConvertDescription(description));

            return result.ToArray();
        }

        protected virtual StyledLine[] GetCharacteristicsDescription(Player player)
        {
            var equipedWeapon = player.Equipment.Weapon;

            var result = new List<StyledLine>();

            if (equipedWeapon == null || Equals(equipedWeapon))
            {
                result.Add(TextHelper.GetWeightLine(Weight));
            }
            else
            {
                result.Add(TextHelper.GetCompareWeightLine(Weight, equipedWeapon.Weight));
            }

            result.Add(StyledLine.Empty);

            AddDamageDescription(result, equipedWeapon);

            var hitChanceLine = new StyledLine { "Hit Chance: " };
            if (equipedWeapon == null || Equals(equipedWeapon))
            {
                hitChanceLine.Add(TextHelper.GetValueString(HitChance, "%", false));
            }
            else
            {
                hitChanceLine.Add(TextHelper.GetCompareValueString(HitChance, equipedWeapon.HitChance, "%", false));
            }
            result.Add(hitChanceLine);

            result.Add(StyledLine.Empty);
            TextHelper.AddBonusesDescription(this, equipedWeapon, result);
            result.Add(StyledLine.Empty);
            TextHelper.AddLightBonusDescription(this, result);

            return result.ToArray();
        }

        private void AddDamageDescription(List<StyledLine> descriptionResult, WeaponItem equipedWeapon)
        {
            var equiped = Equals(equipedWeapon);

            foreach (Element element in Enum.GetValues(typeof(Element)))
            {
                var maxDamage = GetMaxDamage(this, element);
                var minDamage = GetMinDamage(this, element);

                var otherMaxDamage = GetMaxDamage(equipedWeapon, element);
                var otherMinDamage = GetMinDamage(equipedWeapon, element);

                if (maxDamage == 0 && minDamage == 0 && otherMaxDamage == 0 && otherMinDamage == 0)
                    continue;

                var damageLine = new StyledLine
                {
                    new StyledString($"{TextHelper.GetElementName(element)}",
                        TextHelper.GetElementColor(element)),
                    " Damage: "
                };

                if (equiped)
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

        private static int GetMaxDamage(WeaponItem item, Element element)
        {
            if (item == null)
                return 0;

            return item.MaxDamage.ContainsKey(element) ? item.MaxDamage[element] : 0;
        }

        private static int GetMinDamage(WeaponItem item, Element element)
        {
            if (item == null)
                return 0;

            return item.MinDamage.ContainsKey(element) ? item.MinDamage[element] : 0;
        }

        public virtual SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return worldImage;
        }

        public virtual SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return inventoryImage;
        }
    }

    public class WeaponItemConfiguration : EquipableItemConfiguration
    {
        public SymbolsImage InventoryImage { get; set; }

        public SymbolsImage WorldImage { get; set; }

        public string[] Description { get; set; }

        public Dictionary<Element, int> MaxDamage { get; set; }

        public Dictionary<Element, int> MinDamage { get; set; }

        public int HitChance { get; set; }
    }
}