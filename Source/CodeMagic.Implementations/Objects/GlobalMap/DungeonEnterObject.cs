using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Locations;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.GlobalMap
{
    public class DungeonEnterObject : IMapObject, IUsableObject, IWorldImageProvider
    {
        private const string WorldImageName = "GlobalMap_Dungeon";

        public DungeonEnterObject(ItemRareness dungeonRareness)
        {
            this.dungeonRareness = dungeonRareness;
        }

        private readonly ItemRareness dungeonRareness;

        public bool BlocksMovement => true;

        public bool BlocksProjectiles => true;

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
            game.Map.RemoveObject(position, this);
            game.Map.AddObject(position, new GlobalMapObject(GlobalMapObject.Type.DungeonClosed));
            var dungeon = new DungeonLocation(dungeonRareness);
            dungeon.Initialize(game.GameTime);
            game.World.TravelToLocation(game, dungeon);
        }

        public string Name => "Dungeon Enter";

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImageName);
        }
    }
}