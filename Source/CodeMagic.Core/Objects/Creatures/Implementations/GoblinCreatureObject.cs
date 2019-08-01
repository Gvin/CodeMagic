using CodeMagic.Core.Area;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.CreaturesLogic.MovementStrategies;
using CodeMagic.Core.CreaturesLogic.Strategies;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.Objects.Creatures.Implementations
{
    public class GoblinCreatureObject : NonPlayableCreatureObject
    {
        private readonly int hitChance;
        private readonly int minDamage;
        private readonly int maxDamage;

        public GoblinCreatureObject(GoblinCreatureObjectConfiguration configuration) 
            : base(configuration)
        {
            minDamage = configuration.MinDamage;
            maxDamage = configuration.MaxDamage;
            hitChance = configuration.HitChance;

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
            target.Damage(journal, damage);
            journal.Write(new DealDamageMessage(this, target, damage));
        }

        public override bool BlocksMovement => true;

        protected override IMapObject CreateDeathRemains()
        {
            var type = DecorativeObjectConfiguration.ObjectTypeGreenBloodMedium;
            var typeRoll = RandomHelper.GetRandomValue(1, 2);
            if (typeRoll == 2)
                type = DecorativeObjectConfiguration.ObjectTypeGreenBloodBig;

            return Injector.Current.Create<IDecorativeObject>(new DecorativeObjectConfiguration
            {
                Name = "Goblin Blood",
                Type = type,
                ZIndex = ZIndex.GroundDecoration
            });
        }
    }

    public class GoblinCreatureObjectConfiguration : NonPlayableCreatureObjectConfiguration
    {
        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public int HitChance { get; set; }
    }
}