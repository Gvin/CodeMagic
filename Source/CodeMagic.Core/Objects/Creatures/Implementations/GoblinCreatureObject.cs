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
    public class GoblinCreatureObject : NonPlayableCreatureObject
    {
        private readonly int hitChance;
        private readonly int minDamage;
        private readonly int maxDamage;
        private readonly Element damageElement;

        public GoblinCreatureObject(GoblinCreatureObjectConfiguration configuration) 
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
            var standStill = new StandStillStrategy();
            var attack = new ChasePlayerStrategy(new SimpleCreatureMovementStrategy());

            Logic.SetInitialStrategy(standStill);

            Logic.AddTransferRule(standStill, attack, GetIfPlayerVisible);
            Logic.AddTransferRule(attack, standStill, (map, position) => !GetIfPlayerVisible(map, position));
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
            return new StandardLootGenerator(ItemRareness.Trash, ItemRareness.Common,
                    0, 2,
                    0, 1, new[] {ArmorClass.Leather},
                    potionsCountMin: 0, potionsCountMax: 1)
                .GenerateLoot();
        }
    }

    public class GoblinCreatureObjectConfiguration : NonPlayableCreatureObjectConfiguration
    {
        public GoblinCreatureObjectConfiguration()
        {
            DamageElement = Element.Slashing;
            RemainsType = RemainsType.BloodGreenMedium;
        }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public Element DamageElement { get; set; }

        public int HitChance { get; set; }
    }
}