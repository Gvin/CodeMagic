using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.CreaturesLogic.Strategies;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects.Creatures
{
    public abstract class NonPlayableCreatureObject : CreatureObject, INonPlayableCreatureObject
    {
        private readonly float normalSpeed;
        private float turnsCounter;

        public NonPlayableCreatureObject(NonPlayableCreatureObjectConfiguration configuration) 
            : base(configuration)
        {
            Logic = new Logic();
            normalSpeed = 1 / configuration.Speed;
            RemainsType = configuration.RemainsType;
            turnsCounter = 0;

            if (!string.IsNullOrEmpty(configuration.LogicPattern))
            {
                var configurator = StandardLogicFactory.GetConfigurator(configuration.LogicPattern);
                configurator.Configure(Logic);
            }
            else
            {
                Logic.SetInitialStrategy(new StandStillStrategy());
            }
        }

        public RemainsType RemainsType { get; set; }

        private float Speed
        {
            get
            {
                if (Statuses.Contains(FrozenObjectStatus.StatusType))
                {
                    return normalSpeed * FrozenObjectStatus.SpeedMultiplier;
                }

                return normalSpeed;
            }
        }

        public void Update(IAreaMap map, IJournal journal, Point position)
        {
            turnsCounter += 1;
            if (turnsCounter >= Speed)
            {
                Logic.Update(this, map, journal, position);
                turnsCounter -= Speed;
            }
        }

        public virtual void Attack(IDestroyableObject target, IJournal journal)
        {
        }

        public bool Updated { get; set; }

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        protected Logic Logic { get; }

        public override void OnDeath(IAreaMap map, IJournal journal, Point position)
        {
            base.OnDeath(map, journal, position);

            var remains = GenerateRemains();
            if (remains != null)
            {
                map.AddObject(position, remains);
            }

            var loot = GenerateLoot();
            if (loot != null && loot.Any())
            {
                foreach (var item in loot)
                {
                    map.AddObject(position, item);
                }
            }
        }

        protected virtual IMapObject GenerateRemains()
        {
            return null;
        }

        protected virtual IItem[] GenerateLoot()
        {
            return null;
        }
    }

    public class NonPlayableCreatureObjectConfiguration : CreatureObjectConfiguration
    {
        public NonPlayableCreatureObjectConfiguration()
        {
            Speed = 1f;
        }

        public string LogicPattern { get; set; }

        public float Speed { get; set; }

        public RemainsType RemainsType { get; set; }
    }
}