using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.JournalMessages;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class SpikedDungeonWall : SolidWallBase, ICollideDamageObject
    {
        private const int MinDamage = 10;
        private const int MaxDamage = 30;

        public SpikedDungeonWall(SaveData data) : base(data)
        {
        }

        public SpikedDungeonWall() : base("Spiked Dungeon Wall")
        {
        }

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is SpikedDungeonWall || mapObject is DungeonWall || mapObject is DungeonDoor || mapObject is DungeonTorchWall;
        }

        protected override string ImageNormal => "Wall_Spiked";
        protected override string ImageBottom => "Wall_Spiked_Bottom";
        protected override string ImageRight => "Wall_Spiked_Right";
        protected override string ImageBottomRight => "Wall_Spiked_Bottom_Right";
        protected override string ImageCorner => "Wall_Spiked_Corner";

        public void Damage(IDestroyableObject collidedObject, Point position)
        {
            var damage = RandomHelper.GetRandomValue(MinDamage, MaxDamage);
            collidedObject.Damage(position, damage, Element.Piercing);
            CurrentGame.Journal.Write(new EnvironmentDamageMessage(collidedObject, damage, Element.Piercing), collidedObject);
        }
    }
}