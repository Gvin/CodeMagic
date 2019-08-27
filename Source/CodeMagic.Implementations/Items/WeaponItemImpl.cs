using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items
{
    public class WeaponItemImpl : WeaponItem, IDescriptionProvider, IInventoryImageProvider, IWorldImageProvider
    {
        private readonly string[] description;
        private readonly SymbolsImage inventoryImage;
        private readonly SymbolsImage worldImage;

        public WeaponItemImpl(WeaponItemImplConfiguration configuration) : base(configuration)
        {
            description = configuration.Description;

            inventoryImage = configuration.InventoryImage;
            worldImage = configuration.WorldImage;
        }

        public StyledLine[] GetDescription(IPlayer player)
        {
            var equipedWeapon = player.Equipment.Weapon;

            var result = new List<StyledLine>();

            if (equipedWeapon == null || Equals(equipedWeapon))
            {
                result.Add(ItemTextHelper.GetWeightLine(Weight));
            }
            else
            {
                result.Add(ItemTextHelper.GetCompareWeightLine(Weight, equipedWeapon.Weight));
            }

            result.Add(StyledLine.Empty);

            AddDamageDescription(result, equipedWeapon);

            var hitChanceLine = new StyledLine {"Hit Chance: "};
            if (equipedWeapon == null || Equals(equipedWeapon))
            {
                hitChanceLine.Add(ItemTextHelper.GetValueString(HitChance, "%", false));
            }
            else
            {
                hitChanceLine.Add(ItemTextHelper.GetCompareValueString(HitChance, equipedWeapon.HitChance, "%", false));
            }
            result.Add(hitChanceLine);

            result.Add(StyledLine.Empty);
            ItemTextHelper.AddBonusesDescription(this, equipedWeapon, result);
            result.Add(StyledLine.Empty);
            ItemTextHelper.AddLightBonusDescription(this, result);
            result.Add(StyledLine.Empty);
            result.AddRange(ItemTextHelper.ConvertDescription(description));

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
                    new StyledString($"{ItemTextHelper.GetElementName(element)}",
                        ItemTextHelper.GetElementColor(element)),
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
                return ItemTextHelper.NeutralColor;
            return value > otherValue ? ItemTextHelper.PositiveValueColor : ItemTextHelper.NegativeValueColor;
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

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return worldImage;
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return inventoryImage;
        }
    }

    public class WeaponItemImplConfiguration : WeaponItemConfiguration
    {
        public SymbolsImage InventoryImage { get; set; }

        public SymbolsImage WorldImage { get; set; }

        public string[] Description { get; set; }
    }
}