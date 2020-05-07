using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class DungeonWall : SolidWallBase
    {
        public DungeonWall(SaveData data) : base(data)
        {
        }

        public DungeonWall() : base("Dungeon Wall")
        {
        }

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is DungeonWall || mapObject is DungeonTorchWall || mapObject is DungeonDoor;
        }

        protected override string ImageNormal => "Wall_Dungeon";
        protected override string ImageBottom => "Wall_Dungeon_Bottom";
        protected override string ImageRight => "Wall_Dungeon_Right";
        protected override string ImageBottomRight => "Wall_Dungeon_Bottom_Right";
        protected override string ImageCorner => "Wall_Dungeon_Corner";
    }
}