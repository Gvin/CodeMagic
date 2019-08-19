using System;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects.Creatures
{
    public abstract class CreatureObject : DestroyableObject, ICreatureObject
    {
        private const double ParalyzedChanceMultiplier = 5;
        private const double FrozenChanceMultiplier = 5;

        private readonly int blindVisibilityRange;

        protected CreatureObject(CreatureObjectConfiguration configuration) 
            : base(configuration)
        {
            MaxVisibilityRange = configuration.VisibilityRange;
            blindVisibilityRange = configuration.BlindVisibilityRange;

            Direction = Direction.North;
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

        public override void Damage(Journal journal, int damage, Element element)
        {
            base.Damage(journal, damage, element);

            switch (element)
            {
                case Element.Electricity:
                    CheckParalyzed(damage, journal);
                    break;
                case Element.Frost:
                    CheckFrozen(damage, journal);
                    break;
            }
        }

        private void CheckFrozen(int damage, Journal journal)
        {
            var chance = (int)Math.Round(damage * FrozenChanceMultiplier);
            if (RandomHelper.CheckChance(chance))
            {
                Statuses.Add(new FrozenObjectStatus(), journal);
            }
        }

        private void CheckParalyzed(int damage, Journal journal)
        {
            var chance = (int) Math.Round(damage * ParalyzedChanceMultiplier);
            if (RandomHelper.CheckChance(chance))
            {
                Statuses.Add(new ParalyzedObjectStatus(), journal);
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