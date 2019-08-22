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

            var result = new List<StyledLine>
            {
                new StyledLine {$"Weight: {Weight}"},
                StyledLine.Empty
            };
            AddDamageDescription(result, equipedWeapon);

            var hitChanceLine = new StyledLine {$"Hit Chance: {HitChance}"};
            if (!Equals(equipedWeapon))
            {
                hitChanceLine.Add(ItemTextHelper.GetComparisonString(HitChance, equipedWeapon?.HitChance ?? 0));
            }
            result.Add(hitChanceLine);

            result.Add(StyledLine.Empty);
            ItemTextHelper.AddBonusesDescription(this, equipedWeapon, result);
            result.Add(StyledLine.Empty);
            ItemTextHelper.AddLightBonusDescription(this, result);
            result.Add(StyledLine.Empty);
            result.AddRange(description.Select(line => new StyledLine
            {
                new StyledString(line, ItemTextHelper.DescriptionTextColor)
            }).ToArray());

            return result.ToArray();
        }

        private void AddDamageDescription(List<StyledLine> descriptionResult, WeaponItem equipedWeapon)
        {
            foreach (Element element in Enum.GetValues(typeof(Element)))
            {
                var maxDamage = GetMaxDamage(this, element);
                var minDamage = GetMinDamage(this, element);

                var otherMaxDamage = GetMaxDamage(equipedWeapon, element);
                var otherMinDamage = GetMinDamage(equipedWeapon, element);

                if (maxDamage == 0 && minDamage == 0 && otherMaxDamage == 0 && otherMinDamage == 0)
                    continue;

                Color? otherMinDamageColor = null;
                if (otherMinDamage > minDamage)
                {
                    otherMinDamageColor = ItemTextHelper.NegativeValueColor;
                }

                if (otherMinDamage < minDamage)
                {
                    otherMinDamageColor = ItemTextHelper.PositiveValueColor;
                }

                Color? otherMaxDamageColor = null;
                if (otherMaxDamage > maxDamage)
                {
                    otherMaxDamageColor = ItemTextHelper.NegativeValueColor;
                }

                if (otherMaxDamage < maxDamage)
                {
                    otherMaxDamageColor = ItemTextHelper.PositiveValueColor;
                }

                var damageLine = new StyledLine
                {
                    new StyledString($"{ItemTextHelper.GetElementName(element)}",
                        ItemTextHelper.GetElementColor(element)),
                    $" Damage: {minDamage} - {maxDamage}"
                };

                if (minDamage != otherMinDamage || maxDamage != otherMaxDamage)
                {
                    damageLine.Add(" now (");
                    damageLine.Add(new StyledString(otherMinDamage.ToString(), otherMinDamageColor));
                    damageLine.Add(" - ");
                    damageLine.Add(new StyledString(otherMaxDamage.ToString(), otherMaxDamageColor));
                    damageLine.Add(")");
                }

                descriptionResult.Add(damageLine);
            }
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