using System.Collections.Generic;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures.Loot;
using CodeMagic.Core.Objects.Creatures.Remains;
using CodeMagic.Core.Objects.DecorativeObjects;

namespace CodeMagic.Core.Objects.Creatures
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

        public sealed override RemainsType RemainsType => configuration.RemainsType;

        protected sealed override double CatchFireChanceMultiplier => configuration.CatchFireChanceMultiplier;

        protected sealed override double SelfExtinguishChance => configuration.SelfExtinguishChance;

        public sealed override int MaxVisibilityRange => configuration.VisibilityRange;

        protected sealed override float NormalSpeed => configuration.Speed / 1;

        public override void Attack(IDestroyableObject target, IJournal journal)
        {
            base.Attack(target, journal);

            var currentHitChance = CalculateHitChance(hitChance);
            if (!RandomHelper.CheckChance(currentHitChance))
            {
                journal.Write(new AttackMissMessage(this, target));
                return;
            }

            foreach (var damageValue in damage)
            {
                var value = RandomHelper.GetRandomValue(damageValue.MinValue, damageValue.MaxValue);
                target.Damage(journal, value, damageValue.Element);
                journal.Write(new DealDamageMessage(this, target, value, damageValue.Element));
            }
        }

        protected override IMapObject GenerateRemains()
        {
            return new CreatureRemainsGenerator().GenerateRemains(this);
        }

        protected override IItem[] GenerateLoot()
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