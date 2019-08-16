using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects.SolidObjects
{
    public abstract class StairsObject : IMapObject, IUsableObject
    {
        public abstract string Name { get; }

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public ZIndex ZIndex => ZIndex.BigDecoration;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public void Use(IGameCore game, Point position)
        {
            game.GoToNextLevel();
        }
    }
}