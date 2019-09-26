using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.SolidObjects
{
    public class TreeObject : IMapObject, IWorldImageProvider
    {
        private const string WorldImageName = "Tree";

        public string Name => "Tree";

        public bool BlocksMovement => true;

        public bool BlocksProjectiles => true;

        public bool IsVisible => true;

        public bool BlocksAttack => true;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public ZIndex ZIndex => ZIndex.BigDecoration;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImageName);
        }
    }
}