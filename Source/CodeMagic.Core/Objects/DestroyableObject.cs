using System;
using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects
{
    public class DestroyableObject : IDestroyableObject
    {
        private int health;
        private int maxHealth;
        private readonly List<IDamageRecord> damageRecords;

        public DestroyableObject(DestroyableObjectConfiguration configuration)
        {
            Id = Guid.NewGuid().ToString();

            Name = configuration.Name;
            MaxHealth = configuration.MaxHealth;
            Health = configuration.Health;
            SelfExtinguishChance = configuration.SelfExtinguishChance;
            CatchFireChanceMultiplier = configuration.CatchFireChanceMultiplier;
            ZIndex = configuration.ZIndex;

            Statuses = new ObjectStatusesCollection(this);
            damageRecords = new List<IDamageRecord>();
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

        public ZIndex ZIndex { get; }

        public IDamageRecord[] DamageRecords => damageRecords.ToArray();

        public int GetSelfExtinguishChance()
        {
            var result = SelfExtinguishChance;

            var burningRelatesStatuses = Statuses.GetStatuses<IBurningRelatedStatus>();
            foreach (var burningRelatesStatus in burningRelatesStatuses)
            {
                result += burningRelatesStatus.SelfExtinguishChanceModifier;
            }

            return result;
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

        public virtual void Damage(Journal journal, int damage, Element element = Element.Physical)
        {
            if (damage < 0)
                throw new ArgumentException($"Damage should be greater or equal 0. Got {damage}.");

            Health -= damage;

            if (element == Element.Fire)
            {
                CheckCatchFire(damage, journal);
            }

            damageRecords.Add(Injector.Current.Create<IDamageRecord>(damage, element));
        }

        public void ClearDamageRecords()
        {
            damageRecords.Clear();
        }

        private OnFireObjectStatusConfiguration GetFireConfiguration()
        {
            return new OnFireObjectStatusConfiguration
            {
                BurnBeforeExtinguishCheck = 3
            };
        }

        private void CheckCatchFire(int damage, Journal journal)
        {
            var catchFireChance = damage * CatchFireChanceMultiplier;

            var burningRelatesStatuses = Statuses.GetStatuses<IBurningRelatedStatus>();
            foreach (var burningRelatesStatus in burningRelatesStatuses)
            {
                catchFireChance += burningRelatesStatus.CatchFireChanceModifier;
            }

            if (RandomHelper.CheckChance(catchFireChance))
            {
                Statuses.Add(new OnFireObjectStatus(GetFireConfiguration()), journal);
            }
        }

        protected virtual IMapObject CreateDeathRemains()
        {
            return null;
        }

        public virtual void OnDeath(IAreaMap map, Point position)
        {
            var remains = CreateDeathRemains();
            if (remains != null)
            {
                map.AddObject(position, remains);
            }
        }

        public bool Equals(IMapObject other)
        {
            if (other is DestroyableObject destroyable)
            {
                return string.Equals(Id, destroyable.Id);
            }

            return false;
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
            ZIndex = ZIndex.BigDecoration;
        }

        public string Name { get; set; }

        public int Health { get; set; }

        public int MaxHealth { get; set; }

        public int CatchFireChanceMultiplier { get; set; }

        public int SelfExtinguishChance { get; set; }

        public ZIndex ZIndex { get; set; }
    }
}