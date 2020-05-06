using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Saving;
using CodeMagic.Game.JournalMessages;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Floor
{
    public class SpikedFloorObject : MapObjectBase, IStepReactionObject, IWorldImageProvider, ITrapObject
    {
        private const string WorldImageName = "Trap_SpikedFloor";
        private const int MinDamage = 2;
        private const int MaxDamage = 5;

        public SpikedFloorObject(SaveData data) : base(data)
        {
        }

        public SpikedFloorObject() : base("Spikes")
        {
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImageName);
        }

        public override ZIndex ZIndex => ZIndex.FloorCover;

        public override ObjectSize Size => ObjectSize.Huge;

        public Point ProcessStepOn(Point position, ICreatureObject target, Point initialTargetPosition)
        {
            var damage = RandomHelper.GetRandomValue(MinDamage, MaxDamage);
            target.Damage(position, damage, Element.Piercing);
            CurrentGame.Journal.Write(new DealDamageMessage(this, target, damage, Element.Piercing), target);
            return position;
        }
    }
}