using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Saving;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.CreaturesLogic;
using CodeMagic.Game.CreaturesLogic.Strategies;

namespace CodeMagic.Game.Objects.Creatures
{
    public abstract class NonPlayableCreatureObject : CreatureObject, INonPlayableCreatureObject
    {
        private const string SaveKeyMaxHealth = "MaxHealth";
        private const string SaveKeyLogicPattern = "LogicPattern";
        private const string SaveKeyTurnsCounter = "TurnsCounter";

        private float turnsCounter;
        private readonly string logicPattern;

        protected NonPlayableCreatureObject(SaveData data) : base(data)
        {
            Logic = new Logic();
            turnsCounter = float.Parse(data.GetStringValue(SaveKeyTurnsCounter));
            logicPattern = data.GetStringValue(SaveKeyLogicPattern);

            if (!string.IsNullOrEmpty(logicPattern))
            {
                var configurator = StandardLogicFactory.GetConfigurator(logicPattern);
                configurator.Configure(Logic);
            }
            else
            {
                Logic.SetInitialStrategy(new StandStillStrategy());
            }

            MaxHealth = data.GetIntValue(SaveKeyMaxHealth);
        }

        protected NonPlayableCreatureObject(string name, int maxHealth, string logicPattern)
            : base(name, maxHealth)
        {
            Logic = new Logic();
            turnsCounter = 0;
            this.logicPattern = logicPattern;

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

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyLogicPattern, logicPattern);
            data.Add(SaveKeyMaxHealth, MaxHealth);
            data.Add(SaveKeyTurnsCounter, turnsCounter);
            return data;
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
            base.Update(position);

            turnsCounter += 1;
            if (turnsCounter >= Speed)
            {
                Logic.Update(this, position);
                turnsCounter -= Speed;
            }
        }

        public virtual void Attack(Point position, Point targetPosition, IDestroyableObject target)
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