using System;
using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.GlobalMap
{
    public class GlobalMapObject : IMapObject, IWorldImageProvider
    {
        private readonly Type objectType;

        public GlobalMapObject(Type objectType)
        {
            this.objectType = objectType;
        }

        public string Name
        {
            get
            {
                switch (objectType)
                {
                    case Type.Grass:
                        return "Grass";
                    case Type.Wheat:
                        return "Wheat";
                    case Type.Rock:
                        return "Rock";
                    case Type.Forest:
                        return "Forest";
                    case Type.Dirt:
                        return "Dirt";
                    case Type.DungeonClosed:
                        return "Blocked Dungeon Enter";
                    default:
                        throw new ArgumentException($"Unknown object type: {objectType}");
                }
            }
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(GetWorldImageName());
        }

        private string GetWorldImageName()
        {
            switch (objectType)
            {
                case Type.Grass:
                    return "GlobalMap_Grass";
                case Type.Wheat:
                    return "GlobalMap_Wheat";
                case Type.Rock:
                    return "GlobalMap_Rock";
                case Type.Forest:
                    return "GlobalMap_Forest";
                case Type.Dirt:
                    return "GlobalMap_Dirt";
                case Type.DungeonClosed:
                    return "GlobalMap_DungeonClosed";
                default:
                    throw new ArgumentException($"Unknown object type: {objectType}");
            }
        }

        public bool BlocksAttack => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => true;

        public bool BlocksEnvironment => false;

        public bool BlocksMovement
        {
            get
            {
                switch (objectType)
                {
                    case Type.Grass:
                    case Type.Wheat:
                    case Type.Dirt:
                    case Type.Forest:
                        return false;
                    case Type.Rock:
                    case Type.DungeonClosed:
                        return true;
                    default:
                        throw new ArgumentException($"Unknown object type: {objectType}");
                }
            }
        }

        public bool BlocksVisibility
        {
            get
            {
                switch (objectType)
                {
                    case Type.Grass:
                    case Type.Wheat:
                    case Type.Dirt:
                    case Type.Forest:
                    case Type.DungeonClosed:
                        return false;
                    case Type.Rock:
                        return true;
                    default:
                        throw new ArgumentException($"Unknown object type: {objectType}");
                }
            }
        }

        public ZIndex ZIndex
        {
            get
            {
                switch (objectType)
                {
                    case Type.Grass:
                    case Type.Wheat:
                    case Type.Dirt:
                        return ZIndex.Floor;
                    case Type.Forest:
                    case Type.Rock:
                        return ZIndex.AreaDecoration;
                    case Type.DungeonClosed:
                        return ZIndex.BigDecoration;
                    default:
                        throw new ArgumentException($"Unknown object type: {objectType}");
                }
            }
        }

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public enum Type
        {
            Grass,
            Wheat,
            Rock,
            Forest,
            Dirt,
            DungeonClosed
        }
    }
}