using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects.SolidObjects
{
    public abstract class DoorObject : IWallObject, IUsableObject
    {
        protected DoorObject(bool horizontal)
        {
            Horizontal = horizontal;
            Closed = true;
        }

        protected bool Horizontal { get; }

        public abstract string Name { get; }

        protected bool Closed { get; private set; }

        public bool BlocksMovement => Closed;

        public bool BlocksProjectiles => Closed;

        public bool IsVisible => true;

        public bool BlocksVisibility => Closed;

        public bool BlocksEnvironment => Closed;

        public ZIndex ZIndex => ZIndex.Wall;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public void Use(IGameCore game, Point position)
        {
            Closed = !Closed;
        }
    }
}