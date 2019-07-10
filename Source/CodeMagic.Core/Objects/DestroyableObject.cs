using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects
{
    public class DestroyableObject : IDestroyableObject
    {
        private const int CatchFireChanceMultiplier = 1;

        private int health;
        private int maxHealth;

        public DestroyableObject(DestroyableObjectConfiguration configuration)
        {
            Id = Guid.NewGuid().ToString();

            Name = configuration.Name;
            MaxHealth = configuration.MaxHealth;
            Health = configuration.Health;

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

        private void CheckCatchFire(int damage)
        {
            var catchFireChance = damage * CatchFireChanceMultiplier;
            if (RandomHelper.CheckChance(catchFireChance))
            {
                Statuses.Add(new OnFireObjectStatus());
            }
        }

        public virtual void OnDeath(IAreaMap map, Point position)
        {
        }
    }

    public class DestroyableObjectConfiguration
    {
        public string Name { get; set; }

        public int Health { get; set; }

        public int MaxHealth { get; set; }
    }
}