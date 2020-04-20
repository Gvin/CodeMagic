using System;
using System.Collections.Generic;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Floor
{
    public class FloorObject : MapObjectBase, IWorldImageProvider
    {
        private const string SaveKeyFloorType = "FloorType";

        private readonly Type floorType;

        public FloorObject(SaveData data) 
            : base(data)
        {
            floorType = (Type) data.GetIntValue(SaveKeyFloorType);
        }

        public FloorObject(Type floorType)
            : base(GetName(floorType))
        {
            this.floorType = floorType;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyFloorType, (int) floorType);
            return data;
        }

        private static string GetName(Type type)
        {
            switch (type)
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
                    throw new ArgumentException($"Unknown floor type: {type}");
            }
        }

        public override ZIndex ZIndex => ZIndex.Floor;

        public override ObjectSize Size => ObjectSize.Huge;

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