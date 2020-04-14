using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Statuses;

namespace CodeMagic.Game.Objects
{
    public abstract class DestroyableObject : IDestroyableObject
    {
        private int health;
        protected readonly Dictionary<Element, int> BaseProtection;

        protected DestroyableObject(int startHealth = 1)
        {
            Id = Guid.NewGuid().ToString();
            BaseProtection = new Dictionary<Element, int>();
            Statuses = new ObjectStatusesCollection(this);
            ObjectEffects = new List<IObjectEffect>();

            health = startHealth;
        }

        public string Id { get; }

        public abstract string Name { get; }

        public virtual bool BlocksMovement => false;

        public virtual bool BlocksAttack => false;

        public virtual bool IsVisible => true;

        public virtual bool BlocksVisibility => false;

        public virtual bool BlocksProjectiles => false;

        public virtual bool BlocksEnvironment => false;

        public IObjectStatusesCollection Statuses { get; }

        protected virtual double SelfExtinguishChance => 15;

        public abstract ZIndex ZIndex { get; }

        public abstract ObjectSize Size { get; }

        public List<IObjectEffect> ObjectEffects { get; }

        public virtual int GetProtection(Element element)
        {
            if (BaseProtection.ContainsKey(element))
                return BaseProtection[element];
            return 0;
        }

        public double GetSelfExtinguishChance()
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
            set => health = Math.Max(0, Math.Min(MaxHealth, value));
        }

        public abstract int MaxHealth { get; }

        protected virtual double CatchFireChanceMultiplier => 1;

        public virtual void Damage(Point position, int damage, Element element)
        {
            if (damage < 0)
                throw new ArgumentException($"Damage should be greater or equal 0. Got {damage}.");

            var protection = GetProtection(element);
            var protectionMultiplier = (100 - protection) / 100f;

            var realDamage = (int) Math.Round(damage * protectionMultiplier);
            realDamage = Math.Max(realDamage, 0);
            if (realDamage < damage)
            {
                CurrentGame.Journal.Write(new DamageBlockedMessage(this, damage - realDamage, element));
            }

            if (realDamage == 0)
                return;

            ApplyRealDamage(realDamage, element, position);

            ObjectEffects.Add(Injector.Current.Create<IDamageEffect>(realDamage, element));
        }

        protected virtual void ApplyRealDamage(int damage, Element element, Point position)
        {
            Health -= damage;

            if (element == Element.Fire)
            {
                CheckCatchFire(damage);
            }
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

        private void CheckCatchFire(int damage)
        {
            var catchFireChance = damage * CatchFireChanceMultiplier;

            var burningRelatesStatuses = Statuses.GetStatuses<IBurningRelatedStatus>();
            foreach (var burningRelatesStatus in burningRelatesStatuses)
            {
                catchFireChance += burningRelatesStatus.CatchFireChanceModifier;
            }

            var catchFireWholeChange = (int) Math.Round(catchFireChance);
            if (RandomHelper.CheckChance(catchFireWholeChange))
            {
                Statuses.Add(new OnFireObjectStatus(GetFireConfiguration()));
            }
        }

        public virtual void OnDeath(Point position)
        {
            CurrentGame.Journal.Write(new DeathMessage(this));
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
}