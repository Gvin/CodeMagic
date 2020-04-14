using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures.Loot;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.Objects.Creatures
{
    public abstract class MonsterCreatureObject : NonPlayableCreatureObject
    {
        private readonly ILootConfiguration lootConfiguration;
        private readonly MonsterDamageValue[] damage;
        private readonly int hitChance;
        private readonly MonsterCreatureObjectConfiguration configuration;

        protected MonsterCreatureObject(MonsterCreatureObjectConfiguration configuration) 
            : base(configuration.MaxHealth, configuration.LogicPattern)
        {
            this.configuration = configuration;

            damage = configuration.Damage.ToArray();
            hitChance = configuration.HitChance;
            lootConfiguration = configuration.LootConfiguration;

            if (configuration.BaseProtection != null)
            {
                foreach (var pair in configuration.BaseProtection)
                {
                    BaseProtection.Add(pair.Key, pair.Value);
                }
            }
        }

        public sealed override string Name => configuration.Name;

        public sealed override ObjectSize Size => configuration.Size;

        public sealed override ZIndex ZIndex => configuration.ZIndex;

        protected sealed override double CatchFireChanceMultiplier => configuration.CatchFireChanceMultiplier;

        protected sealed override double SelfExtinguishChance => configuration.SelfExtinguishChance;

        public sealed override int MaxVisibilityRange => configuration.VisibilityRange;

        protected sealed override float NormalSpeed => configuration.Speed / 1;

        public override void Attack(Point position, IDestroyableObject target)
        {
            base.Attack(position, target);

            var currentHitChance = CalculateHitChance(hitChance);
            if (!RandomHelper.CheckChance(currentHitChance))
            {
                CurrentGame.Journal.Write(new AttackMissMessage(this, target));
                return;
            }

            foreach (var damageValue in damage)
            {
                var value = RandomHelper.GetRandomValue(damageValue.MinValue, damageValue.MaxValue);
                target.Damage(position, value, damageValue.Element);
                CurrentGame.Journal.Write(new DealDamageMessage(this, target, value, damageValue.Element));
            }
        }

        protected sealed override IMapObject GenerateRemains()
        {
            return new CreatureRemains(configuration.RemainsType);
        }

        protected override IMapObject GenerateDamageMark()
        {
            return new CreatureRemains(configuration.DamageMarkType);
        }

        protected sealed override IItem[] GenerateLoot()
        {
            return new ChancesLootGenerator(lootConfiguration).GenerateLoot();
        }
    }

    public class MonsterCreatureObjectConfiguration
    {
        public MonsterCreatureObjectConfiguration()
        {
            Damage = new List<MonsterDamageValue>();
            BaseProtection = new Dictionary<Element, int>();
        }

        public int HitChance { get; set; }

        public List<MonsterDamageValue> Damage { get; }

        public ILootConfiguration LootConfiguration { get; set; }

        public string Name { get; set; }

        public string LogicPattern { get; set; }

        public ObjectSize Size { get; set; }

        public ZIndex ZIndex { get; set; }

        public int MaxHealth { get; set; }

        public RemainsType RemainsType { get; set; }

        public RemainsType DamageMarkType { get; set; }

        public double CatchFireChanceMultiplier { get; set; }

        public double SelfExtinguishChance { get; set; }

        public int VisibilityRange { get; set; }

        public float Speed { get; set; }

        public Dictionary<Element, int> BaseProtection { get; set; }
    }

    public class MonsterDamageValue
    {
        public MonsterDamageValue(Element element, int minValue, int maxValue)
        {
            Element = element;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public Element Element { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }
    }
}