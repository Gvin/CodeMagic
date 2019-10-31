using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.CreaturesLogic.Strategies;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.Objects.Creatures
{
    public abstract class NonPlayableCreatureObject : CreatureObject, INonPlayableCreatureObject
    {
        private float turnsCounter;

        protected NonPlayableCreatureObject(int maxHealth, string logicPattern)
            : base(maxHealth)
        {
            Logic = new Logic();
            turnsCounter = 0;

            if (!string.IsNullOrEmpty(logicPattern))
            {
                var configurator = StandardLogicFactory.GetConfigurator(logicPattern);
                configurator.Configure(Logic);
            }
            else
            {
                Logic.SetInitialStrategy(new StandStillStrategy());
            }
        }

        protected virtual float NormalSpeed => 1f;

        public abstract RemainsType RemainsType { get; }

        private float Speed
        {
            get
            {
                if (Statuses.Contains(FrozenObjectStatus.StatusType))
                {
                    return NormalSpeed * FrozenObjectStatus.SpeedMultiplier;
                }

                return NormalSpeed;
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
}