using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.GlobalMap
{
    public class HomeObject : IMapObject, IUsableObject, IWorldImageProvider
    {
        private const string WorldImageName = "GlobalMap_Home";

        public string Name => "Home";

        public bool BlocksMovement => true;

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
            game.World.TravelToLocation(game, "home");
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImageName);
        }
    }
}