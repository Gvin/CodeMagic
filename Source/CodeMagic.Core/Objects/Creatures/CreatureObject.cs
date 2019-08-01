using System;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects.Creatures
{
    public abstract class CreatureObject : DestroyableObject, ICreatureObject
    {
        private const double ParalyzedChanceMultiplier = 5;

        private readonly int blindVisibilityRange;

        protected CreatureObject(CreatureObjectConfiguration configuration) 
            : base(configuration)
        {
            MaxVisibilityRange = configuration.VisibilityRange;
            blindVisibilityRange = configuration.BlindVisibilityRange;

            Direction = Direction.Up;
        }

        public Direction Direction { get; set; }

        public int VisibilityRange
        {
            get
            {
                if (Statuses.Contains(BlindObjectStatus.StatusType))
                    return blindVisibilityRange;

                return MaxVisibilityRange;
            }
        }

        public int MaxVisibilityRange { get; }

        public override void Damage(int damage, Element? element = null)
        {
            base.Damage(damage, element);

            if (element.HasValue && element.Value == Element.Electricity)
            {
                CheckParalyzed(damage);
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
    }

    public class CreatureObjectConfiguration : DestroyableObjectConfiguration
    {
        public CreatureObjectConfiguration()
        {
            ZIndex = ZIndex.Creature;
        }

        public int VisibilityRange { get; set; }

        public int BlindVisibilityRange { get; set; }
    }
}