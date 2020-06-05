using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Monsters;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Creatures.NonPlayable
{
    public class MonsterCreatureImpl : MonsterCreatureObject, IWorldImageProvider
    {
        private const string SaveKeyConfigurationId = "ConfigurationId";

        private readonly MonsterCreatureImplConfiguration configuration;

        public MonsterCreatureImpl(SaveData data) 
            : base(data, GetConfiguration(data))
        {
            configuration = GetConfiguration(data);
        }

        public MonsterCreatureImpl(MonsterCreatureImplConfiguration configuration) 
            : base(configuration)
        {
            this.configuration = configuration;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyConfigurationId, configuration.Id);
            return data;
        }

        private static MonsterCreatureImplConfiguration GetConfiguration(SaveData data)
        {
            var configId = data.GetStringValue(SaveKeyConfigurationId);
            var config = ConfigurationManager.Current.Monsters.Monsters.First(monster => string.Equals(monster.Id, configId));
            return new MonsterCreatureImplConfiguration(config);
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            var body = storage.GetImage(configuration.Image);
            var directionImageName = GetWorldImageName();
            var directionImage = storage.GetImage(directionImageName);
            return SymbolsImage.Combine(body, directionImage);
        }

        private string GetWorldImageName()
        {
            switch (Direction)
            {
                case Direction.North:
                    return "Creature_Up";
                case Direction.South:
                    return "Creature_Down";
                case Direction.West:
                    return "Creature_Left";
                case Direction.East:
                    return "Creature_Right";
                default:
                    throw new ArgumentException($"Unknown creature direction: {Direction}");
            }
        }
    }

    public class MonsterCreatureImplConfiguration : MonsterCreatureObjectConfiguration
    {
        public MonsterCreatureImplConfiguration(IMonsterConfiguration config)
        {
            var health = RandomHelper.GetRandomValue(config.Stats.MinHealth, config.Stats.MaxHealth);

            Id = config.Id;
            Name = config.Name;
            LogicPattern = config.LogicPattern;
            Experience = config.Experience;
            Size = config.Size;
            ZIndex = ZIndex.Creature;
            Accuracy = config.Stats.Accuracy;
            DodgeChance = config.Stats.DodgeChance;
            MaxHealth = health;
            RemainsType = config.RemainsType;
            DamageMarkType = config.DamageMarkType;
            CatchFireChanceMultiplier = config.Stats.CatchFireChanceMultiplier;
            SelfExtinguishChance = config.Stats.SelfExtinguishChanceMultiplier;
            Image = config.Image;
            LootConfiguration = config.Loot;
            VisibilityRange = config.Stats.VisibilityRange;
            Speed = config.Stats.Speed;
            Damage.AddRange(config.Stats.Damage.Select(conf => new MonsterDamageValue(conf.Element, conf.MinValue, conf.MaxValue)));
            foreach (var protectionConfiguration in config.Stats.Protection)
            {
                BaseProtection.Add(protectionConfiguration.Element, protectionConfiguration.Value);
            }
        }

        public string Image { get; set; }
    }
}