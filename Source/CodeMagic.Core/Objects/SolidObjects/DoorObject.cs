using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects.SolidObjects
{
    public abstract class DoorObject : WallBase, IUsableObject
    {
        protected DoorObject()
        {
            Closed = true;
        }
        protected bool Closed { get; private set; }

        public sealed override bool BlocksMovement => Closed;

        public sealed override bool BlocksAttack => Closed;

        public override bool BlocksProjectiles => Closed;

        public override bool BlocksVisibility => Closed;

        public override bool BlocksEnvironment => Closed;

        public void Use(IGameCore game, Point position)
        {
            if (!Closed && game.Map.GetCell(position).BlocksMovement)
                return;

            Closed = !Closed;
        }
    }
}