using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class DungeonStairs : MapObjectBase, IWorldImageProvider
    {
        private const string ImageName = "Stairs_Up";

        public DungeonStairs(SaveData data) : base(data)
        {
        }

        public DungeonStairs() : base("Stairs")
        {
        }

        public override ZIndex ZIndex => ZIndex.BigDecoration;

        public override ObjectSize Size => ObjectSize.Huge;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(ImageName);
        }
    }
}