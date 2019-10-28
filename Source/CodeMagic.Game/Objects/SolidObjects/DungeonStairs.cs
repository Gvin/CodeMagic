using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Locations;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class DungeonStairs : IMapObject, IUsableObject, IWorldImageProvider
    {
        private const string ImageName = "Stairs_Up";

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public bool BlocksAttack => false;

        public ZIndex ZIndex => ZIndex.BigDecoration;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public void Use(GameCore<Player> game, Point position)
        {
            if (!(game.World.CurrentLocation is DungeonLocation dungeon))
                throw new ArgumentException("Can only use stairs in dungeon locations.");

            dungeon.MoveUp(game);
        }

        public string Name => "Stairs";

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(ImageName);
        }
    }
}