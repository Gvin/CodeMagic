using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.ItemsGeneration.Configuration.Bonuses;
using CodeMagic.ItemsGeneration.Implementations.Bonuses.Instances;

namespace CodeMagic.ItemsGeneration.Implementations.Bonuses
{
    public class BonusesGenerator
    {
        private const string GroupNameCommon = "All";
        private const string GroupNameEquipable = "Equipable";
        private const string GroupNameWeapon = "Weapon";
        private const string GroupNameArmor = "Armor";

        private static readonly Dictionary<string, IBonusApplier> BonusAppliers = new Dictionary<string, IBonusApplier>
        {
            {DamageBonusApplier.BonusType, new DamageBonusApplier()},
            {HealthBonusApplier.BonusType, new HealthBonusApplier()},
            {ManaBonusApplier.BonusType, new ManaBonusApplier()},
            {ManaRegenerationBonusApplier.BonusType, new ManaRegenerationBonusApplier()},
            {ProtectionBonusApplier.BonusType, new ProtectionBonusApplier()},
            {WeightBonusApplier.BonusType, new WeightBonusApplier()}
        };

        private readonly IBonusesConfiguration configuration;

        public BonusesGenerator(IBonusesConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void GenerateBonuses(ItemConfiguration item, int bonusesCount)
        {
            var configurationGroup = GetConfigurationGroup(item);
            var config = GetConfiguration(item.Rareness, configurationGroup);
            var nameBuilder = new NameBuilder(item.Name);

            GenerateBonuses(item, nameBuilder, config, bonusesCount);

            item.Name = nameBuilder.ToString();
        }

        private void GenerateBonuses(
            ItemConfiguration item, 
            NameBuilder name, 
            IBonusConfiguration[] config,
            int bonusesCount)
        {
            for (var counter = 0; counter < bonusesCount; counter++)
            {
                var bonusConfig = RandomHelper.GetRandomElement(config);
                ApplyBonus(item, name, bonusConfig);
            }
        }

        private void ApplyBonus(ItemConfiguration item, NameBuilder name, IBonusConfiguration bonusConfig)
        {
            if (!BonusAppliers.ContainsKey(bonusConfig.Type))
                throw new ApplicationException($"Bonus applier not found for bonus type: {bonusConfig.Type}");

            var applier = BonusAppliers[bonusConfig.Type];
            applier.Apply(bonusConfig, item, name);
        }

        private string GetConfigurationGroup(ItemConfiguration item)
        {
            switch (item)
            {
                case WeaponItemConfiguration _:
                    return GroupNameWeapon;
                case ArmorItemConfiguration _:
                    return GroupNameArmor;
                case EquipableItemConfiguration _:
                    return GroupNameEquipable;
                default:
                    return GroupNameCommon;
            }
        }

        private IBonusConfiguration[] GetConfiguration(ItemRareness rareness, string groupName)
        {
            var configurations = CollectGroupConfiguration(groupName);
            return configurations.Where(conf => conf.Rareness == rareness).SelectMany(conf => conf.Bonuses).ToArray();
        }

        private IBonusRarenessConfiguration[] CollectGroupConfiguration(string groupName)
        {
            var result = new List<IBonusRarenessConfiguration>();

            var group = configuration.Groups.FirstOrDefault(gr => string.Equals(gr.Type, groupName));
            while (group != null)
            {
                result.AddRange(group.Configuration);
                if (string.IsNullOrEmpty(group.InheritFrom))
                    break;
                group = configuration.Groups.FirstOrDefault(gr => string.Equals(gr.Type, group.InheritFrom));
            }

            return result.ToArray();
        }
    }
}