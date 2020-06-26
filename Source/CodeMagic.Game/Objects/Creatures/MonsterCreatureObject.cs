﻿using System;
using System.Collections.Generic;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Monsters;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures.Loot;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.Objects.Creatures
{
    public abstract class MonsterCreatureObject : NonPlayableCreatureObject
    {
        private readonly ILootConfiguration lootConfiguration;
        private readonly MonsterDamageValue[] attackDamage;
        private readonly int hitChance;
        private readonly MonsterCreatureObjectConfiguration configuration;

        protected MonsterCreatureObject(SaveData data, MonsterCreatureObjectConfiguration configuration) 
            : base(data)
        {
            this.configuration = configuration;
            attackDamage = configuration.Damage.ToArray();
            hitChance = configuration.Accuracy;
            lootConfiguration = configuration.LootConfiguration;
        }

        protected MonsterCreatureObject(MonsterCreatureObjectConfiguration configuration) 
            : base(configuration.Name, configuration.MaxHealth, configuration.LogicPattern)
        {
            this.configuration = configuration;

            attackDamage = configuration.Damage.ToArray();
            hitChance = configuration.Accuracy;
            lootConfiguration = configuration.LootConfiguration;

            if (configuration.BaseProtection != null)
            {
                foreach (var pair in configuration.BaseProtection)
                {
                    BaseProtection.Add(pair.Key, pair.Value);
                }
            }

            if (configuration.StatusesImmunity != null)
            {
                StatusesImmunity.AddRange(configuration.StatusesImmunity);
            }
        }

        protected override int TryBlockMeleeDamage(Direction damageDirection, int damage, Element element)
        {
            if (!RandomHelper.CheckChance(configuration.ShieldBlockChance))
                return damage;

            var blockedDamage = Math.Min(configuration.ShieldBlocksDamage, damage);
            if (blockedDamage == 0)
                return damage;

            CurrentGame.Journal.Write(new ShieldBlockedDamageMessage(this, blockedDamage, element));
            return damage - blockedDamage;
        }

        public override int DodgeChance => configuration.DodgeChance;

        public sealed override ObjectSize Size => configuration.Size;

        public sealed override ZIndex ZIndex => configuration.ZIndex;

        protected sealed override double CatchFireChanceMultiplier => configuration.CatchFireChanceMultiplier;

        protected sealed override double SelfExtinguishChance => configuration.SelfExtinguishChance;

        public sealed override int MaxVisibilityRange => configuration.VisibilityRange;

        protected sealed override float NormalSpeed => configuration.Speed / 1;

        public override void Attack(Point position, Point targetPosition, IDestroyableObject target)
        {
            base.Attack(position, targetPosition, target);

            var currentHitChance = CalculateHitChance(hitChance);
            if (!RandomHelper.CheckChance(currentHitChance))
            {
                CurrentGame.Journal.Write(new AttackMissMessage(this, target), this);
                return;
            }

            if (RandomHelper.CheckChance(target.DodgeChance))
            {
                CurrentGame.Journal.Write(new AttackDodgedMessage(this, target));
                return;
            }

            var attackDirection = Point.GetAdjustedPointRelativeDirection(targetPosition, position);
            if (!attackDirection.HasValue)
                throw new ApplicationException("Can only attack adjusted target");

            foreach (var damageValue in attackDamage)
            {
                var value = RandomHelper.GetRandomValue(damageValue.MinValue, damageValue.MaxValue);
                target.MeleeDamage(targetPosition, attackDirection.Value, value, damageValue.Element);
                CurrentGame.Journal.Write(new DealDamageMessage(this, target, value, damageValue.Element), this);
            }
        }

        protected sealed override IMapObject GenerateRemains()
        {
            return new CreatureRemains(configuration.RemainsType);
        }

        protected override IMapObject GenerateDamageMark()
        {
            return new CreatureRemains(configuration.DamageMarkType);
        }

        protected sealed override IItem[] GenerateLoot()
        {
            return new ChancesLootGenerator(lootConfiguration).GenerateLoot();
        }

        public override void OnDeath(Point position)
        {
            base.OnDeath(position);

            var experience = RandomHelper.GetRandomValue(configuration.Experience.Min, configuration.Experience.Max);
            CurrentGame.Player.AddExperience(experience);
        }
    }

    public class MonsterCreatureObjectConfiguration
    {
        public MonsterCreatureObjectConfiguration()
        {
            Damage = new List<MonsterDamageValue>();
            BaseProtection = new Dictionary<Element, int>();
            StatusesImmunity = new List<string>();
        }

        public int Accuracy { get; set; }

        public List<MonsterDamageValue> Damage { get; }

        public int DodgeChance { get; set; }

        public ILootConfiguration LootConfiguration { get; set; }

        public string Name { get; set; }

        public string Id { get; set; }

        public string LogicPattern { get; set; }

        public IMonsterExperienceConfiguration Experience { get; set; }

        public ObjectSize Size { get; set; }

        public ZIndex ZIndex { get; set; }

        public int MaxHealth { get; set; }

        public RemainsType RemainsType { get; set; }

        public RemainsType DamageMarkType { get; set; }

        public double CatchFireChanceMultiplier { get; set; }

        public double SelfExtinguishChance { get; set; }

        public int VisibilityRange { get; set; }

        public float Speed { get; set; }

        public Dictionary<Element, int> BaseProtection { get; set; }

        public List<string> StatusesImmunity { get; set; }

        public int ShieldBlockChance { get; set; }

        public int ShieldBlocksDamage { get; set; }
    }

    public class MonsterDamageValue
    {
        public MonsterDamageValue(Element element, int minValue, int maxValue)
        {
            Element = element;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public Element Element { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }
    }
}