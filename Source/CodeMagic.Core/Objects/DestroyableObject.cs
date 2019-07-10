using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Area.EnvironmentData;
using CodeMagic.Core.Game;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects
{
    public class DestroyableObject : IDestroyableObject
    {
        private int health;
        private int maxHealth;

        public DestroyableObject(DestroyableObjectConfiguration configuration)
        {
            Id = Guid.NewGuid().ToString();

            Name = configuration.Name;
            MaxHealth = configuration.MaxHealth;
            Health = configuration.Health;
            SelfExtinguishChance = configuration.SelfExtinguishChance;
            CatchFireChanceMultiplier = configuration.CatchFireChanceMultiplier;

            Statuses = new ObjectStatusesCollection();
        }

        public virtual string Id { get; }

        public string Name { get; }

        public virtual bool BlocksMovement => false;

        public virtual bool IsVisible => true;

        public virtual bool BlocksVisibility => false;

        public virtual bool BlocksProjectiles => false;

        public virtual bool BlocksEnvironment => false;

        public ObjectStatusesCollection Statuses { get; }

        private int SelfExtinguishChance { get; }

        public int GetSelfExtinguishChance()
        {
            if (Statuses.Contains(WetObjectStatus.StatusType))
                return SelfExtinguishChance + WetObjectStatus.SelfExtinguishChanceBonus;
            return SelfExtinguishChance;
        }

        public int Health
        {
            get => health;
            set
            {
                if (value < 0)
                {
                    health = 0;
                    return;
                }
                if (value > maxHealth)
                {
                    health = maxHealth;
                    return;
                }
                health = value;
            }
        }

        public int MaxHealth
        {
            get => maxHealth;
            set
            {
                if (value < 0)
                    throw new ArgumentException($"Max Health value cannot be < 0 for object {Name}");
                maxHealth = value;
                if (health > maxHealth)
                    health = maxHealth;
            }
        }

        private int CatchFireChanceMultiplier { get; }

        public virtual void Damage(int damage, Element? element = null)
        {
            if (damage < 0)
                throw new ArgumentException($"Damage should be greater or equal 0. Got {damage}.");

            Health -= damage;

            if (element.HasValue && element.Value == Element.Fire)
            {
                CheckCatchFire(damage);
            }
        }

        protected virtual OnFireObjectStatusConfiguration GetFireConfiguration()
        {
            return new OnFireObjectStatusConfiguration
            {
                BurnBeforeExtinguishCheck = 3,
                BurningTemperature = Temperature.WoodBurnTemperature,
                FireDamageMax = 6,
                FireDamageMin = 2
            };
        }

        private void CheckCatchFire(int damage)
        {
            var catchFireChance = damage * CatchFireChanceMultiplier;
            if (Statuses.Contains(WetObjectStatus.StatusType))
            {
                catchFireChance -= WetObjectStatus.CatchFireChancePenalty;
            }
            if (RandomHelper.CheckChance(catchFireChance))
            {
                Statuses.Add(new OnFireObjectStatus(GetFireConfiguration()));
            }
        }

        public virtual void OnDeath(IAreaMap map, Point position)
        {
        }
    }

    public class DestroyableObjectConfiguration
    {
        private const int DefaultCatchFireChanceMultiplier = 1;
        private const int DefaultSelfExtinguishChance = 15;

        public DestroyableObjectConfiguration()
        {
            CatchFireChanceMultiplier = DefaultCatchFireChanceMultiplier;
            SelfExtinguishChance = DefaultSelfExtinguishChance;
        }

        public string Name { get; set; }

        public int Health { get; set; }

        public int MaxHealth { get; set; }

        public int CatchFireChanceMultiplier { get; set; }

        public int SelfExtinguishChance { get; set; }
    }
}