using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Implementations.Objects
{
    public class LocationExitTriggerObject : IMapObject, IUsableObject
    {
        public string Name => "location_exit_trigger";

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => false;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public ZIndex ZIndex => ZIndex.Floor;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public void Use(IGameCore game, Point position)
        {
            game.World.TravelToLocation(game, "world");
        }
    }
}