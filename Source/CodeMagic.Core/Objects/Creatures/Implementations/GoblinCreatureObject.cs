using CodeMagic.Core.Area;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.CreaturesLogic.MovementStrategies;
using CodeMagic.Core.CreaturesLogic.Strategies;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.Objects.Creatures.Implementations
{
    public class GoblinCreatureObject : NonPlayableCreatureObject
    {
        public GoblinCreatureObject(GoblinCreatureObjectConfiguration configuration) 
            : base(configuration)
        {
            MinDamage = configuration.MinDamage;
            MaxDamage = configuration.MaxDamage;
            ViewDistance = configuration.ViewDistance;

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
            return
                CreaturesVisibilityHelper.GetIfPointIsVisible(map, position, ViewDistance, playerPosition);
        }

        public override void Attack(IDestroyableObject target, Journal journal)
        {
            var damage = RandomHelper.GetRandomValue(MinDamage, MaxDamage);
            target.Damage(damage, null);
            journal.Write(new DealDamageMessage(this, target, damage));
        }

        private int MinDamage { get; }

        private int MaxDamage { get; }

        private int ViewDistance { get; }

        public override bool BlocksMovement => true;
    }

    public class GoblinCreatureObjectConfiguration : NonPlayableCreatureObjectConfiguration
    {
        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public int ViewDistance { get; set; }
    }
}