using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects.Creatures
{
    public abstract class NonPlayableCreatureObject : CreatureObject, INonPlayableCreatureObject
    {
        private readonly float normalSpeed;
        private float turnsCounter;

        public NonPlayableCreatureObject(NonPlayableCreatureObjectConfiguration configuration) 
            : base(configuration)
        {
            Logic = new Logic();
            normalSpeed = configuration.Speed;
            turnsCounter = 0;
        }

        private float Speed
        {
            get
            {
                if (Statuses.Contains(FrozenObjectStatus.StatusType))
                {
                    return normalSpeed * FrozenObjectStatus.SpeedMultiplier;
                }

                return normalSpeed;
            }
        }

        public void Update(IGameCore game, Point position)
        {
            turnsCounter += 1;
            if (turnsCounter >= Speed)
            {
                Logic.Update(this, game, position);
                turnsCounter -= Speed;
            }
        }

        public virtual void Attack(IDestroyableObject target, Journal journal)
        {
        }

        public bool Updated { get; set; }

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        protected Logic Logic { get; }
    }

    public class NonPlayableCreatureObjectConfiguration : CreatureObjectConfiguration
    {
        public NonPlayableCreatureObjectConfiguration()
        {
            BlindVisibilityRange = 1;
            Speed = 1f;
        }

        /// <summary>
        /// Number of turns required to perform 1 action (less -> faster).
        /// </summary>
        public float Speed { get; set; }
    }
}