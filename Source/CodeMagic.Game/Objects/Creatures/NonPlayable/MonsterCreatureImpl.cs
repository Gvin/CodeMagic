using System;
using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.Configuration.Monsters;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Creatures.NonPlayable
{
    public class MonsterCreatureImpl : MonsterCreatureObject, IWorldImageProvider
    {
        private readonly IMonsterImagesConfiguration images;

        public MonsterCreatureImpl(MonsterCreatureImplConfiguration configuration) 
            : base(configuration)
        {
            images = configuration.Images;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            var worldImageName = GetWorldImageName();
            return storage.GetImage(worldImageName);
        }

        private string GetWorldImageName()
        {
            switch (Direction)
            {
                case Direction.North:
                    return images.North;
                case Direction.South:
                    return images.South;
                case Direction.West:
                    return images.West;
                case Direction.East:
                    return images.East;
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

            Name = config.Name;
            LogicPattern = config.LogicPattern;
            Size = config.Size;
            ZIndex = ZIndex.Creature;
            HitChance = config.Stats.HitChance;
            MaxHealth = health;
            RemainsType = config.RemainsType;
            CatchFireChanceMultiplier = config.Stats.CatchFireChanceMultiplier;
            SelfExtinguishChance = config.Stats.SelfExtinguishChanceMultiplier;
            Images = config.Images;
            LootConfiguration = config.Loot;
            VisibilityRange = config.Stats.VisibilityRange;
            Speed = config.Stats.Speed;
            Damage.AddRange(config.Stats.Damage.Select(conf => new MonsterDamageValue(conf.Element, conf.MinValue, conf.MaxValue)));
            foreach (var protectionConfiguration in config.Stats.Protection)
            {
                BaseProtection.Add(protectionConfiguration.Element, protectionConfiguration.Value);
            }
        }

        public IMonsterImagesConfiguration Images { get; set; }
    }
}