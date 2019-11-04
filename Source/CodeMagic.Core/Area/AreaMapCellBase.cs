using System.Linq;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area
{
    public abstract class AreaMapCellBase : IAreaMapCell
    {
        protected AreaMapCellBase(IEnvironment environment)
        {
            ObjectsCollection = new MapObjectsCollection();
            LightLevel = LightLevel.Darkness;
            Environment = environment;
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

        public abstract bool HasRoof { get; }

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