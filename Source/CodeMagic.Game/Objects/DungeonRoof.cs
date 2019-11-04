using CodeMagic.Core.Objects;

namespace CodeMagic.Game.Objects
{
    /// <summary>
    /// Standard roof for dungeons which covers the whole map.
    /// Provides static roof which does not rely on walls.
    /// </summary>
    public class DungeonRoof : IRoof
    {
        public string Name => "Dungeon Roof";
        public bool BlocksMovement => false;
        public bool BlocksProjectiles => false;
        public bool IsVisible => false;
        public bool BlocksVisibility => false;
        public bool BlocksAttack => false;
        public bool BlocksEnvironment => false;
        public ZIndex ZIndex => ZIndex.Roof;
        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;
    }
}