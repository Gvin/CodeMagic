using CodeMagic.Core.Area;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.CreaturesLogic.MovementStrategies;
using CodeMagic.Core.CreaturesLogic.Strategies;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures.Loot;
using CodeMagic.Core.Objects.Creatures.Remains;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.Objects.Creatures.Implementations
{
    // TODO: Implement creature weapon
    public class SkeletonCreatureObject : NonPlayableCreatureObject
    {
        private readonly int hitChance;
        private readonly int minDamage;
        private readonly int maxDamage;
        private readonly Element damageElement;

        public SkeletonCreatureObject(SkeletonCreatureObjectConfiguration configuration) 
            : base(configuration)
        {
            minDamage = configuration.MinDamage;
            maxDamage = configuration.MaxDamage;
            hitChance = configuration.HitChance;
            damageElement = configuration.DamageElement;

            ConfigureLogic();
        }

        private void ConfigureLogic()
        {
            var patrol = new PatrolAreaStrategy();
            var attack = new ChasePlayerStrategy(new SimpleCreatureMovementStrategy());

            Logic.SetInitialStrategy(patrol);

            Logic.AddTransferRule(patrol, attack, GetIfPlayerVisible);
            Logic.AddTransferRule(attack, patrol, (map, position) => !GetIfPlayerVisible(map, position));
        }

        private bool GetIfPlayerVisible(IAreaMap map, Point position)
        {
            var playerPosition = map.GetObjectPosition<IPlayer>();
            if (playerPosition == null)
                return false;
            return
                CreaturesVisibilityHelper.GetIfPointIsVisible(map, position, VisibilityRange, playerPosition);
        }

        public override void Attack(IDestroyableObject target, Journal journal)
        {
            var currentHitChance = CalculateHitChance(hitChance);
            if (!RandomHelper.CheckChance(currentHitChance))
            {
                journal.Write(new AttackMissMessage(this, target));
                return;
            }

            var damage = RandomHelper.GetRandomValue(minDamage, maxDamage);
            target.Damage(journal, damage, damageElement);
            journal.Write(new DealDamageMessage(this, target, damage, damageElement));
        }

        public override bool BlocksMovement => true;

        protected override IMapObject GenerateRemains()
        {
            return new CreatureRemainsGenerator().GenerateRemains(this);
        }

        protected override IItem[] GenerateLoot()
        {
            return new ChancesLootGenerator(
                new[]
                {
                    new Chance<int>(50, 0),
                    new Chance<int>(50, 1)
                },
                new[]
                {
                    new Chance<ItemRareness>(70, ItemRareness.Trash),
                    new Chance<ItemRareness>(30, ItemRareness.Common),
                },
                new[]
                {
                    new Chance<int>(80, 0),
                    new Chance<int>(20, 1),
                },
                new[]
                {
                    new Chance<ItemRareness>(70, ItemRareness.Trash),
                    new Chance<ItemRareness>(30, ItemRareness.Common),
                },
                new[]
                {
                    new Chance<ArmorClass>(90, ArmorClass.Leather),
                    new Chance<ArmorClass>(10, ArmorClass.Mail),
                }
            ).GenerateLoot();
        }
    }

    public class SkeletonCreatureObjectConfiguration : NonPlayableCreatureObjectConfiguration
    {
        public SkeletonCreatureObjectConfiguration()
        {
            DamageElement = Element.Blunt;
            RemainsType = RemainsType.BonesMedium;
        }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public Element DamageElement { get; set; }

        public int HitChance { get; set; }
    }
}