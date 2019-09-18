using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Locations;

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
            if (!(game.World.CurrentLocation is DungeonLocation dungeon))
                throw new ArgumentException("Can only use stairs in dungeon locations.");

            dungeon.MoveUp(game);
        }
    }
}