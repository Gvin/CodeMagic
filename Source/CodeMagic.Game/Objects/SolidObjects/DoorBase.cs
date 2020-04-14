using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public abstract class DoorBase : WallBase, IUsableObject
    {
        protected DoorBase()
        {
            Closed = true;
        }
        protected bool Closed { get; private set; }

        public override bool BlocksMovement => Closed;

        public override bool BlocksAttack => Closed;

        public override bool BlocksProjectiles => Closed;

        public override bool BlocksVisibility => Closed;

        public override bool BlocksEnvironment => Closed;

        public void Use(CurrentGame.GameCore<Player> game, Point position)
        {
            if (!Closed && game.Map.GetCell(position).Objects.Any(obj => !obj.Equals(this) && obj.BlocksMovement))
                return;

            Closed = !Closed;
        }
    }
}