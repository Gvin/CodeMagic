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

        protected CreatureObject(CreatureObjectConfiguration configuration) 
            : base(configuration)
        {
            MaxVisibilityRange = configuration.VisibilityRange;

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

        public int MaxVisibilityRange { get; }

        public override void Damage(IJournal journal, int damage, Element element)
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

        private void CheckFrozen(int damage, IJournal journal)
        {
            var chance = (int)Math.Round(damage * FrozenChanceMultiplier);
            if (RandomHelper.CheckChance(chance))
            {
                Statuses.Add(new FrozenObjectStatus(), journal);
            }
        }

        private void CheckParalyzed(int damage, IJournal journal)
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
    }
}