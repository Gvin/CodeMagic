using System;
using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.Floor
{
    public class FloorObject : IMapObject, IWorldImageProvider
    {
        private readonly Type floorType;

        public FloorObject(Type floorType)
        {
            this.floorType = floorType;
        }

        public string Name
        {
            get
            {
                switch (floorType)
                {
                    case Type.Grass:
                        return "Grass";
                    case Type.Stone:
                        return "Stone";
                    case Type.Dirt:
                        return "Dirt";
                    case Type.Wood:
                        return "Wood";
                    default:
                        throw new ArgumentException($"Unknown floor type: {floorType}");
                }
            }
        }

        public bool BlocksAttack => false;

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public ZIndex ZIndex => ZIndex.Floor;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(GetWorldImageName());
        }

        private string GetWorldImageName()
        {
            switch (floorType)
            {
                case Type.Grass:
                    return "Floor_Grass";
                case Type.Stone:
                    return "Floor_Stone";
                case Type.Dirt:
                    return "Floor_Dirt";
                case Type.Wood:
                    return "Floor_Wood";
                default:
                    throw new ArgumentException($"Unknown floor type: {floorType}");
            }
        }

        public enum Type
        {
            Grass,
            Stone,
            Dirt,
            Wood
        }
    }
}