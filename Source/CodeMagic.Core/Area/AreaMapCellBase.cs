using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Area
{
    public abstract class AreaMapCellBase : IAreaMapCell
    {
        private const string SaveKeyEnvironment = "Environment";
        private const string SaveKeyObjects = "Objects";
        private const string SaveKeyLightLevel = "LightLevel";

        protected AreaMapCellBase(SaveData data)
        {
            ObjectsCollection = new MapObjectsCollection(data.GetObjectsCollection<IMapObject>(SaveKeyObjects));
            LightLevel = (LightLevel)data.GetIntValue(SaveKeyLightLevel);
            Environment = data.GetObject<IEnvironment>(SaveKeyEnvironment);
        }

        protected AreaMapCellBase(IEnvironment environment)
        {
            ObjectsCollection = new MapObjectsCollection();
            LightLevel = LightLevel.Darkness;
            Environment = environment;
        }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyLightLevel, (int)LightLevel},
                {SaveKeyObjects, ObjectsCollection},
                {SaveKeyEnvironment, Environment}
            });
        }

        public IEnvironment Environment { get; }

        IMapObject[] IAreaMapCell.Objects => ObjectsCollection.ToArray();

        public int GetVolume<T>() where T : IVolumeObject
        {
            return ObjectsCollection.GetVolume<T>();
        }

        public void RemoveVolume<T>(int volume) where T : IVolumeObject
        {
            ObjectsCollection.RemoveVolume<T>(volume);
        }

        public MapObjectsCollection ObjectsCollection { get; }

        public LightLevel LightLevel { get; set; }

        public bool BlocksMovement
        {
            get { return ObjectsCollection.Any(obj => obj.BlocksMovement); }
        }

        public bool BlocksEnvironment
        {
            get { return ObjectsCollection.Any(obj => obj.BlocksEnvironment); }
        }

        public bool BlocksVisibility
        {
            get { return ObjectsCollection.Any(obj => obj.BlocksVisibility); }
        }

        public bool BlocksProjectiles
        {
            get { return ObjectsCollection.Any(obj => obj.BlocksProjectiles); }
        }

        public IDestroyableObject GetBiggestDestroyable()
        {
            var destroyable = ObjectsCollection.OfType<IDestroyableObject>().ToArray();
            var bigDestroyable = destroyable.FirstOrDefault(obj => obj.BlocksMovement);
            if (bigDestroyable != null)
                return bigDestroyable;
            return destroyable.LastOrDefault();
        }
    }
}