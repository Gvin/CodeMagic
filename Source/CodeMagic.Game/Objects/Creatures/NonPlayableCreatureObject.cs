using System.Linq;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.CreaturesLogic;
using CodeMagic.Game.CreaturesLogic.Strategies;

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

            MaxHealth = maxHealth;
        }

        public override int MaxHealth { get; }

        protected virtual float NormalSpeed => 1f;

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

        public override void Update(Point position)
        {
            turnsCounter += 1;
            if (turnsCounter >= Speed)
            {
                Logic.Update(this, position);
                turnsCounter -= Speed;
            }
        }

        public virtual void Attack(Point position, IDestroyableObject target)
        {
        }

        protected Logic Logic { get; }

        public override void OnDeath(Point position)
        {
            base.OnDeath(position);

            var remains = GenerateRemains();
            if (remains != null)
            {
                CurrentGame.Map.AddObject(position, remains);
            }

            var loot = GenerateLoot();
            if (loot != null && loot.Any())
            {
                foreach (var item in loot)
                {
                    CurrentGame.Map.AddObject(position, item);
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