using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.Statuses;

namespace CodeMagic.Game.Objects.Creatures
{
    public abstract class CreatureObject : DestroyableObject, ICreatureObject
    {
        private const int BloodMarkPercentage = 20;

        protected CreatureObject(int maxHealth)
            : base(maxHealth)
        {
            Direction = Direction.North;
        }

        public Direction Direction { get; set; }

        public int VisibilityRange
        {
            get
            {
                if (Statuses.Contains(BlindObjectStatus.StatusType))
                    return 0;

                return MaxVisibilityRange;
            }
        }

        public override ZIndex ZIndex => ZIndex.Creature;

        protected virtual double ParalyzedChanceMultiplier => 5;

        protected virtual double FrozenChanceMultiplier => 5;

        public abstract int MaxVisibilityRange { get; }

        protected override void ApplyRealDamage(int damage, Element element, Point position)
        {
            base.ApplyRealDamage(damage, element, position);

            switch (element)
            {
                case Element.Electricity:
                    CheckParalyzed(damage);
                    break;
                case Element.Frost:
                    CheckFrozen(damage);
                    break;
                case Element.Blunt:
                case Element.Piercing:
                case Element.Slashing:
                    CheckDamageMark(damage, position);
                    break;
            }
        }

        protected abstract IMapObject GenerateDamageMark();

        private void CheckDamageMark(int damage, Point position)
        {
            var damagePercents = (int)Math.Round((float) damage / MaxHealth * 100);
            if (damagePercents >= BloodMarkPercentage)
            {
                CurrentGame.Map.AddObject(position, GenerateDamageMark());
            }
        }

        private void CheckFrozen(int damage)
        {
            var chance = (int)Math.Round(damage * FrozenChanceMultiplier);
            if (RandomHelper.CheckChance(chance))
            {
                Statuses.Add(new FrozenObjectStatus());
            }
        }

        private void CheckParalyzed(int damage)
        {
            var chance = (int) Math.Round(damage * ParalyzedChanceMultiplier);
            if (RandomHelper.CheckChance(chance))
            {
                Statuses.Add(new ParalyzedObjectStatus());
            }
        }

        protected int CalculateHitChance(int initialValue)
        {
            if (Statuses.Contains(BlindObjectStatus.StatusType))
            {
                return (int) Math.Round(initialValue * BlindObjectStatus.HitChanceMultiplier);
            }

            return initialValue;
        }

        public override bool BlocksMovement => true;

        public virtual void Update(Point position)
        {
            if (CurrentGame.Map.GetCell(position).LightLevel >= LightLevel.Blinding)
            {
                Statuses.Add(new BlindObjectStatus());
            }
        }

        public bool Updated { get; set; }

        public virtual UpdateOrder UpdateOrder => UpdateOrder.Medium;
    }
}