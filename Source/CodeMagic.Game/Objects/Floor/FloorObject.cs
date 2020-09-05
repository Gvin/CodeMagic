using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Floor
{
    public class FloorObject : MapObjectBase, IWorldImageProvider
    {
        private const string SaveKeyFloorType = "FloorType";
        private const string SaveKeyImageName = "ImageName";

        private readonly Type floorType;
        private readonly string imageName;

        public FloorObject(SaveData data) 
            : base(data)
        {
            floorType = (Type) data.GetIntValue(SaveKeyFloorType);
            imageName = data.GetStringValue(SaveKeyImageName);
        }

        public FloorObject(Type floorType)
            : base(GetName(floorType))
        {
            this.floorType = floorType;
            imageName = GetWorldImageName(floorType);
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyFloorType, (int) floorType);
            data.Add(SaveKeyImageName, imageName);
            return data;
        }

        private static string GetName(Type type)
        {
            switch (type)
            {
                case Type.Stone:
                    return "Stone";
                default:
                    throw new ArgumentException($"Unknown floor type: {type}");
            }
        }

        public override ZIndex ZIndex => ZIndex.Floor;

        public override ObjectSize Size => ObjectSize.Huge;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(imageName);
        }

        private static string GetWorldImageName(Type floorType)
        {
            switch (floorType)
            {
                case Type.Stone:
                    return RandomHelper.GetRandomElement("Floor_Stone_V1", "Floor_Stone_V2", "Floor_Stone_V3");
                default:
                    throw new ArgumentException($"Unknown floor type: {floorType}");
            }
        }

        public enum Type
        {
            Stone
        }
    }
}