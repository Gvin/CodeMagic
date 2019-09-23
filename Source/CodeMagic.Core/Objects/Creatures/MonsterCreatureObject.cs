using System.Collections.Generic;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures.Loot;
using CodeMagic.Core.Objects.Creatures.Remains;

namespace CodeMagic.Core.Objects.Creatures
{
    public abstract class MonsterCreatureObject : NonPlayableCreatureObject
    {
        private readonly ILootConfiguration lootConfiguration;
        private readonly MonsterDamageValue[] damage;
        private readonly int hitChance;

        protected MonsterCreatureObject(MonsterCreatureObjectConfiguration configuration) 
            : base(configuration)
        {
            damage = configuration.Damage.ToArray();
            hitChance = configuration.HitChance;
            lootConfiguration = configuration.LootConfiguration;
            RemainsType = configuration.RemainsType;
        }

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

    public class MonsterCreatureObjectConfiguration : NonPlayableCreatureObjectConfiguration
    {
        public MonsterCreatureObjectConfiguration()
        {
            Damage = new List<MonsterDamageValue>();
        }

        public int HitChance { get; set; }

        public List<MonsterDamageValue> Damage { get; }

        public ILootConfiguration LootConfiguration { get; set; }
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