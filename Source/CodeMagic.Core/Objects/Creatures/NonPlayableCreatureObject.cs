using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects.Creatures
{
    public abstract class NonPlayableCreatureObject : DestroyableObject, INonPlayableCreatureObject
    {
        private readonly int visibilityRange;

        public NonPlayableCreatureObject(NonPlayableCreatureObjectConfiguration configuration) 
            : base(configuration)
        {
            Logic = new Logic();
            Direction = Direction.Up;
            visibilityRange = configuration.VisibilityRange;
        }

        public Direction Direction { get; set; }

        public void Update(IGameCore game, Point position)
        {
            Logic.Update(this, game, position);

            var cell = game.Map.GetCell(position);
            if (cell.LightLevel == LightLevel.Blinding)
            {
                Statuses.Add(new BlindObjectStatus());
            }
        }

        public virtual void Attack(IDestroyableObject target, Journal journal)
        {
        }

        public bool Updated { get; set; }

        protected Logic Logic { get; }

        public int VisibilityRange
        {
            get
            {
                if (Statuses.Contains(BlindObjectStatus.StatusType))
                    return 0;

                return visibilityRange;
            }
        }
    }

    public class NonPlayableCreatureObjectConfiguration : DestroyableObjectConfiguration
    {
        public NonPlayableCreatureObjectConfiguration()
        {
            ZIndex = ZIndex.Creature;
        }

        public int VisibilityRange { get; set; }
    }
}