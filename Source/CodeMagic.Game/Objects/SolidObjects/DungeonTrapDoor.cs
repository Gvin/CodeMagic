using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Locations;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class DungeonTrapDoor : IMapObject, IUsableObject, IWorldImageProvider
    {
        private const string ImageName = "Decoratives_TrapDoor";

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksAttack => false;

        public bool BlocksEnvironment => false;

        public ZIndex ZIndex => ZIndex.BigDecoration;

        public string Name => "Trap Door";

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(ImageName);
        }

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public void Use(GameCore<Player> game, Point position)
        {
            if (!(game.World.CurrentLocation is DungeonLocation dungeon))
                throw new ArgumentException("Can only use stairs in dungeon locations.");

            dungeon.MoveDown(game);
        }
    }
}