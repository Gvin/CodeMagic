using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects
{
    public class DestroyableObject : IDestroyableObject
    {
        private int health;
        private int maxHealth;
        private readonly Dictionary<Element, int> baseProtection;

        public DestroyableObject(DestroyableObjectConfiguration configuration)
        {
            Id = Guid.NewGuid().ToString();

            Name = configuration.Name;
            maxHealth = configuration.MaxHealth;
            Health = configuration.Health;
            SelfExtinguishChance = configuration.SelfExtinguishChance;
            CatchFireChanceMultiplier = configuration.CatchFireChanceMultiplier;
            ZIndex = configuration.ZIndex;
            Size = configuration.Size;

            Statuses = new ObjectStatusesCollection(this);
            ObjectEffects = new List<IObjectEffect>();

            baseProtection = configuration.BaseProtection.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public virtual string Id { get; }

        public string Name { get; }

        public virtual bool BlocksMovement => false;

        public bool BlocksAttack => false;

        public virtual bool IsVisible => true;

        public virtual bool BlocksVisibility => false;

        public virtual bool BlocksProjectiles => false;

        public virtual bool BlocksEnvironment => false;

        public ObjectStatusesCollection Statuses { get; }

        private int SelfExtinguishChance { get; }

        public ZIndex ZIndex { get; }

        public ObjectSize Size { get; }

        public List<IObjectEffect> ObjectEffects { get; }

        protected virtual int GetProtection(Element element)
        {
            if (baseProtection.ContainsKey(element))
                return baseProtection[element];
            return 0;
        }

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

        public virtual int MaxHealth
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

        public virtual void Damage(IJournal journal, int damage, Element element)
        {
            if (damage < 0)
                throw new ArgumentException($"Damage should be greater or equal 0. Got {damage}.");

            var protection = GetProtection(element);
            var protectionMultiplier = (100 - protection) / 100f;

            var realDamage = (int) Math.Round(damage * protectionMultiplier);
            realDamage = Math.Max(realDamage, 0);
            if (realDamage < damage)
            {
                journal.Write(new DamageBlockedMessage(this, damage - realDamage, element));
            }

            if (realDamage == 0)
                return;

            ApplyRealDamage(realDamage);

            if (element == Element.Fire)
            {
                CheckCatchFire(realDamage, journal);
            }

            ObjectEffects.Add(Injector.Current.Create<IDamageEffect>(realDamage, element));
        }

        protected virtual void ApplyRealDamage(int damage)
        {
            Health -= damage;
        }

        public void ClearDamageRecords()
        {
            ObjectEffects.Clear();
        }

        private OnFireObjectStatusConfiguration GetFireConfiguration()
        {
            return new OnFireObjectStatusConfiguration
            {
                BurnBeforeExtinguishCheck = 3
            };
        }

        private void CheckCatchFire(int damage, IJournal journal)
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

        public virtual void OnDeath(IAreaMap map, IJournal journal, Point position)
        {
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
            Size = ObjectSize.Medium;
            BaseProtection = new Dictionary<Element, int>();
        }

        public Dictionary<Element, int> BaseProtection { get; set; }

        public string Name { get; set; }

        public int Health { get; set; }

        public int MaxHealth { get; set; }

        public int CatchFireChanceMultiplier { get; set; }

        public int SelfExtinguishChance { get; set; }

        public ZIndex ZIndex { get; set; }

        public ObjectSize Size { get; set; }
    }
}