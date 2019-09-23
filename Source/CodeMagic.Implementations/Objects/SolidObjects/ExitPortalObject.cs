using System.Drawing;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Implementations.Objects.SolidObjects
{
    public class ExitPortalObject : IMapObject, IUsableObject, IWorldImageProvider, ILightObject
    {
        private const string WorldImageName = "Mechanism_Portal";

        public string Name => "Exit Portal";

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
            game.World.TravelToLocation(game, "world", Direction.North);
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImageName);
        }

        public ILightSource[] LightSources => new ILightSource[]
        {
            new StaticLightSource(LightLevel.Dusk2, Color.Red)
        };
    }
}